using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cloud_Element_Test_Form
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) ex = ex.InnerException;
                Clipboard.SetText(ex.ToString(), TextDataFormat.Text);
                if (System.Windows.Forms.MessageBox.Show(string.Format("Unhandled exception has been placed on clipboard: {0} \n\n{1}\n\nEnd program?", ex.Message, ex.ToString()), "Exception", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    Clipboard.SetText(ex.ToString(), TextDataFormat.Text);
                    Application.Exit();
                }
                else Application.Restart();
            }
        }
    }
}
