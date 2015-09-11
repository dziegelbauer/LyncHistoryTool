namespace LyncHistoryTool
{
    partial class MainFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.FSWatch = new System.IO.FileSystemWatcher();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnUploadConv = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listFiles = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.HTMLViewer = new System.Windows.Forms.WebBrowser();
            this.btnLocalCopies = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.FSWatch)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FSWatch
            // 
            this.FSWatch.EnableRaisingEvents = true;
            this.FSWatch.NotifyFilter = System.IO.NotifyFilters.LastWrite;
            this.FSWatch.SynchronizingObject = this;
            this.FSWatch.Changed += new System.IO.FileSystemEventHandler(this.FSWatch_Changed);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUploadConv,
            this.btnLocalCopies});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(918, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnUploadConv
            // 
            this.btnUploadConv.Image = ((System.Drawing.Image)(resources.GetObject("btnUploadConv.Image")));
            this.btnUploadConv.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUploadConv.Name = "btnUploadConv";
            this.btnUploadConv.Size = new System.Drawing.Size(132, 22);
            this.btnUploadConv.Text = "Upload to Exchange";
            this.btnUploadConv.Click += new System.EventHandler(this.btnUploadConv_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listFiles);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.HTMLViewer);
            this.splitContainer1.Size = new System.Drawing.Size(918, 373);
            this.splitContainer1.SplitterDistance = 236;
            this.splitContainer1.TabIndex = 1;
            // 
            // listFiles
            // 
            this.listFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listFiles.Location = new System.Drawing.Point(0, 0);
            this.listFiles.MultiSelect = false;
            this.listFiles.Name = "listFiles";
            this.listFiles.Size = new System.Drawing.Size(236, 373);
            this.listFiles.TabIndex = 0;
            this.listFiles.UseCompatibleStateImageBehavior = false;
            this.listFiles.View = System.Windows.Forms.View.Details;
            this.listFiles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listFiles_ColumnClick);
            this.listFiles.SelectedIndexChanged += new System.EventHandler(this.listFiles_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "File Name";
            this.columnHeader1.Width = 122;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Modified Date";
            this.columnHeader2.Width = 95;
            // 
            // HTMLViewer
            // 
            this.HTMLViewer.AllowWebBrowserDrop = false;
            this.HTMLViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HTMLViewer.Location = new System.Drawing.Point(0, 0);
            this.HTMLViewer.MinimumSize = new System.Drawing.Size(20, 20);
            this.HTMLViewer.Name = "HTMLViewer";
            this.HTMLViewer.Size = new System.Drawing.Size(678, 373);
            this.HTMLViewer.TabIndex = 0;
            this.HTMLViewer.WebBrowserShortcutsEnabled = false;
            // 
            // btnLocalCopies
            // 
            this.btnLocalCopies.Checked = true;
            this.btnLocalCopies.CheckOnClick = true;
            this.btnLocalCopies.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnLocalCopies.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLocalCopies.Image = ((System.Drawing.Image)(resources.GetObject("btnLocalCopies.Image")));
            this.btnLocalCopies.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLocalCopies.Name = "btnLocalCopies";
            this.btnLocalCopies.Size = new System.Drawing.Size(109, 22);
            this.btnLocalCopies.Text = "Retain local copies";
            this.btnLocalCopies.ToolTipText = "Check this button to keep a local repository of .hist files in %userprofile%\\Docu" +
    "ments\\LyncArchive";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 398);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainFrm";
            this.Text = "Lync History Tool";
            ((System.ComponentModel.ISupportInitialize)(this.FSWatch)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.FileSystemWatcher FSWatch;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listFiles;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnUploadConv;
        private System.Windows.Forms.WebBrowser HTMLViewer;
        private System.Windows.Forms.ToolStripButton btnLocalCopies;
    }
}

