namespace Cloud_Element_Test_Form
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripConnectionSecrets = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveCurrentSecretsAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSecretsFromToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTxtConnectionNow = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnPing = new System.Windows.Forms.ToolStripButton();
            this.tsBtnUpload = new System.Windows.Forms.ToolStripDropDownButton();
            this.uploadOneFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadSubtreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnNewFolder = new System.Windows.Forms.ToolStripButton();
            this.tsTxtFolderName = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tstxtTagData = new System.Windows.Forms.ToolStripTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpAuthorize = new System.Windows.Forms.TabPage();
            this.cmdWorkFolder = new System.Windows.Forms.Button();
            this.txtWorkFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtExtraThing = new System.Windows.Forms.TextBox();
            this.cmdApply = new System.Windows.Forms.Button();
            this.txtUserKey = new System.Windows.Forms.TextBox();
            this.txtElementKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tpContents = new System.Windows.Forms.TabPage();
            this.cmdGetFN = new System.Windows.Forms.Button();
            this.cmdGetID = new System.Windows.Forms.Button();
            this.btnGetPriorFolder = new System.Windows.Forms.Button();
            this.chkWithTags = new System.Windows.Forms.CheckBox();
            this.dgFolderContents = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifiedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.directoryDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.HasTags = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FolderRowContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsGetThisFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsGetPriorFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsDeleteFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsRemoveEmptyFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.tsTxtObjectName = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsFileTagInfoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsGetFileLink = new System.Windows.Forms.ToolStripMenuItem();
            this.tsGetFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getMetadataByPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getMetadataByIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cloudFileBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtFolderPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdGetFolderContents = new System.Windows.Forms.Button();
            this.tpLog = new System.Windows.Forms.TabPage();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.tpTest = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.spnRequestsPerSecond = new System.Windows.Forms.NumericUpDown();
            this.chkSerializeGetFileInfoReq = new System.Windows.Forms.CheckBox();
            this.cmdForceClean = new System.Windows.Forms.Button();
            this.chkAutoSaveLog = new System.Windows.Forms.CheckBox();
            this.cmdTestClearLog = new System.Windows.Forms.Button();
            this.chkTestCleanup = new System.Windows.Forms.CheckBox();
            this.tbTestOutput = new System.Windows.Forms.TextBox();
            this.cmdTestButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openSecretsFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveSecretsFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserUploadTree = new System.Windows.Forms.FolderBrowserDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpAuthorize.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tpContents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgFolderContents)).BeginInit();
            this.FolderRowContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cloudFileBindingSource)).BeginInit();
            this.tpLog.SuspendLayout();
            this.tpTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnRequestsPerSecond)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 405);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(977, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(962, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripConnectionSecrets,
            this.toolStripSeparator4,
            this.tsBtnPing,
            this.tsBtnUpload,
            this.toolStripSeparator3,
            this.tsBtnNewFolder,
            this.tsTxtFolderName,
            this.toolStripSeparator5,
            this.toolStripButton1,
            this.tstxtTagData});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(977, 31);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripConnectionSecrets
            // 
            this.toolStripConnectionSecrets.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripConnectionSecrets.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveCurrentSecretsAsToolStripMenuItem,
            this.loadSecretsFromToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripTxtConnectionNow});
            this.toolStripConnectionSecrets.Image = ((System.Drawing.Image)(resources.GetObject("toolStripConnectionSecrets.Image")));
            this.toolStripConnectionSecrets.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripConnectionSecrets.Name = "toolStripConnectionSecrets";
            this.toolStripConnectionSecrets.Size = new System.Drawing.Size(37, 28);
            this.toolStripConnectionSecrets.Text = "toolStripDropDownButton1";
            // 
            // saveCurrentSecretsAsToolStripMenuItem
            // 
            this.saveCurrentSecretsAsToolStripMenuItem.Enabled = false;
            this.saveCurrentSecretsAsToolStripMenuItem.Name = "saveCurrentSecretsAsToolStripMenuItem";
            this.saveCurrentSecretsAsToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.saveCurrentSecretsAsToolStripMenuItem.Text = "Save Current Secrets As...";
            this.saveCurrentSecretsAsToolStripMenuItem.Click += new System.EventHandler(this.saveCurrentSecretsAsToolStripMenuItem_Click);
            // 
            // loadSecretsFromToolStripMenuItem
            // 
            this.loadSecretsFromToolStripMenuItem.Name = "loadSecretsFromToolStripMenuItem";
            this.loadSecretsFromToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.loadSecretsFromToolStripMenuItem.Text = "Load Secrets from...";
            this.loadSecretsFromToolStripMenuItem.Click += new System.EventHandler(this.loadSecretsFromToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(203, 6);
            // 
            // toolStripTxtConnectionNow
            // 
            this.toolStripTxtConnectionNow.Name = "toolStripTxtConnectionNow";
            this.toolStripTxtConnectionNow.ReadOnly = true;
            this.toolStripTxtConnectionNow.Size = new System.Drawing.Size(100, 23);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 31);
            // 
            // tsBtnPing
            // 
            this.tsBtnPing.Enabled = false;
            this.tsBtnPing.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnPing.Image")));
            this.tsBtnPing.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsBtnPing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnPing.Name = "tsBtnPing";
            this.tsBtnPing.Size = new System.Drawing.Size(59, 28);
            this.tsBtnPing.Text = "Ping";
            this.tsBtnPing.Click += new System.EventHandler(this.tsBtnPing_Click);
            // 
            // tsBtnUpload
            // 
            this.tsBtnUpload.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uploadOneFileToolStripMenuItem,
            this.uploadSubtreeToolStripMenuItem});
            this.tsBtnUpload.Image = global::Cloud_Element_Test_Form.Properties.Resources.External_Hard_Drive_Move_Up;
            this.tsBtnUpload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnUpload.Name = "tsBtnUpload";
            this.tsBtnUpload.Size = new System.Drawing.Size(99, 28);
            this.tsBtnUpload.Text = "Uploading";
            // 
            // uploadOneFileToolStripMenuItem
            // 
            this.uploadOneFileToolStripMenuItem.Name = "uploadOneFileToolStripMenuItem";
            this.uploadOneFileToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.uploadOneFileToolStripMenuItem.Text = "Upload one file ...";
            this.uploadOneFileToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // uploadSubtreeToolStripMenuItem
            // 
            this.uploadSubtreeToolStripMenuItem.Name = "uploadSubtreeToolStripMenuItem";
            this.uploadSubtreeToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.uploadSubtreeToolStripMenuItem.Text = "Upload subtree ...";
            this.uploadSubtreeToolStripMenuItem.Click += new System.EventHandler(this.uploadSubtreeToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
            // 
            // tsBtnNewFolder
            // 
            this.tsBtnNewFolder.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnNewFolder.Image")));
            this.tsBtnNewFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnNewFolder.Name = "tsBtnNewFolder";
            this.tsBtnNewFolder.Size = new System.Drawing.Size(82, 28);
            this.tsBtnNewFolder.Text = "+ Folder:";
            this.tsBtnNewFolder.Click += new System.EventHandler(this.tsBtnNewFolder_Click);
            // 
            // tsTxtFolderName
            // 
            this.tsTxtFolderName.Name = "tsTxtFolderName";
            this.tsTxtFolderName.Size = new System.Drawing.Size(155, 31);
            this.tsTxtFolderName.Text = "New API Test Folder";
            this.tsTxtFolderName.ToolTipText = "Name for new Folder (created in current folder)";
            this.tsTxtFolderName.Leave += new System.EventHandler(this.tsTxtFolderName_Leave);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(68, 28);
            this.toolStripButton1.Text = " + Tag";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click_1);
            // 
            // tstxtTagData
            // 
            this.tstxtTagData.Name = "tstxtTagData";
            this.tstxtTagData.Size = new System.Drawing.Size(100, 31);
            this.tstxtTagData.Tag = "Type the tag to add.  If includes an = is treated as a KV pair; Try: *TestTag for" +
    " a random tag or sfKey=* for a guid tag";
            this.tstxtTagData.Text = "*TestTag";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpAuthorize);
            this.tabControl1.Controls.Add(this.tpContents);
            this.tabControl1.Controls.Add(this.tpLog);
            this.tabControl1.Controls.Add(this.tpTest);
            this.tabControl1.Location = new System.Drawing.Point(13, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(952, 364);
            this.tabControl1.TabIndex = 4;
            // 
            // tpAuthorize
            // 
            this.tpAuthorize.Controls.Add(this.cmdWorkFolder);
            this.tpAuthorize.Controls.Add(this.txtWorkFolder);
            this.tpAuthorize.Controls.Add(this.label4);
            this.tpAuthorize.Controls.Add(this.groupBox1);
            this.tpAuthorize.Location = new System.Drawing.Point(4, 22);
            this.tpAuthorize.Name = "tpAuthorize";
            this.tpAuthorize.Padding = new System.Windows.Forms.Padding(3);
            this.tpAuthorize.Size = new System.Drawing.Size(944, 338);
            this.tpAuthorize.TabIndex = 2;
            this.tpAuthorize.Text = "Setup";
            this.tpAuthorize.UseVisualStyleBackColor = true;
            // 
            // cmdWorkFolder
            // 
            this.cmdWorkFolder.Location = new System.Drawing.Point(487, 133);
            this.cmdWorkFolder.Name = "cmdWorkFolder";
            this.cmdWorkFolder.Size = new System.Drawing.Size(26, 23);
            this.cmdWorkFolder.TabIndex = 7;
            this.cmdWorkFolder.Text = "...";
            this.cmdWorkFolder.UseVisualStyleBackColor = true;
            this.cmdWorkFolder.Click += new System.EventHandler(this.cmdWorkFolder_Click);
            // 
            // txtWorkFolder
            // 
            this.txtWorkFolder.Location = new System.Drawing.Point(87, 135);
            this.txtWorkFolder.Name = "txtWorkFolder";
            this.txtWorkFolder.Size = new System.Drawing.Size(394, 20);
            this.txtWorkFolder.TabIndex = 6;
            this.txtWorkFolder.Text = "c:\\temp\\";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Work Path";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtExtraThing);
            this.groupBox1.Controls.Add(this.cmdApply);
            this.groupBox1.Controls.Add(this.txtUserKey);
            this.groupBox1.Controls.Add(this.txtElementKey);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(18, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(587, 109);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "API Authorization Secrets";
            // 
            // txtExtraThing
            // 
            this.txtExtraThing.Location = new System.Drawing.Point(69, 73);
            this.txtExtraThing.Name = "txtExtraThing";
            this.txtExtraThing.Size = new System.Drawing.Size(394, 20);
            this.txtExtraThing.TabIndex = 5;
            this.txtExtraThing.Text = "(from file)";
            // 
            // cmdApply
            // 
            this.cmdApply.Location = new System.Drawing.Point(506, 44);
            this.cmdApply.Name = "cmdApply";
            this.cmdApply.Size = new System.Drawing.Size(75, 23);
            this.cmdApply.TabIndex = 4;
            this.cmdApply.Text = "Apply";
            this.cmdApply.UseVisualStyleBackColor = true;
            this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
            // 
            // txtUserKey
            // 
            this.txtUserKey.Location = new System.Drawing.Point(69, 47);
            this.txtUserKey.Name = "txtUserKey";
            this.txtUserKey.Size = new System.Drawing.Size(394, 20);
            this.txtUserKey.TabIndex = 3;
            this.txtUserKey.Text = "(from file)";
            // 
            // txtElementKey
            // 
            this.txtElementKey.Location = new System.Drawing.Point(69, 23);
            this.txtElementKey.Name = "txtElementKey";
            this.txtElementKey.Size = new System.Drawing.Size(394, 20);
            this.txtElementKey.TabIndex = 2;
            this.txtElementKey.Text = "(from file)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "User";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Element";
            // 
            // tpContents
            // 
            this.tpContents.Controls.Add(this.cmdGetFN);
            this.tpContents.Controls.Add(this.cmdGetID);
            this.tpContents.Controls.Add(this.btnGetPriorFolder);
            this.tpContents.Controls.Add(this.chkWithTags);
            this.tpContents.Controls.Add(this.dgFolderContents);
            this.tpContents.Controls.Add(this.txtFolderPath);
            this.tpContents.Controls.Add(this.label3);
            this.tpContents.Controls.Add(this.cmdGetFolderContents);
            this.tpContents.Location = new System.Drawing.Point(4, 22);
            this.tpContents.Name = "tpContents";
            this.tpContents.Padding = new System.Windows.Forms.Padding(3);
            this.tpContents.Size = new System.Drawing.Size(944, 338);
            this.tpContents.TabIndex = 0;
            this.tpContents.Text = "Contents";
            this.tpContents.UseVisualStyleBackColor = true;
            // 
            // cmdGetFN
            // 
            this.cmdGetFN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdGetFN.Enabled = false;
            this.cmdGetFN.Image = global::Cloud_Element_Test_Form.Properties.Resources.Folder_Refresh;
            this.cmdGetFN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdGetFN.Location = new System.Drawing.Point(768, 6);
            this.cmdGetFN.Name = "cmdGetFN";
            this.cmdGetFN.Size = new System.Drawing.Size(69, 35);
            this.cmdGetFN.TabIndex = 10;
            this.cmdGetFN.Text = "Path";
            this.cmdGetFN.UseVisualStyleBackColor = true;
            this.cmdGetFN.Click += new System.EventHandler(this.cmdGetFN_click);
            // 
            // cmdGetID
            // 
            this.cmdGetID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdGetID.Enabled = false;
            this.cmdGetID.Image = global::Cloud_Element_Test_Form.Properties.Resources.Folder_Refresh;
            this.cmdGetID.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdGetID.Location = new System.Drawing.Point(693, 6);
            this.cmdGetID.Name = "cmdGetID";
            this.cmdGetID.Size = new System.Drawing.Size(69, 35);
            this.cmdGetID.TabIndex = 9;
            this.cmdGetID.Text = " ID";
            this.cmdGetID.UseVisualStyleBackColor = true;
            this.cmdGetID.Click += new System.EventHandler(this.cmdGetID_Click);
            // 
            // btnGetPriorFolder
            // 
            this.btnGetPriorFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetPriorFolder.Enabled = false;
            this.btnGetPriorFolder.Image = global::Cloud_Element_Test_Form.Properties.Resources.Folder_Move_Up;
            this.btnGetPriorFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGetPriorFolder.Location = new System.Drawing.Point(616, 6);
            this.btnGetPriorFolder.Name = "btnGetPriorFolder";
            this.btnGetPriorFolder.Size = new System.Drawing.Size(71, 35);
            this.btnGetPriorFolder.TabIndex = 8;
            this.btnGetPriorFolder.Text = " Up";
            this.btnGetPriorFolder.UseVisualStyleBackColor = true;
            this.btnGetPriorFolder.Click += new System.EventHandler(this.GetPriorFolder_Click);
            // 
            // chkWithTags
            // 
            this.chkWithTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkWithTags.AutoSize = true;
            this.chkWithTags.Checked = true;
            this.chkWithTags.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWithTags.Location = new System.Drawing.Point(843, 29);
            this.chkWithTags.Name = "chkWithTags";
            this.chkWithTags.Size = new System.Drawing.Size(95, 17);
            this.chkWithTags.TabIndex = 7;
            this.chkWithTags.Text = "...also get tags";
            this.chkWithTags.UseVisualStyleBackColor = true;
            // 
            // dgFolderContents
            // 
            this.dgFolderContents.AllowUserToAddRows = false;
            this.dgFolderContents.AllowUserToDeleteRows = false;
            this.dgFolderContents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgFolderContents.AutoGenerateColumns = false;
            this.dgFolderContents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgFolderContents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.sizeDataGridViewTextBoxColumn,
            this.createdDateDataGridViewTextBoxColumn,
            this.modifiedDateDataGridViewTextBoxColumn,
            this.directoryDataGridViewCheckBoxColumn,
            this.HasTags,
            this.pathDataGridViewTextBoxColumn,
            this.idDataGridViewTextBoxColumn});
            this.dgFolderContents.ContextMenuStrip = this.FolderRowContextMenu;
            this.dgFolderContents.DataSource = this.cloudFileBindingSource;
            this.dgFolderContents.Location = new System.Drawing.Point(6, 52);
            this.dgFolderContents.Name = "dgFolderContents";
            this.dgFolderContents.Size = new System.Drawing.Size(932, 280);
            this.dgFolderContents.TabIndex = 6;
            this.dgFolderContents.DoubleClick += new System.EventHandler(this.dgFolderContents_DoubleClick);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column1.DataPropertyName = "name";
            this.Column1.HeaderText = "Name";
            this.Column1.Name = "Column1";
            this.Column1.Width = 60;
            // 
            // sizeDataGridViewTextBoxColumn
            // 
            this.sizeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.sizeDataGridViewTextBoxColumn.DataPropertyName = "size";
            this.sizeDataGridViewTextBoxColumn.HeaderText = "size";
            this.sizeDataGridViewTextBoxColumn.Name = "sizeDataGridViewTextBoxColumn";
            this.sizeDataGridViewTextBoxColumn.Width = 5;
            // 
            // createdDateDataGridViewTextBoxColumn
            // 
            this.createdDateDataGridViewTextBoxColumn.DataPropertyName = "createdDate";
            this.createdDateDataGridViewTextBoxColumn.HeaderText = "createdDate";
            this.createdDateDataGridViewTextBoxColumn.Name = "createdDateDataGridViewTextBoxColumn";
            // 
            // modifiedDateDataGridViewTextBoxColumn
            // 
            this.modifiedDateDataGridViewTextBoxColumn.DataPropertyName = "modifiedDate";
            this.modifiedDateDataGridViewTextBoxColumn.HeaderText = "modifiedDate";
            this.modifiedDateDataGridViewTextBoxColumn.Name = "modifiedDateDataGridViewTextBoxColumn";
            // 
            // directoryDataGridViewCheckBoxColumn
            // 
            this.directoryDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.directoryDataGridViewCheckBoxColumn.DataPropertyName = "directory";
            this.directoryDataGridViewCheckBoxColumn.HeaderText = "Folder";
            this.directoryDataGridViewCheckBoxColumn.Name = "directoryDataGridViewCheckBoxColumn";
            this.directoryDataGridViewCheckBoxColumn.Width = 42;
            // 
            // HasTags
            // 
            this.HasTags.DataPropertyName = "HasTags";
            this.HasTags.HeaderText = "Tagged";
            this.HasTags.Name = "HasTags";
            this.HasTags.ReadOnly = true;
            // 
            // pathDataGridViewTextBoxColumn
            // 
            this.pathDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.pathDataGridViewTextBoxColumn.DataPropertyName = "path";
            this.pathDataGridViewTextBoxColumn.FillWeight = 55F;
            this.pathDataGridViewTextBoxColumn.HeaderText = "path";
            this.pathDataGridViewTextBoxColumn.Name = "pathDataGridViewTextBoxColumn";
            this.pathDataGridViewTextBoxColumn.Width = 53;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.idDataGridViewTextBoxColumn.DataPropertyName = "id";
            this.idDataGridViewTextBoxColumn.HeaderText = "id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.Width = 5;
            // 
            // FolderRowContextMenu
            // 
            this.FolderRowContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsGetThisFolder,
            this.tsGetPriorFolder,
            this.tsDeleteFolderMenuItem,
            this.tsRemoveEmptyFolders,
            this.tsTxtObjectName,
            this.toolStripSeparator1,
            this.tsFileTagInfoMenuItem,
            this.toolStripSeparator2,
            this.tsGetFileLink,
            this.tsGetFileMenuItem,
            this.getMetadataByPathToolStripMenuItem,
            this.getMetadataByIDToolStripMenuItem});
            this.FolderRowContextMenu.Name = "FolderRowContextMenu";
            this.FolderRowContextMenu.Size = new System.Drawing.Size(204, 239);
            this.FolderRowContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.FolderRowContextMenu_Opening);
            // 
            // tsGetThisFolder
            // 
            this.tsGetThisFolder.Name = "tsGetThisFolder";
            this.tsGetThisFolder.Size = new System.Drawing.Size(203, 22);
            this.tsGetThisFolder.Text = "Get This Folder";
            this.tsGetThisFolder.Click += new System.EventHandler(this.tsGetThisFolder_Click);
            // 
            // tsGetPriorFolder
            // 
            this.tsGetPriorFolder.Name = "tsGetPriorFolder";
            this.tsGetPriorFolder.Size = new System.Drawing.Size(203, 22);
            this.tsGetPriorFolder.Text = "Get Prior Folder";
            this.tsGetPriorFolder.Click += new System.EventHandler(this.tsGetPriorFolder_Click);
            // 
            // tsDeleteFolderMenuItem
            // 
            this.tsDeleteFolderMenuItem.Name = "tsDeleteFolderMenuItem";
            this.tsDeleteFolderMenuItem.Size = new System.Drawing.Size(203, 22);
            this.tsDeleteFolderMenuItem.Text = "Delete this Folder";
            this.tsDeleteFolderMenuItem.Click += new System.EventHandler(this.deleteThisFolderToolStripMenuItem_Click);
            // 
            // tsRemoveEmptyFolders
            // 
            this.tsRemoveEmptyFolders.Name = "tsRemoveEmptyFolders";
            this.tsRemoveEmptyFolders.Size = new System.Drawing.Size(203, 22);
            this.tsRemoveEmptyFolders.Text = "Remove Empty Folder(s)";
            this.tsRemoveEmptyFolders.Click += new System.EventHandler(this.tsRemoveEmptyFolders_Click);
            // 
            // tsTxtObjectName
            // 
            this.tsTxtObjectName.Name = "tsTxtObjectName";
            this.tsTxtObjectName.Size = new System.Drawing.Size(100, 23);
            this.tsTxtObjectName.Tag = "Change name here";
            this.tsTxtObjectName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tsTxtObjectName_KeyPress);
            this.tsTxtObjectName.Click += new System.EventHandler(this.tsTxtObjectName_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(200, 6);
            // 
            // tsFileTagInfoMenuItem
            // 
            this.tsFileTagInfoMenuItem.Name = "tsFileTagInfoMenuItem";
            this.tsFileTagInfoMenuItem.Size = new System.Drawing.Size(203, 22);
            this.tsFileTagInfoMenuItem.Text = "Tags...";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(200, 6);
            // 
            // tsGetFileLink
            // 
            this.tsGetFileLink.Name = "tsGetFileLink";
            this.tsGetFileLink.Size = new System.Drawing.Size(203, 22);
            this.tsGetFileLink.Text = "Get Link";
            this.tsGetFileLink.Click += new System.EventHandler(this.tsGetFileLink_Click);
            // 
            // tsGetFileMenuItem
            // 
            this.tsGetFileMenuItem.Name = "tsGetFileMenuItem";
            this.tsGetFileMenuItem.Size = new System.Drawing.Size(203, 22);
            this.tsGetFileMenuItem.Text = "Get File";
            this.tsGetFileMenuItem.Click += new System.EventHandler(this.tsGetFileMenuItem_Click);
            // 
            // getMetadataByPathToolStripMenuItem
            // 
            this.getMetadataByPathToolStripMenuItem.Name = "getMetadataByPathToolStripMenuItem";
            this.getMetadataByPathToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.getMetadataByPathToolStripMenuItem.Text = "Get Metadata by Path";
            this.getMetadataByPathToolStripMenuItem.Click += new System.EventHandler(this.getMetadataByPathToolStripMenuItem_Click);
            // 
            // getMetadataByIDToolStripMenuItem
            // 
            this.getMetadataByIDToolStripMenuItem.Name = "getMetadataByIDToolStripMenuItem";
            this.getMetadataByIDToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.getMetadataByIDToolStripMenuItem.Text = "Get Metadata by ID";
            this.getMetadataByIDToolStripMenuItem.Click += new System.EventHandler(this.getMetadataByIDToolStripMenuItem_Click);
            // 
            // cloudFileBindingSource
            // 
            this.cloudFileBindingSource.AllowNew = false;
            this.cloudFileBindingSource.DataSource = typeof(Cloud_Elements_API.CloudFile);
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolderPath.Location = new System.Drawing.Point(64, 12);
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.Size = new System.Drawing.Size(472, 20);
            this.txtFolderPath.TabIndex = 4;
            this.txtFolderPath.Text = "/";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Folder";
            // 
            // cmdGetFolderContents
            // 
            this.cmdGetFolderContents.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdGetFolderContents.Enabled = false;
            this.cmdGetFolderContents.Image = global::Cloud_Element_Test_Form.Properties.Resources.Folder_Refresh;
            this.cmdGetFolderContents.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdGetFolderContents.Location = new System.Drawing.Point(542, 6);
            this.cmdGetFolderContents.Name = "cmdGetFolderContents";
            this.cmdGetFolderContents.Size = new System.Drawing.Size(69, 35);
            this.cmdGetFolderContents.TabIndex = 5;
            this.cmdGetFolderContents.Text = " Get";
            this.cmdGetFolderContents.UseVisualStyleBackColor = true;
            this.cmdGetFolderContents.Click += new System.EventHandler(this.cmdGetFolderContents_Click);
            // 
            // tpLog
            // 
            this.tpLog.Controls.Add(this.txtLog);
            this.tpLog.Location = new System.Drawing.Point(4, 22);
            this.tpLog.Name = "tpLog";
            this.tpLog.Padding = new System.Windows.Forms.Padding(3);
            this.tpLog.Size = new System.Drawing.Size(944, 338);
            this.tpLog.TabIndex = 1;
            this.tpLog.Text = "Log";
            this.tpLog.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(3, 3);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(938, 332);
            this.txtLog.TabIndex = 0;
            // 
            // tpTest
            // 
            this.tpTest.Controls.Add(this.label5);
            this.tpTest.Controls.Add(this.spnRequestsPerSecond);
            this.tpTest.Controls.Add(this.chkSerializeGetFileInfoReq);
            this.tpTest.Controls.Add(this.cmdForceClean);
            this.tpTest.Controls.Add(this.chkAutoSaveLog);
            this.tpTest.Controls.Add(this.cmdTestClearLog);
            this.tpTest.Controls.Add(this.chkTestCleanup);
            this.tpTest.Controls.Add(this.tbTestOutput);
            this.tpTest.Controls.Add(this.cmdTestButton);
            this.tpTest.Location = new System.Drawing.Point(4, 22);
            this.tpTest.Name = "tpTest";
            this.tpTest.Padding = new System.Windows.Forms.Padding(3);
            this.tpTest.Size = new System.Drawing.Size(944, 338);
            this.tpTest.TabIndex = 3;
            this.tpTest.Text = "Test";
            this.tpTest.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(474, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "requests per second";
            // 
            // spnRequestsPerSecond
            // 
            this.spnRequestsPerSecond.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.spnRequestsPerSecond.Location = new System.Drawing.Point(418, 33);
            this.spnRequestsPerSecond.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.spnRequestsPerSecond.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            -2147483648});
            this.spnRequestsPerSecond.Name = "spnRequestsPerSecond";
            this.spnRequestsPerSecond.Size = new System.Drawing.Size(50, 20);
            this.spnRequestsPerSecond.TabIndex = 9;
            this.spnRequestsPerSecond.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spnRequestsPerSecond.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            // 
            // chkSerializeGetFileInfoReq
            // 
            this.chkSerializeGetFileInfoReq.AutoSize = true;
            this.chkSerializeGetFileInfoReq.Location = new System.Drawing.Point(418, 10);
            this.chkSerializeGetFileInfoReq.Name = "chkSerializeGetFileInfoReq";
            this.chkSerializeGetFileInfoReq.Size = new System.Drawing.Size(195, 17);
            this.chkSerializeGetFileInfoReq.TabIndex = 8;
            this.chkSerializeGetFileInfoReq.Text = "Serialize Get File Metadata requests";
            this.chkSerializeGetFileInfoReq.UseVisualStyleBackColor = true;
            // 
            // cmdForceClean
            // 
            this.cmdForceClean.Location = new System.Drawing.Point(760, 29);
            this.cmdForceClean.Name = "cmdForceClean";
            this.cmdForceClean.Size = new System.Drawing.Size(84, 23);
            this.cmdForceClean.TabIndex = 5;
            this.cmdForceClean.Tag = "cmdForceClean";
            this.cmdForceClean.Text = "Cleanup Test";
            this.cmdForceClean.UseVisualStyleBackColor = true;
            this.cmdForceClean.Click += new System.EventHandler(this.cmdForceClean_Click);
            // 
            // chkAutoSaveLog
            // 
            this.chkAutoSaveLog.AutoSize = true;
            this.chkAutoSaveLog.Checked = true;
            this.chkAutoSaveLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoSaveLog.Location = new System.Drawing.Point(158, 33);
            this.chkAutoSaveLog.Name = "chkAutoSaveLog";
            this.chkAutoSaveLog.Size = new System.Drawing.Size(231, 17);
            this.chkAutoSaveLog.TabIndex = 4;
            this.chkAutoSaveLog.Tag = "chkTestCleanup";
            this.chkAutoSaveLog.Text = "Automatically save log (overwrites previous)";
            this.chkAutoSaveLog.UseVisualStyleBackColor = true;
            // 
            // cmdTestClearLog
            // 
            this.cmdTestClearLog.Location = new System.Drawing.Point(850, 29);
            this.cmdTestClearLog.Name = "cmdTestClearLog";
            this.cmdTestClearLog.Size = new System.Drawing.Size(75, 23);
            this.cmdTestClearLog.TabIndex = 3;
            this.cmdTestClearLog.Text = "Clear Log";
            this.cmdTestClearLog.UseVisualStyleBackColor = true;
            this.cmdTestClearLog.Click += new System.EventHandler(this.cmdTestClearLog_Click);
            // 
            // chkTestCleanup
            // 
            this.chkTestCleanup.AutoSize = true;
            this.chkTestCleanup.Checked = true;
            this.chkTestCleanup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTestCleanup.Location = new System.Drawing.Point(158, 10);
            this.chkTestCleanup.Name = "chkTestCleanup";
            this.chkTestCleanup.Size = new System.Drawing.Size(216, 17);
            this.chkTestCleanup.TabIndex = 2;
            this.chkTestCleanup.Tag = "chkTestCleanup";
            this.chkTestCleanup.Text = "Automatically delete test folders and files";
            this.chkTestCleanup.UseVisualStyleBackColor = true;
            // 
            // tbTestOutput
            // 
            this.tbTestOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTestOutput.Location = new System.Drawing.Point(20, 58);
            this.tbTestOutput.Multiline = true;
            this.tbTestOutput.Name = "tbTestOutput";
            this.tbTestOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbTestOutput.Size = new System.Drawing.Size(905, 238);
            this.tbTestOutput.TabIndex = 1;
            // 
            // cmdTestButton
            // 
            this.cmdTestButton.Location = new System.Drawing.Point(20, 27);
            this.cmdTestButton.Name = "cmdTestButton";
            this.cmdTestButton.Size = new System.Drawing.Size(117, 23);
            this.cmdTestButton.TabIndex = 0;
            this.cmdTestButton.Text = "Run Test";
            this.cmdTestButton.UseVisualStyleBackColor = true;
            this.cmdTestButton.Click += new System.EventHandler(this.cmdTestButton_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Specify folder for work files";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.ReadOnlyChecked = true;
            this.openFileDialog1.RestoreDirectory = true;
            this.openFileDialog1.Title = "Select File to Upload to the Current Cloud Folder";
            // 
            // openSecretsFileDialog
            // 
            this.openSecretsFileDialog.DefaultExt = "json";
            this.openSecretsFileDialog.FileName = "openFileDialog2";
            this.openSecretsFileDialog.Filter = "JSON files|*.json";
            this.openSecretsFileDialog.Title = "Open Stored Connection Secrets";
            // 
            // saveSecretsFileDialog1
            // 
            this.saveSecretsFileDialog1.DefaultExt = "json";
            this.saveSecretsFileDialog1.Filter = "JSON Files|*.json";
            this.saveSecretsFileDialog1.Title = "Save Current Connection Secrets";
            // 
            // folderBrowserUploadTree
            // 
            this.folderBrowserUploadTree.Description = "Select folder to begin tree upload";
            this.folderBrowserUploadTree.ShowNewFolderButton = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Optional";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 427);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.Text = "Cloud Elements Documents Connector Test  Form";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tpAuthorize.ResumeLayout(false);
            this.tpAuthorize.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tpContents.ResumeLayout(false);
            this.tpContents.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgFolderContents)).EndInit();
            this.FolderRowContextMenu.ResumeLayout(false);
            this.FolderRowContextMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cloudFileBindingSource)).EndInit();
            this.tpLog.ResumeLayout(false);
            this.tpLog.PerformLayout();
            this.tpTest.ResumeLayout(false);
            this.tpTest.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnRequestsPerSecond)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsBtnPing;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpContents;
        private System.Windows.Forms.DataGridView dgFolderContents;
        private System.Windows.Forms.Button cmdGetFolderContents;
        private System.Windows.Forms.TextBox txtFolderPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tpLog;
        private System.Windows.Forms.BindingSource cloudFileBindingSource;
        private System.Windows.Forms.CheckBox chkWithTags;
        private System.Windows.Forms.ContextMenuStrip FolderRowContextMenu;
        private System.Windows.Forms.ToolStripMenuItem tsGetThisFolder;
        private System.Windows.Forms.ToolStripMenuItem tsGetPriorFolder;
        private System.Windows.Forms.TabPage tpAuthorize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cmdApply;
        private System.Windows.Forms.TextBox txtUserKey;
        private System.Windows.Forms.TextBox txtElementKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsGetFileLink;
        private System.Windows.Forms.Button cmdWorkFolder;
        private System.Windows.Forms.TextBox txtWorkFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem tsGetFileMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripMenuItem tsFileTagInfoMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsBtnNewFolder;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripTextBox tsTxtFolderName;
        private System.Windows.Forms.ToolStripMenuItem tsDeleteFolderMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifiedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn directoryDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn HasTags;
        private System.Windows.Forms.DataGridViewTextBoxColumn pathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btnGetPriorFolder;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripTextBox tstxtTagData;
        private System.Windows.Forms.ToolStripTextBox tsTxtObjectName;
        private System.Windows.Forms.ToolStripDropDownButton toolStripConnectionSecrets;
        private System.Windows.Forms.ToolStripMenuItem saveCurrentSecretsAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSecretsFromToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openSecretsFileDialog;
        private System.Windows.Forms.SaveFileDialog saveSecretsFileDialog1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripTextBox toolStripTxtConnectionNow;
        private System.Windows.Forms.TabPage tpTest;
        private System.Windows.Forms.TextBox tbTestOutput;
        private System.Windows.Forms.Button cmdTestButton;
        private System.Windows.Forms.CheckBox chkTestCleanup;
        private System.Windows.Forms.Button cmdTestClearLog;
        private System.Windows.Forms.CheckBox chkAutoSaveLog;
        private System.Windows.Forms.Button cmdForceClean;
        private System.Windows.Forms.ToolStripMenuItem getMetadataByPathToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkSerializeGetFileInfoReq;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown spnRequestsPerSecond;
        private System.Windows.Forms.Button cmdGetID;
        private System.Windows.Forms.ToolStripMenuItem tsRemoveEmptyFolders;
        private System.Windows.Forms.ToolStripDropDownButton tsBtnUpload;
        private System.Windows.Forms.ToolStripMenuItem uploadOneFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadSubtreeToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserUploadTree;
        private System.Windows.Forms.Button cmdGetFN;
        private System.Windows.Forms.ToolStripMenuItem getMetadataByIDToolStripMenuItem;
        private System.Windows.Forms.TextBox txtExtraThing;
        private System.Windows.Forms.Label label6;
    }
}

