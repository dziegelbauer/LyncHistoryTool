using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncHistoryTool
{
    class LyncHistoryFile
    {
        public class Participant
        {
            public Int32 UnknownValue1;
            public String DisplayName;
            public Int32 UnknownValue2;
            public String SIPAddress;
        }
        public class FileBinaryData
        {
            public Int32 FileTypeID;
            public Participant Initiator = new Participant();
            public Int32 ParticipantCount;
            public List<Participant> Members = new List<Participant>();
            public Int32 Header9;
            public String Title;
            public Int32 Header10;
            public Int32 Header11;
            public String HTML1;
            public Int32 SIPHeader0;
            public Int16 SIPHeader1;
            public Byte Byte1;
            public Byte Byte2;
            public Byte Byte3;
            public Int32 SIPHeader2;
            public Int32 SIPHeader3;
            public Int32 SIPHeader4;
            //public Byte[] UnknownData1 = new Byte[4];
            public String UnknownSIP1;
            public Int32 Header12;
            public String UnknownSIP2;
            public Int32 Header13;
            public String TypeString;
            public Byte[] UnknownData2 = new Byte[21];
            public String XML1;
            public Int32 Header14;
            public String UnknownString7;
            public Int32 Header15;
            public String FileName;
            public Int32 Header16;
            public String UnknownSIP3;
            public Byte[] UnknownData3 = new Byte[13];

        }

        public FileBinaryData _Guts = new FileBinaryData();

        public bool ReadFromFile(string FilePath)
        {
            byte[] RawData = File.ReadAllBytes(FilePath);

            _Guts.FileTypeID = BitConverter.ToInt32(RawData, 0);
            _Guts.Initiator.UnknownValue1 = BitConverter.ToInt32(RawData, 4);

            byte tmp = 0;
            int counter = 8;
            StringBuilder InitiatorDisplayName = new StringBuilder();

            do
            {
                tmp = RawData[counter];
                if (tmp != 0)
                {
                    InitiatorDisplayName.Append(Convert.ToChar(tmp));
                }
                counter++;
            } while (tmp != 0);

            _Guts.Initiator.DisplayName = InitiatorDisplayName.ToString();
            _Guts.Initiator.UnknownValue2 = BitConverter.ToInt32(RawData, counter);
            counter += 4;

            StringBuilder InitiatorSIP = new StringBuilder();

            do
            {
                tmp = RawData[counter];
                if (tmp != 0)
                {
                    InitiatorSIP.Append(Convert.ToChar(tmp));
                }
                counter++;
            } while (tmp != 0);

            _Guts.Initiator.SIPAddress = InitiatorSIP.ToString();
            _Guts.ParticipantCount = BitConverter.ToInt32(RawData, counter);
            counter += 4;

            for (int i = 0; i < _Guts.ParticipantCount; i++)
            {
                Participant TempMember = new Participant();
                TempMember.UnknownValue1 = BitConverter.ToInt32(RawData, counter);
                counter += 4;

                StringBuilder MemberDisplayName = new StringBuilder();

                do
                {
                    tmp = RawData[counter];
                    if (tmp != 0)
                    {
                        MemberDisplayName.Append(Convert.ToChar(tmp));
                    }
                    counter++;
                } while (tmp != 0);

                TempMember.DisplayName = MemberDisplayName.ToString();
                TempMember.UnknownValue2 = BitConverter.ToInt32(RawData, counter);
                counter += 4;

                StringBuilder MemberSIP = new StringBuilder();

                do
                {
                    tmp = RawData[counter];
                    if (tmp != 0)
                    {
                        MemberSIP.Append(Convert.ToChar(tmp));
                    }
                    counter++;
                } while (tmp != 0);

                TempMember.SIPAddress = MemberSIP.ToString();

                _Guts.Members.Add(TempMember);
            }

            _Guts.Header9 = BitConverter.ToInt32(RawData, counter);
            counter += 4;

            StringBuilder Title = new StringBuilder();

            do
            {
                tmp = RawData[counter];
                if (tmp != 0)
                {
                    Title.Append(Convert.ToChar(tmp));
                }
                counter++;
            } while (tmp != 0);

            _Guts.Title = Title.ToString();
            _Guts.Header10 = BitConverter.ToInt32(RawData, counter);
            counter += 4;
            _Guts.Header11 = BitConverter.ToInt32(RawData, counter);
            counter += 4;

            StringBuilder HTML1 = new StringBuilder();

            do
            {
                tmp = RawData[counter];
                if (tmp != 0)
                {
                    HTML1.Append(Convert.ToChar(tmp));
                }
                counter++;
            } while (tmp != 0);

            _Guts.HTML1 = HTML1.ToString();

            _Guts.SIPHeader0 = BitConverter.ToInt32(RawData, counter);
            counter += 4;
            _Guts.SIPHeader1 = BitConverter.ToInt16(RawData, counter);
            counter += 2;
            _Guts.SIPHeader1 = RawData[counter];
            counter += 1;
            _Guts.SIPHeader1 = RawData[counter];
            counter += 1;
            _Guts.SIPHeader1 = RawData[counter];
            counter += 1;
            _Guts.SIPHeader2 = BitConverter.ToInt32(RawData, counter);
            counter += 4;
            _Guts.SIPHeader3 = BitConverter.ToInt32(RawData, counter);
            counter += 4;
            _Guts.SIPHeader4 = BitConverter.ToInt32(RawData, counter);
            counter += 4;
            //_Guts.SIPHeader5 = BitConverter.ToInt32(RawData, counter);
            //counter += 4;
            //_Guts.SIPHeader6 = BitConverter.ToInt32(RawData, counter);
            //counter += 4;

            //for (int i = 0; i < 6; i++)
            //{
            //   _Guts.UnknownData1[i] = RawData[counter + i];
            //}
            //counter += 6;

            StringBuilder UnknownSIP1 = new StringBuilder();

            do
            {
                tmp = RawData[counter];
                if (tmp != 0)
                {
                    UnknownSIP1.Append(Convert.ToChar(tmp));
                }
                counter++;
            } while (tmp != 0);

            _Guts.UnknownSIP1 = UnknownSIP1.ToString();
            _Guts.Header12 = BitConverter.ToInt32(RawData, counter);
            counter += 4;

            StringBuilder UnknownSIP2 = new StringBuilder();

            do
            {
                tmp = RawData[counter];
                if (tmp != 0)
                {
                    UnknownSIP2.Append(Convert.ToChar(tmp));
                }
                counter++;
            } while (tmp != 0);

            _Guts.UnknownSIP2 = UnknownSIP2.ToString();
            _Guts.Header13 = BitConverter.ToInt32(RawData, counter);
            counter += 4;

            StringBuilder TypeString = new StringBuilder();

            do
            {
                tmp = RawData[counter];
                if (tmp != 0)
                {
                    TypeString.Append(Convert.ToChar(tmp));
                }
                counter++;
            } while (tmp != 0);

            _Guts.TypeString = TypeString.ToString();

            for (int i = 0; i < 21; i++)
            {
                _Guts.UnknownData2[i] = RawData[counter + i];
            }
            counter += 21;

            StringBuilder XML1 = new StringBuilder();

            do
            {
                tmp = RawData[counter];
                if (tmp != 0)
                {
                    XML1.Append(Convert.ToChar(tmp));
                }
                counter++;
            } while (tmp != 0);

            _Guts.XML1 = XML1.ToString();
            _Guts.Header14 = BitConverter.ToInt32(RawData, counter);
            counter += 4;

            StringBuilder UnknownString7 = new StringBuilder();

            do
            {
                tmp = RawData[counter];
                if (tmp != 0)
                {
                    UnknownString7.Append(Convert.ToChar(tmp));
                }
                counter++;
            } while (tmp != 0);
            _Guts.UnknownString7 = UnknownString7.ToString();
            _Guts.Header15 = BitConverter.ToInt32(RawData, counter);
            counter += 4;

            StringBuilder FileName = new StringBuilder();

            do
            {
                tmp = RawData[counter];
                if (tmp != 0)
                {
                    FileName.Append(Convert.ToChar(tmp));
                }
                counter++;
            } while (tmp != 0);
            _Guts.FileName = FileName.ToString();
            _Guts.Header16 = BitConverter.ToInt32(RawData, counter);
            counter += 4;

            StringBuilder UnknownSIP3 = new StringBuilder();

            do
            {
                tmp = RawData[counter];
                if (tmp != 0)
                {
                    UnknownSIP3.Append(Convert.ToChar(tmp));
                }
                counter++;
            } while (tmp != 0);
            _Guts.UnknownSIP3 = UnknownSIP3.ToString();

            for (int i = 0; i < 13; i++)
            {
                _Guts.UnknownData3[i] = RawData[counter + i];
            }
            counter += 13;

            return true;
        }
    }
}
