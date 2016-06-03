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
    public partial class frmGetCloudFileID : Form
    {

        public string FileID = "";

        public void SetFNMode()
        {
            this.Text = "File Path of Cloud File to Get";
            this.txtID.Text = "Path";
        }



        public frmGetCloudFileID()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileID = txtID.Text.Trim();
            Close();
        }

        private void frmGetCloudFileID_Load(object sender, EventArgs e)
        {
            txtID.Focus();
        }
    }
}
