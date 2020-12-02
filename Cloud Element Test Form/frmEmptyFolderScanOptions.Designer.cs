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
            this.label5 = new System.Windows.Forms.Label();
            this.chkTagged = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.spnHoursOld = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.spnMaxBytes = new System.Windows.Forms.NumericUpDown();
            this.txtIgnoreExtenion = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtPathRequires = new System.Windows.Forms.TextBox();
            this.chkMustContain = new System.Windows.Forms.CheckBox();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.txtFolderList = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnHoursOld)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnMaxBytes)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkSingleFile
            // 
            this.chkSingleFile.AutoSize = true;
            this.chkSingleFile.Checked = true;
            this.chkSingleFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSingleFile.Location = new System.Drawing.Point(9, 43);
            this.chkSingleFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSingleFile.Name = "chkSingleFile";
            this.chkSingleFile.Size = new System.Drawing.Size(191, 24);
            this.chkSingleFile.TabIndex = 0;
            this.chkSingleFile.Text = "Folders with only one  ";
            this.chkSingleFile.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtFolderList);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.chkTagged);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.spnHoursOld);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.spnMaxBytes);
            this.groupBox1.Controls.Add(this.txtIgnoreExtenion);
            this.groupBox1.Controls.Add(this.chkSingleFile);
            this.groupBox1.Location = new System.Drawing.Point(18, 18);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(639, 303);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Include";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(292, 123);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "and ";
            // 
            // chkTagged
            // 
            this.chkTagged.AutoSize = true;
            this.chkTagged.Location = new System.Drawing.Point(387, 122);
            this.chkTagged.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkTagged.Name = "chkTagged";
            this.chkTagged.Size = new System.Drawing.Size(162, 24);
            this.chkTagged.TabIndex = 9;
            this.chkTagged.Text = "Has been Tagged";
            this.chkTagged.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(292, 85);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "and >= ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(502, 85);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "hours old";
            // 
            // spnHoursOld
            // 
            this.spnHoursOld.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.spnHoursOld.Location = new System.Drawing.Point(387, 82);
            this.spnHoursOld.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.spnHoursOld.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.spnHoursOld.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spnHoursOld.Name = "spnHoursOld";
            this.spnHoursOld.Size = new System.Drawing.Size(106, 26);
            this.spnHoursOld.TabIndex = 6;
            this.spnHoursOld.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spnHoursOld.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(292, 45);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "file with <=";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(502, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "bytes";
            // 
            // spnMaxBytes
            // 
            this.spnMaxBytes.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.spnMaxBytes.Location = new System.Drawing.Point(387, 42);
            this.spnMaxBytes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.spnMaxBytes.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.spnMaxBytes.Name = "spnMaxBytes";
            this.spnMaxBytes.Size = new System.Drawing.Size(106, 26);
            this.spnMaxBytes.TabIndex = 3;
            this.spnMaxBytes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spnMaxBytes.Value = new decimal(new int[] {
            768,
            0,
            0,
            0});
            // 
            // txtIgnoreExtenion
            // 
            this.txtIgnoreExtenion.Location = new System.Drawing.Point(201, 38);
            this.txtIgnoreExtenion.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtIgnoreExtenion.Name = "txtIgnoreExtenion";
            this.txtIgnoreExtenion.Size = new System.Drawing.Size(80, 26);
            this.txtIgnoreExtenion.TabIndex = 2;
            this.txtIgnoreExtenion.Text = ".htm";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtPathRequires);
            this.groupBox2.Controls.Add(this.chkMustContain);
            this.groupBox2.Location = new System.Drawing.Point(18, 351);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(639, 125);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Exclude";
            // 
            // txtPathRequires
            // 
            this.txtPathRequires.Location = new System.Drawing.Point(387, 48);
            this.txtPathRequires.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPathRequires.Name = "txtPathRequires";
            this.txtPathRequires.Size = new System.Drawing.Size(148, 26);
            this.txtPathRequires.TabIndex = 1;
            this.txtPathRequires.Text = "/RFI/";
            // 
            // chkMustContain
            // 
            this.chkMustContain.AutoSize = true;
            this.chkMustContain.Checked = true;
            this.chkMustContain.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMustContain.Location = new System.Drawing.Point(9, 48);
            this.chkMustContain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkMustContain.Name = "chkMustContain";
            this.chkMustContain.Size = new System.Drawing.Size(296, 24);
            this.chkMustContain.TabIndex = 0;
            this.chkMustContain.Text = "Path that does NOT contain the text: ";
            this.chkMustContain.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(687, 48);
            this.cmdOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(112, 35);
            this.cmdOK.TabIndex = 3;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(687, 114);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(112, 35);
            this.cmdCancel.TabIndex = 4;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // txtFolderList
            // 
            this.txtFolderList.Location = new System.Drawing.Point(36, 187);
            this.txtFolderList.Multiline = true;
            this.txtFolderList.Name = "txtFolderList";
            this.txtFolderList.Size = new System.Drawing.Size(584, 95);
            this.txtFolderList.TabIndex = 11;
            this.txtFolderList.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 154);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(343, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "Folder List (optional list of folders to check, one per line)";
            // 
            // frmEmptyFolderScanOptions
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(862, 611);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmEmptyFolderScanOptions";
            this.Text = "Empty Folder Scan Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnHoursOld)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnMaxBytes)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown spnHoursOld;
        private System.Windows.Forms.CheckBox chkTagged;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFolderList;
    }
}