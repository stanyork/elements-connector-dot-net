using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cloud_Element_Test_Form
{
    public partial class frmEmptyFolderScanOptions : Form
    {

        public EmptyFolderOptions ScanOptions = new EmptyFolderOptions();

        public frmEmptyFolderScanOptions()
        {
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            ScanOptions.PathCheck = chkMustContain.Checked;
            ScanOptions.SingleFileOK = chkSingleFile.Checked;

            ScanOptions.PathMustContain = txtPathRequires.Text.Trim();
            ScanOptions.SingleFileSizeUnder = (ulong)spnMaxBytes.Value;
            ScanOptions.SingleFileType = txtIgnoreExtenion.Text.Trim();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
