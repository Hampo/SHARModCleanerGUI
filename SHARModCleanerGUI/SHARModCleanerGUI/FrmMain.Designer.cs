namespace SHARModCleanerGUI
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            SCMain = new SplitContainer();
            GBInput = new GroupBox();
            CBLogCopied = new CheckBox();
            BtnBrowseSHARFolder = new Button();
            TxtSHARFolder = new TextBox();
            LblSHARFolder = new Label();
            PBProgress = new ProgressBar();
            BtnProcess = new Button();
            CBReplaceP3DsWithDiffs = new CheckBox();
            CBConvertRSDToOGG = new CheckBox();
            CBRemoveUnchangedFiles = new CheckBox();
            BtnBrowseOutputFolder = new Button();
            TxtOutputFolder = new TextBox();
            LblOutputFolder = new Label();
            BtnBrowseInputFolder = new Button();
            TxtInputFolder = new TextBox();
            LblInputFolder = new Label();
            GBOutput = new GroupBox();
            LBLogs = new ListBox();
            CMSLogs = new ContextMenuStrip(components);
            ExportToolStripMenuItem = new ToolStripMenuItem();
            ClearToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)SCMain).BeginInit();
            SCMain.Panel1.SuspendLayout();
            SCMain.Panel2.SuspendLayout();
            SCMain.SuspendLayout();
            GBInput.SuspendLayout();
            GBOutput.SuspendLayout();
            CMSLogs.SuspendLayout();
            SuspendLayout();
            // 
            // SCMain
            // 
            SCMain.Dock = DockStyle.Fill;
            SCMain.Location = new Point(0, 0);
            SCMain.Name = "SCMain";
            // 
            // SCMain.Panel1
            // 
            SCMain.Panel1.Controls.Add(GBInput);
            // 
            // SCMain.Panel2
            // 
            SCMain.Panel2.Controls.Add(GBOutput);
            SCMain.Size = new Size(800, 450);
            SCMain.SplitterDistance = 266;
            SCMain.TabIndex = 0;
            // 
            // GBInput
            // 
            GBInput.Controls.Add(CBLogCopied);
            GBInput.Controls.Add(BtnBrowseSHARFolder);
            GBInput.Controls.Add(TxtSHARFolder);
            GBInput.Controls.Add(LblSHARFolder);
            GBInput.Controls.Add(PBProgress);
            GBInput.Controls.Add(BtnProcess);
            GBInput.Controls.Add(CBReplaceP3DsWithDiffs);
            GBInput.Controls.Add(CBConvertRSDToOGG);
            GBInput.Controls.Add(CBRemoveUnchangedFiles);
            GBInput.Controls.Add(BtnBrowseOutputFolder);
            GBInput.Controls.Add(TxtOutputFolder);
            GBInput.Controls.Add(LblOutputFolder);
            GBInput.Controls.Add(BtnBrowseInputFolder);
            GBInput.Controls.Add(TxtInputFolder);
            GBInput.Controls.Add(LblInputFolder);
            GBInput.Dock = DockStyle.Fill;
            GBInput.Location = new Point(0, 0);
            GBInput.Name = "GBInput";
            GBInput.Size = new Size(266, 450);
            GBInput.TabIndex = 0;
            GBInput.TabStop = false;
            GBInput.Text = "Input";
            // 
            // CBLogCopied
            // 
            CBLogCopied.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            CBLogCopied.AutoSize = true;
            CBLogCopied.Checked = true;
            CBLogCopied.CheckState = CheckState.Checked;
            CBLogCopied.Location = new Point(12, 361);
            CBLogCopied.Name = "CBLogCopied";
            CBLogCopied.Size = new Size(109, 19);
            CBLogCopied.TabIndex = 14;
            CBLogCopied.Text = "Log copied files";
            CBLogCopied.UseVisualStyleBackColor = true;
            // 
            // BtnBrowseSHARFolder
            // 
            BtnBrowseSHARFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnBrowseSHARFolder.Location = new Point(229, 200);
            BtnBrowseSHARFolder.Name = "BtnBrowseSHARFolder";
            BtnBrowseSHARFolder.Size = new Size(31, 23);
            BtnBrowseSHARFolder.TabIndex = 13;
            BtnBrowseSHARFolder.Text = "•••";
            BtnBrowseSHARFolder.UseVisualStyleBackColor = true;
            BtnBrowseSHARFolder.Click += BtnBrowseSHARFolder_Click;
            // 
            // TxtSHARFolder
            // 
            TxtSHARFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TxtSHARFolder.Location = new Point(12, 200);
            TxtSHARFolder.Name = "TxtSHARFolder";
            TxtSHARFolder.Size = new Size(212, 23);
            TxtSHARFolder.TabIndex = 12;
            // 
            // LblSHARFolder
            // 
            LblSHARFolder.AutoSize = true;
            LblSHARFolder.Location = new Point(6, 182);
            LblSHARFolder.Name = "LblSHARFolder";
            LblSHARFolder.Size = new Size(76, 15);
            LblSHARFolder.TabIndex = 11;
            LblSHARFolder.Text = "SHAR Folder:";
            // 
            // PBProgress
            // 
            PBProgress.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PBProgress.Location = new Point(12, 386);
            PBProgress.Name = "PBProgress";
            PBProgress.Size = new Size(248, 23);
            PBProgress.TabIndex = 10;
            // 
            // BtnProcess
            // 
            BtnProcess.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            BtnProcess.Location = new Point(12, 415);
            BtnProcess.Name = "BtnProcess";
            BtnProcess.Size = new Size(248, 23);
            BtnProcess.TabIndex = 9;
            BtnProcess.Text = "Process";
            BtnProcess.UseVisualStyleBackColor = true;
            BtnProcess.Click += BtnProcess_Click;
            // 
            // CBReplaceP3DsWithDiffs
            // 
            CBReplaceP3DsWithDiffs.AutoSize = true;
            CBReplaceP3DsWithDiffs.Checked = true;
            CBReplaceP3DsWithDiffs.CheckState = CheckState.Checked;
            CBReplaceP3DsWithDiffs.Location = new Point(12, 160);
            CBReplaceP3DsWithDiffs.Name = "CBReplaceP3DsWithDiffs";
            CBReplaceP3DsWithDiffs.Size = new Size(149, 19);
            CBReplaceP3DsWithDiffs.TabIndex = 8;
            CBReplaceP3DsWithDiffs.Text = "Replace P3Ds with Diffs";
            CBReplaceP3DsWithDiffs.UseVisualStyleBackColor = true;
            // 
            // CBConvertRSDToOGG
            // 
            CBConvertRSDToOGG.AutoSize = true;
            CBConvertRSDToOGG.Checked = true;
            CBConvertRSDToOGG.CheckState = CheckState.Checked;
            CBConvertRSDToOGG.Location = new Point(12, 135);
            CBConvertRSDToOGG.Name = "CBConvertRSDToOGG";
            CBConvertRSDToOGG.Size = new Size(134, 19);
            CBConvertRSDToOGG.TabIndex = 7;
            CBConvertRSDToOGG.Text = "Convert RSD to OGG";
            CBConvertRSDToOGG.UseVisualStyleBackColor = true;
            // 
            // CBRemoveUnchangedFiles
            // 
            CBRemoveUnchangedFiles.AutoSize = true;
            CBRemoveUnchangedFiles.Checked = true;
            CBRemoveUnchangedFiles.CheckState = CheckState.Checked;
            CBRemoveUnchangedFiles.Location = new Point(12, 110);
            CBRemoveUnchangedFiles.Name = "CBRemoveUnchangedFiles";
            CBRemoveUnchangedFiles.Size = new Size(159, 19);
            CBRemoveUnchangedFiles.TabIndex = 6;
            CBRemoveUnchangedFiles.Text = "Remove Unchanged Files";
            CBRemoveUnchangedFiles.UseVisualStyleBackColor = true;
            // 
            // BtnBrowseOutputFolder
            // 
            BtnBrowseOutputFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnBrowseOutputFolder.Location = new Point(229, 81);
            BtnBrowseOutputFolder.Name = "BtnBrowseOutputFolder";
            BtnBrowseOutputFolder.Size = new Size(31, 23);
            BtnBrowseOutputFolder.TabIndex = 5;
            BtnBrowseOutputFolder.Text = "•••";
            BtnBrowseOutputFolder.UseVisualStyleBackColor = true;
            BtnBrowseOutputFolder.Click += BtnBrowseOutputFolder_Click;
            // 
            // TxtOutputFolder
            // 
            TxtOutputFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TxtOutputFolder.Location = new Point(12, 81);
            TxtOutputFolder.Name = "TxtOutputFolder";
            TxtOutputFolder.Size = new Size(212, 23);
            TxtOutputFolder.TabIndex = 4;
            // 
            // LblOutputFolder
            // 
            LblOutputFolder.AutoSize = true;
            LblOutputFolder.Location = new Point(6, 63);
            LblOutputFolder.Name = "LblOutputFolder";
            LblOutputFolder.Size = new Size(112, 15);
            LblOutputFolder.TabIndex = 3;
            LblOutputFolder.Text = "Output Mod Folder:";
            // 
            // BtnBrowseInputFolder
            // 
            BtnBrowseInputFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnBrowseInputFolder.Location = new Point(229, 37);
            BtnBrowseInputFolder.Name = "BtnBrowseInputFolder";
            BtnBrowseInputFolder.Size = new Size(31, 23);
            BtnBrowseInputFolder.TabIndex = 2;
            BtnBrowseInputFolder.Text = "•••";
            BtnBrowseInputFolder.UseVisualStyleBackColor = true;
            BtnBrowseInputFolder.Click += BtnBrowseInputFolder_Click;
            // 
            // TxtInputFolder
            // 
            TxtInputFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TxtInputFolder.Location = new Point(12, 37);
            TxtInputFolder.Name = "TxtInputFolder";
            TxtInputFolder.Size = new Size(212, 23);
            TxtInputFolder.TabIndex = 1;
            // 
            // LblInputFolder
            // 
            LblInputFolder.AutoSize = true;
            LblInputFolder.Location = new Point(6, 19);
            LblInputFolder.Name = "LblInputFolder";
            LblInputFolder.Size = new Size(102, 15);
            LblInputFolder.TabIndex = 0;
            LblInputFolder.Text = "Input Mod Folder:";
            // 
            // GBOutput
            // 
            GBOutput.Controls.Add(LBLogs);
            GBOutput.Dock = DockStyle.Fill;
            GBOutput.Location = new Point(0, 0);
            GBOutput.Name = "GBOutput";
            GBOutput.Size = new Size(530, 450);
            GBOutput.TabIndex = 1;
            GBOutput.TabStop = false;
            GBOutput.Text = "Output";
            // 
            // LBLogs
            // 
            LBLogs.ContextMenuStrip = CMSLogs;
            LBLogs.Dock = DockStyle.Fill;
            LBLogs.FormattingEnabled = true;
            LBLogs.ItemHeight = 15;
            LBLogs.Location = new Point(3, 19);
            LBLogs.Name = "LBLogs";
            LBLogs.SelectionMode = SelectionMode.MultiExtended;
            LBLogs.Size = new Size(524, 428);
            LBLogs.TabIndex = 0;
            LBLogs.KeyDown += LBLogs_KeyDown;
            // 
            // CMSLogs
            // 
            CMSLogs.Items.AddRange(new ToolStripItem[] { ExportToolStripMenuItem, ClearToolStripMenuItem });
            CMSLogs.Name = "CMSLogs";
            CMSLogs.Size = new Size(181, 70);
            // 
            // ExportToolStripMenuItem
            // 
            ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            ExportToolStripMenuItem.Size = new Size(180, 22);
            ExportToolStripMenuItem.Text = "Export";
            ExportToolStripMenuItem.Click += ExportToolStripMenuItem_Click;
            // 
            // ClearToolStripMenuItem
            // 
            ClearToolStripMenuItem.Name = "ClearToolStripMenuItem";
            ClearToolStripMenuItem.Size = new Size(180, 22);
            ClearToolStripMenuItem.Text = "Clear";
            ClearToolStripMenuItem.Click += ClearToolStripMenuItem_Click;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(SCMain);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmMain";
            Text = "SHAR Mod Cleaner";
            Load += FrmMain_Load;
            SCMain.Panel1.ResumeLayout(false);
            SCMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SCMain).EndInit();
            SCMain.ResumeLayout(false);
            GBInput.ResumeLayout(false);
            GBInput.PerformLayout();
            GBOutput.ResumeLayout(false);
            CMSLogs.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer SCMain;
        private GroupBox GBInput;
        private GroupBox GBOutput;
        private ListBox LBLogs;
        private Button BtnBrowseInputFolder;
        private TextBox TxtInputFolder;
        private Label LblInputFolder;
        private Button BtnBrowseOutputFolder;
        private TextBox TxtOutputFolder;
        private Label LblOutputFolder;
        private ProgressBar PBProgress;
        private Button BtnProcess;
        private CheckBox CBConvertRSDToOGG;
        private CheckBox CBRemoveUnchangedFiles;
        private Button BtnBrowseSHARFolder;
        private TextBox TxtSHARFolder;
        private Label LblSHARFolder;
        private ContextMenuStrip CMSLogs;
        private ToolStripMenuItem ExportToolStripMenuItem;
        private CheckBox CBLogCopied;
        private CheckBox CBReplaceP3DsWithDiffs;
        private ToolStripMenuItem ClearToolStripMenuItem;
    }
}
