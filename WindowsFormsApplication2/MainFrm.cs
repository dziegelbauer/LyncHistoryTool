using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace LyncHistoryTool
{
    public partial class MainFrm : Form
    {
        private String HistorySpoolerPath = "";
        private ExchangeService _exchangeService = null;
        private int _pageSize = 50;
        private Folder _imHistoryFolder = null;
        private SearchResult User;
        private int sortColumn = -1;

        private class ListViewItemComparer : IComparer
        {
            private int col;
            private SortOrder order;
            public ListViewItemComparer()
            {
                col = 0;
                order = SortOrder.Ascending;
            }
            public ListViewItemComparer(int column, SortOrder order)
            {
                col = column;
                this.order = order;
            }
            public int Compare(object x, object y)
            {
                int returnVal;
                
                try
                {
                    System.DateTime firstDate = DateTime.Parse(((ListViewItem)x).SubItems[col].Text);
                    System.DateTime secondDate = DateTime.Parse(((ListViewItem)y).SubItems[col].Text);
                    
                    returnVal = DateTime.Compare(firstDate, secondDate);
                }
                
                catch
                {
                    returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
                }
                
                if (order == SortOrder.Descending)
                    returnVal *= -1;

                return returnVal;
            }
        }

        public MainFrm()
        {
            InitializeComponent();

            String SearchBase = "LDAP://DC=AREA52,DC=AFNOAPPS,DC=USAF,DC=MIL";
            String SearchFilter = String.Format("(&(objectClass=user)(objectCategory=person)(sAMAccountName={0}))", Environment.UserName);
            DirectoryEntry ADRoot = new DirectoryEntry(SearchBase);
            DirectorySearcher DirSearch = new DirectorySearcher(ADRoot, SearchFilter, new String[]{"msRTCSIP-PrimaryUserAddress","targetAddress"});

            User = DirSearch.FindOne();

            if (User != null)
            {
                String SIPAddress = User.Properties["msRTCSIP-PrimaryUserAddress"][0].ToString();

                if (Environment.OSVersion.Version.Minor == 1)
                {
                    HistorySpoolerPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Microsoft\\Communicator\\" + SIPAddress.Replace(":","_") + "\\History Spooler";
                }
                else
                {
                    HistorySpoolerPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Microsoft\\Office\\15.0\\Lync\\" + SIPAddress.Replace(":", "_") + "\\History Spooler";
                }

                if (!Directory.Exists(HistorySpoolerPath))
                {
                    if (Directory.Exists(HistorySpoolerPath.Replace("\\History Spooler", "")))
                    {
                        Directory.CreateDirectory(HistorySpoolerPath);
                    }
                    else
                    {
                        MessageBox.Show("It appears your Lync client is not configured to log IM history.\nPlease check the options dialog under the personal section in Lync 2013 or the IM section in lync 2010.");
                        Application.Exit();
                    }
                }

                FSWatch.Path = HistorySpoolerPath;
                FSWatch.EnableRaisingEvents = true;

                UpdateFileList();    
            }
            else
            {
                MessageBox.Show("Unable to locate user account");
                System.Windows.Forms.Application.Exit();
            }
        }

        private void FSWatch_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            UpdateFileList();
        }

        private void UpdateFileList()
        {
            listFiles.Items.Clear();

            foreach (String HistFile in Directory.GetFiles(HistorySpoolerPath))
            {
                FileInfo fi = new FileInfo(HistFile);

                ListViewItem lvi = new ListViewItem(new String[] { fi.Name, fi.LastWriteTime.ToShortDateString() });
                lvi.Tag = fi;
                listFiles.Items.Add(lvi);
            }
        }

        private void listFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listFiles.SelectedItems.Count != 0)
            {
                LoadConversation(((FileInfo)listFiles.SelectedItems[0].Tag).FullName);
            }
        }

        private void LoadConversation(String FilePath)
        {
            LyncHistoryFile lhf = new LyncHistoryFile();
            lhf.ReadFromFile(FilePath);

            HTMLViewer.DocumentText = lhf._Guts.HTML1;
        }

        
        private Folder FindImHistoryFolder()
        {
            FolderView folderView = new FolderView(_pageSize, 0);
            folderView.PropertySet = new PropertySet(BasePropertySet.IdOnly);
            folderView.PropertySet.Add(FolderSchema.DisplayName);
            folderView.PropertySet.Add(FolderSchema.ChildFolderCount);

            folderView.Traversal = FolderTraversal.Shallow;
            Folder imHistoryFolder = null;

            FindFoldersResults findFolderResults;
            bool foundImHistoryFolder = false;
            do
            {
                findFolderResults = _exchangeService.FindFolders(WellKnownFolderName.MsgFolderRoot, folderView);
                foreach (Folder folder in findFolderResults)
                {
                    if (folder.DisplayName.ToLower() == "conversation history")
                    {
                        imHistoryFolder = folder;
                        foundImHistoryFolder = true;
                    }
                }
                folderView.Offset += _pageSize;
            } while (findFolderResults.MoreAvailable && !foundImHistoryFolder);

            if(!foundImHistoryFolder)
            {
                imHistoryFolder = new Folder(_exchangeService);
                imHistoryFolder.DisplayName = "Conversation History";
                imHistoryFolder.Save(WellKnownFolderName.MsgFolderRoot);
            }

            return imHistoryFolder;
        }

        private void btnUploadConv_Click(object sender, EventArgs e)
        {
            FSWatch.EnableRaisingEvents = false;
            ImportProgressDialog ipd = new ImportProgressDialog();
            AddOwnedForm(ipd);
            //ipd.Parent = this;

            //if (listFiles.SelectedItems.Count != 0)
            //{
            ipd.Show();
            int FileCounter = 0;
            String[] FileList = Directory.GetFiles(HistorySpoolerPath);
            int FileCount = FileList.Count();

            ipd.prgImportProgress.Maximum = FileCount;

            foreach (String HistFile in FileList)
            {
                FileCounter++;

                FileInfo fi = new FileInfo(HistFile);
                
                ipd.prgImportProgress.Value = FileCounter;
                ipd.lblProgress.Text = String.Format("Processing file {0} of {1}...", FileCounter, FileCount);
                ipd.lblStatus.Text = String.Format("Uploading {0} ({1} bytes)...", fi.Name, fi.Length);

                if (_exchangeService == null)
                {
                    _exchangeService = new ExchangeService();

                    var store = new X509Store(StoreLocation.CurrentUser);
                    store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
                    X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindByExtension, "2.5.29.37", true);
                    _exchangeService.Url = new Uri("https://autodiscover.mail.mil/EWS/Exchange.asmx");

                    X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(certs, "DEE Certificate Select", "Select a certificate to connect to DISA E-mail", X509SelectionFlag.SingleSelection);

                    _exchangeService.Credentials = new ClientCertificateCredentials(scollection);
                }

                if (_imHistoryFolder == null)
                {
                    _imHistoryFolder = FindImHistoryFolder();
                }

                LyncHistoryFile lhf = new LyncHistoryFile();
                lhf.ReadFromFile(fi.FullName);
                //lhf.ReadFromFile(((FileInfo)listFiles.SelectedItems[0].Tag).FullName);

                EmailMessage ConversationItem = new EmailMessage(_exchangeService);

                //ConversationItem.MimeContent = new MimeContent("UTF-8",File.ReadAllBytes(((FileInfo)listFiles.SelectedItems[0].Tag).FullName));
                
                foreach(var Member in lhf._Guts.Members)
                {
                    ConversationItem.ToRecipients.Add(new EmailAddress(Member.SIPAddress));
                }

                ConversationItem.Body = lhf._Guts.HTML1;
                ConversationItem.Subject = lhf._Guts.Title;
                ConversationItem.From = new EmailAddress(lhf._Guts.Initiator.SIPAddress);
                
                ExtendedPropertyDefinition PR_MESSAGE_FLAGS_msgflag_read = new ExtendedPropertyDefinition(3591, MapiPropertyType.Integer);
                //ExtendedPropertyDefinition PR_RECEIPT_TIME = new ExtendedPropertyDefinition(0x002A, MapiPropertyType.SystemTime);
                ConversationItem.SetExtendedProperty(PR_MESSAGE_FLAGS_msgflag_read, 1);
                //ConversationItem.SetExtendedProperty(PR_RECEIPT_TIME, ((FileInfo)listFiles.SelectedItems[0].Tag).CreationTime);

                ConversationItem.Save(_imHistoryFolder.Id);

                if (btnLocalCopies.Checked)
                {
                    if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\LyncArchive\\"))
                    {
                        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\LyncArchive\\");
                    }
                    try
                    {
                        File.Move(HistFile, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\LyncArchive\\" + fi.Name);
                    }
                    catch
                    {
                        File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\LyncArchive\\" + fi.Name);
                        File.Move(HistFile, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\LyncArchive\\" + fi.Name);
                    }
                }
                else
                {
                    File.Delete(HistFile);
                }
                //LoadConversation(((FileInfo)listFiles.SelectedItems[0].Tag).FullName);
            }

            ipd.Close();

            UpdateFileList();
            HTMLViewer.DocumentText = "";

            FSWatch.EnableRaisingEvents = true;
        }

        static bool SetProperty(EmailMessage message, PropertyDefinition propertyDefinition, object value)
        {
            if (message == null)
                return false;

            // get value of PropertyBag property — that is wrapper
            // over dictionary of inner message’s properties
            var members = message.GetType().FindMembers(MemberTypes.Property, BindingFlags.NonPublic | BindingFlags.Instance,PartialName,"PropertyBag");
            if (members.Length < 1)
                return false;

            var propertyInfo = members[0] as PropertyInfo;
            if (propertyInfo == null)
                return false;

            var bag = propertyInfo.GetValue(message, null);
            members = bag.GetType().FindMembers(MemberTypes.Property,BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,PartialName,"Properties");
            if (members.Length < 1)
                return false;

            // get dictionary of properties values
            var properties = ((PropertyInfo)members[0]).GetMethod.Invoke(bag, null);
            var dictionary = properties as Dictionary<PropertyDefinition, object>;
            if (dictionary == null)
                return false;

            dictionary[propertyDefinition] = value;
            return true;
        }

        static bool PartialName(MemberInfo info, Object part)
        {
            // Test whether the name of the candidate member contains the
            // specified partial name.
            return info.Name.Contains(part.ToString());
        }

        private void listFiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine whether the column is the same as the last column clicked.
            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.
                sortColumn = e.Column;
                // Set the sort order to ascending by default.
                listFiles.Sorting = SortOrder.Ascending;
            }
            else
            {
                // Determine what the last sort order was and change it.
                if (listFiles.Sorting == SortOrder.Ascending)
                    listFiles.Sorting = SortOrder.Descending;
                else
                    listFiles.Sorting = SortOrder.Ascending;
            }

            // Call the sort method to manually sort.
            listFiles.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer
            // object.
            listFiles.ListViewItemSorter = new ListViewItemComparer(e.Column, listFiles.Sorting);
        }
    }
}
