namespace Cloud_Element_Test_Form
{
    partial class frmEmptyFolderScanOptions
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
            this.chkSingleFile = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkMustContain = new System.Windows.Forms.CheckBox();
            this.txtPathRequires = new System.Windows.Forms.TextBox();
            this.txtIgnoreExtenion = new System.Windows.Forms.TextBox();
            this.spnMaxBytes = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnMaxBytes)).BeginInit();
            this.SuspendLayout();
            // 
            // chkSingleFile
            // 
            this.chkSingleFile.AutoSize = true;
            this.chkSingleFile.Checked = true;
            this.chkSingleFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSingleFile.Location = new System.Drawing.Point(6, 28);
            this.chkSingleFile.Name = "chkSingleFile";
            this.chkSingleFile.Size = new System.Drawing.Size(131, 17);
            this.chkSingleFile.TabIndex = 0;
            this.chkSingleFile.Text = "Folders with only one  ";
            this.chkSingleFile.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.spnMaxBytes);
            this.groupBox1.Controls.Add(this.txtIgnoreExtenion);
            this.groupBox1.Controls.Add(this.chkSingleFile);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(426, 85);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Include";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtPathRequires);
            this.groupBox2.Controls.Add(this.chkMustContain);
            this.groupBox2.Location = new System.Drawing.Point(12, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(426, 81);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Exclude";
            // 
            // chkMustContain
            // 
            this.chkMustContain.AutoSize = true;
            this.chkMustContain.Checked = true;
            this.chkMustContain.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMustContain.Location = new System.Drawing.Point(6, 31);
            this.chkMustContain.Name = "chkMustContain";
            this.chkMustContain.Size = new System.Drawing.Size(195, 17);
            this.chkMustContain.TabIndex = 0;
            this.chkMustContain.Text = "Path that does not contain the text: ";
            this.chkMustContain.UseVisualStyleBackColor = true;
            // 
            // txtPathRequires
            // 
            this.txtPathRequires.Location = new System.Drawing.Point(208, 31);
            this.txtPathRequires.Name = "txtPathRequires";
            this.txtPathRequires.Size = new System.Drawing.Size(100, 20);
            this.txtPathRequires.TabIndex = 1;
            this.txtPathRequires.Text = "/RFI/";
            // 
            // txtIgnoreExtenion
            // 
            this.txtIgnoreExtenion.Location = new System.Drawing.Point(134, 25);
            this.txtIgnoreExtenion.Name = "txtIgnoreExtenion";
            this.txtIgnoreExtenion.Size = new System.Drawing.Size(55, 20);
            this.txtIgnoreExtenion.TabIndex = 2;
            this.txtIgnoreExtenion.Text = ".htm";
            // 
            // spnMaxBytes
            // 
            this.spnMaxBytes.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.spnMaxBytes.Location = new System.Drawing.Point(258, 27);
            this.spnMaxBytes.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.spnMaxBytes.Name = "spnMaxBytes";
            this.spnMaxBytes.Size = new System.Drawing.Size(71, 20);
            this.spnMaxBytes.TabIndex = 3;
            this.spnMaxBytes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spnMaxBytes.Value = new decimal(new int[] {
            768,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(335, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "bytes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "file with <=";
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(458, 31);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 3;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(458, 74);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 4;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // frmEmptyFolderScanOptions
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(545, 216);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmEmptyFolderScanOptions";
            this.Text = "Empty Folder Scan Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnMaxBytes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkSingleFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtIgnoreExtenion;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtPathRequires;
        private System.Windows.Forms.CheckBox chkMustContain;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown spnMaxBytes;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
    }
}