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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Cloud_Elements_API.CloudAuthorization APIAuthorization;
        private Cloud_Elements_API.CloudElementsConnector APIConnector;

        private Stack<String> FolderPathHistory = new Stack<String>(9);
        private String WorkPath { get { return txtWorkFolder.Text; } }
        private String CurrentFolderPath
        {
            get
            {
                string result = "/";
                if (FolderPathHistory.Count > 0) result = FolderPathHistory.Peek();
                return result;
            }
        }
        private readonly string CONST_GetFolderFirst = "You must have a folder first - specify Folder Path and click GET ";



        public delegate void StatusMsgDelegate(string msg);
        private void StatusMsg(string info)
        {
            if (InvokeRequired)
            {
                this.Invoke(new StatusMsgDelegate(StatusMsg), new object[] { info });
                return;
            }
            if (txtLog.Text.Length > 64000)
            {
                txtLog.Text = txtLog.Text.Substring(32000);
            }
            txtLog.Text += string.Format("{0}: {1}\r\n", Cloud_Elements_API.Tools.TraceTimeNow(), info);
            toolStripStatusLabel1.Text = info;
            statusStrip1.Update();
        }

        private string DefaultSecretsFN;
        private async void Form1_Load(object sender, EventArgs e)
        {
            txtWorkFolder.Text = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Cloud-Elements.NET Connector Demo\\";
            if (!System.IO.Directory.Exists(txtWorkFolder.Text)) System.IO.Directory.CreateDirectory(txtWorkFolder.Text);
            DefaultSecretsFN = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Cloud-Elements.NET Connector Demo\\Default Secrets.json";
            if (System.IO.File.Exists(DefaultSecretsFN))
            {
                APIAuthorization = new Cloud_Elements_API.CloudAuthorization(Cloud_Elements_API.Tools.FileToString(DefaultSecretsFN));
                StatusMsg("Loaded secrets from " + DefaultSecretsFN);
                this.Show();
                await PingService();
                Task ignoredRefreshFolderTask = RefreshCurrentFolder(); // very naughty, ignore exceptions; ref http://stackoverflow.com/questions/14903887/warning-this-call-is-not-awaited-execution-of-the-current-method-continues
            }
            else
            {
                if (System.Windows.Forms.MessageBox.Show(string.Format("Stored Secrets not found - {0}\n\nYou will have to enter element and user secrets on the form.\n\nContinue?", DefaultSecretsFN), "Notification", System.Windows.Forms.MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                {
                    Application.Exit();
                    Close();
                    return;
                }
                StatusMsg("Secrets needed: specify and APPLY authorization tokens");
            }


        }

        private void UIState(Boolean APIIsConnected)
        {
            tsBtnPing.Enabled = APIIsConnected;
            tsBtnNewFolder.Enabled = APIIsConnected && (tsTxtFolderName.Text.Length > 0);
            tsBtnUpload.Enabled = APIIsConnected;
            cmdGetFolderContents.Enabled = APIIsConnected;

            if (APIIsConnected) tabControl1.SelectedTab = tpContents;
            else tabControl1.SelectedTab = tpAuthorize;

        }

        private void UpdateSecretsFile(string result)
        {
            Cloud_Elements_API.Tools.StringToFile(APIAuthorization.ToJSonString(result), DefaultSecretsFN);
        }

        private async Task PingService()
        {
            cmdApply.Enabled = false;
            tsBtnPing.Enabled = false;
            StatusMsg("Pinging ( connection and authorization test )....");
            APIConnector = new Cloud_Elements_API.CloudElementsConnector();
            APIConnector.APIAuthorization = APIAuthorization;
            Boolean result = false;
            try
            {
                Cloud_Elements_API.Pong PongResult = await APIConnector.Ping();
                StatusMsg(PongResult.ToString());
                UpdateSecretsFile(PongResult.ToString());
                result = true;
            }
            catch (Exception ex)
            {
                StatusMsg("Failed: " + ex.Message);
            }
            finally
            {
                cmdApply.Enabled = true;
                UIState(result);
            }

        }
        private void tsBtnPing_Click(object sender, EventArgs e)
        {
            Task ignoredTask = PingService();

        }

        private async void AuthorizationTokenChanged(object sender, EventArgs e)
        {
            string UserKey;
            string ElementKey;
            UserKey = txtUserKey.Text;
            ElementKey = txtElementKey.Text;

            if ((UserKey.Trim().Length > 0) && (ElementKey.Trim().Length > 0))
            {
                APIAuthorization = new Cloud_Elements_API.CloudAuthorization(ElementKey, UserKey);
                UIState(false);
                await PingService();
                tsBtnPing.Enabled = true;

            }
            else StatusMsg("Both secrets are required");
        }

        private void cmdApply_Click(object sender, EventArgs e)
        {
            //Button ApplyBtn = (Button)sender;
            AuthorizationTokenChanged(sender, e);

            Task ignoredRefreshFolderTask = RefreshCurrentFolder(); // very naughty, ignore exceptions; ref http://stackoverflow.com/questions/14903887/warning-this-call-is-not-awaited-execution-of-the-current-method-continues
        }

        private async Task RefreshCurrentFolder()
        {
            cmdGetFolderContents.Enabled = false;
            try
            {
                StatusMsg(string.Format("Requesting [{0}]...", txtFolderPath.Text));
                List<Cloud_Elements_API.CloudFile> Result = await APIConnector.ListFolderContents(txtFolderPath.Text, chkWithTags.Checked);
                cloudFileBindingSource.DataSource = Result;
                if (FolderPathHistory.Count == 0 || (FolderPathHistory.Peek() != txtFolderPath.Text)) FolderPathHistory.Push(txtFolderPath.Text);
                StatusMsg(string.Format("Found {0} - Right Click grid rows for options", Result.Count()));
                btnGetPriorFolder.Enabled = (FolderPathHistory.Count > 1);
            }
            catch (Exception ex)
            {
                StatusMsg("Refresh folder failed: " + ex.Message);
            }

            finally
            {
                cmdGetFolderContents.Enabled = true;
            }

        }
        private async void cmdGetFolderContents_Click(object sender, EventArgs e)
        {
            await RefreshCurrentFolder();
        }


        private Cloud_Elements_API.CloudFile GetCurrentFolderContentRow()
        {
            //BindingManagerBase grdBinding = this.BindingContext(dgFolderContents.DataSource, dgFolderContents.DataMember);

            Cloud_Elements_API.CloudFile currentRow = null;
            if (dgFolderContents.CurrentRow != null)
            {
                if (dgFolderContents.CurrentRow.DataBoundItem is Cloud_Elements_API.CloudFile)
                    currentRow = (Cloud_Elements_API.CloudFile)dgFolderContents.CurrentRow.DataBoundItem;
            }
            return currentRow;
        }

        private void dgFolderContents_DoubleClick(object sender, EventArgs e)
        {
            Cloud_Elements_API.CloudFile currentRow = null;
            if (!HasCurrentCloudFile(ref currentRow)) return;
            if (currentRow.directory)
            {
                tsGetThisFolder_Click(sender, e);
            }
            else
            {
                if (System.Windows.Forms.MessageBox.Show(string.Format("Downdload {0}", currentRow.path), "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    tsGetFileMenuItem_Click(sender, e);
                }
            }
        }



        private void FolderRowContextMenu_Opening(object sender, CancelEventArgs e)
        {
            Cloud_Elements_API.CloudFile currentRow = null;
            if (!HasCurrentCloudFile(ref currentRow)) return;

            tsGetThisFolder.Enabled = currentRow.directory;
            tsGetPriorFolder.Enabled = (FolderPathHistory.Count > 1);

            tsDeleteFolderMenuItem.Enabled = currentRow.directory;
            tsGetFileMenuItem.Enabled = !currentRow.directory;

            tsFileTagInfoMenuItem.Text = "Tags (not requested)";
            tsFileTagInfoMenuItem.Enabled = chkWithTags.Checked;
            if (tsFileTagInfoMenuItem.Enabled)
            {
                tsFileTagInfoMenuItem.DropDownItems.Clear();
                if (currentRow.tags != null)
                {
                    tsFileTagInfoMenuItem.Text = string.Format("{0} Tags ", currentRow.tags.Length);
                    foreach (string tag in currentRow.tags)
                    {
                        tsFileTagInfoMenuItem.DropDownItems.Add(tag);
                    }
                }
                else
                {
                    tsFileTagInfoMenuItem.Text = string.Format("No Tags ", 0);
                    tsFileTagInfoMenuItem.Enabled = false;
                }
            }


        }

        private async void tsGetThisFolder_Click(object sender, EventArgs e)
        {
            Cloud_Elements_API.CloudFile currentRow = null;
            if (!HasCurrentCloudFile(ref currentRow)) return;

            txtFolderPath.Text = currentRow.path;
            await RefreshCurrentFolder();

        }

        private async void GetPriorFolder_Click(object sender, EventArgs e)
        {
            if (FolderPathHistory.Count == 1)
            {
                StatusMsg("No prior path");
                return;
            }
            FolderPathHistory.Pop();
            txtFolderPath.Text = FolderPathHistory.Peek();
            await RefreshCurrentFolder();
        }

        private void tsGetPriorFolder_Click(object sender, EventArgs e)
        {
            Cloud_Elements_API.CloudFile currentRow = null;
            if (!HasCurrentCloudFile(ref currentRow)) return;
            GetPriorFolder_Click(sender, e);
        }

        private void cmdWorkFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.Cancel) return;
            txtWorkFolder.Text = folderBrowserDialog1.SelectedPath;
        }

        /// <summary>
        /// Asynchronously download a files, with progress messages to status bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>You can have multiple simultaneous downloads</remarks>
        private async void tsGetFileMenuItem_Click(object sender, EventArgs e)
        {
            cmdGetFolderContents.Enabled = false;
            try
            {
                Cloud_Elements_API.CloudFile currentRow = null;
                if (!HasCurrentCloudFile(ref currentRow)) return;

                StatusMsg("Requesting...");
                Cloud_Elements_API.FileContent Result = await APIConnector.GetFile(currentRow.id);

                string fn = System.IO.Path.Combine(WorkPath, Result.Disposition);
                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(fn)))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fn));
                }
                if (System.IO.File.Exists(fn))
                {
                    // HINT: would be better to check this before downloading...but we are using the response disposition to illustrate its existence....
                    if (System.Windows.Forms.MessageBox.Show(string.Format("Downdload will replace existing file {0}\n\nOkay?", fn), "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    {
                        Result.ContentStream.Close();
                        return;
                    }
                    System.IO.File.Delete(fn);
                }

                System.IO.Stream Target = new System.IO.FileStream(fn, System.IO.FileMode.Create, System.Security.AccessControl.FileSystemRights.FullControl, System.IO.FileShare.None, 16384, System.IO.FileOptions.Asynchronous, null);

                StatusMsg("Receiving...");
                //error handling here would be nice...
                Cloud_Elements_API.Tools.Progress += Tools_Progress;
                await Cloud_Elements_API.Tools.StreamCopyWithProgress(Result.ContentStream, Target, Result.ContentLength);
                Result.ContentStream.Close();
                Target.Close();
                Cloud_Elements_API.Tools.Progress -= Tools_Progress;
                StatusMsg(string.Format("Stored {1}: {0}", Cloud_Elements_API.Tools.SizeInBytesToString(Result.ContentLength), Result.Disposition));

            }
            finally
            {
                cmdGetFolderContents.Enabled = true;
            }
        }

        void Tools_Progress(object sender, int Pct)
        {
            StatusMsg(string.Format("Progress...{0}%", Pct));
        }

        private Boolean HasGottenFolder()
        {
            var result = true;
            if (FolderPathHistory.Count == 0)
            {
                StatusMsg(CONST_GetFolderFirst);
                result = false;
            }
            return result;
        }

        private async void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!HasGottenFolder()) return;
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            var MIMEType = Cloud_Elements_API.Tools.FileTypeToMimeContentType(System.IO.Path.GetExtension(openFileDialog1.FileName));
            string TargetPath = this.CurrentFolderPath;
            string SourceFileName = System.IO.Path.GetFileName(openFileDialog1.FileName);
            if (!TargetPath.EndsWith("/")) TargetPath += "/";
            TargetPath += SourceFileName;
            var sizeInBytes = new System.IO.FileInfo(openFileDialog1.FileName).Length;
            List<String> TagList = new List<String>();
            TagList.Add("sfCE.NET");

            Cloud_Elements_API.CloudFile Result = await APIConnector.PostFile(openFileDialog1.OpenFile(), MIMEType,
                                                        TargetPath, "Uploaded by .NET Connector Test Tool!",
                                                        TagList.ToArray(), false, sizeInBytes);


        }

        private async void tsBtnNewFolder_Click(object sender, EventArgs e)
        {
            if (!HasGottenFolder()) return;
            if (tsTxtFolderName.Text.Length > 0)
            {
                Cloud_Elements_API.CloudFile newFolder = new Cloud_Elements_API.CloudFile();
                newFolder.path = CurrentFolderPath;
                if (!tsTxtFolderName.Text.StartsWith("/")) newFolder.path += "/";
                newFolder.path += tsTxtFolderName.Text;
                newFolder.tags = new string[] { "sfCE.NET" };
                Cloud_Elements_API.CloudFile Result = await APIConnector.CreateFolder(newFolder);
                StatusMsg(string.Format("Created {0}, id={1}", Result.path, Result.id));
                Task ignoredRefreshFolderTask = RefreshCurrentFolder(); // very naughty, ignore exceptions; ref http://stackoverflow.com/questions/14903887/warning-this-call-is-not-awaited-execution-of-the-current-method-continues
            }

        }

        private void tsTxtFolderName_Leave(object sender, EventArgs e)
        {
            tsBtnNewFolder.Enabled = (tsTxtFolderName.Text.Length > 0);
        }

        private async void deleteThisFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!HasGottenFolder()) return;
            try
            {
                Cloud_Elements_API.CloudFile  currentRow = null;
             
                if (!HasCurrentCloudFile(ref currentRow)) return;

                if (currentRow.size > 0)
                {
                    if (System.Windows.Forms.MessageBox.Show(string.Format("Folder is not empty!  Delete [{0}] anyway?", currentRow.path), "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }

                StatusMsg(string.Format("Deleting {0}, size={1}", currentRow.path, Cloud_Elements_API.Tools.SizeInBytesToString(currentRow.size)));
                Cloud_Elements_API.CloudFile Result = await APIConnector.DeleteFolder(currentRow.path, false);

                StatusMsg("Done.");
                await this.RefreshCurrentFolder();

            }
            finally
            {
                cmdGetFolderContents.Enabled = true;
            }
        }

        private bool HasCurrentCloudFile(ref Cloud_Elements_API.CloudFile currentRow)
        {
            currentRow = GetCurrentFolderContentRow();
            bool result = !(currentRow == null);
            if (!result) StatusMsg("No current row");
            return result;
        }


        private async void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            if (!HasGottenFolder()) return;
            Cloud_Elements_API.CloudFile currentRow = null;
            if (!HasCurrentCloudFile(ref currentRow)) return;

            string TagToSet = tstxtTagData.Text;
            //Type the tag to add.  If includes an = is treated as a KV pair; Try: *TestTag for a random tag or sfKey=* for a guid tag
            if (TagToSet == "*TestTag") TagToSet = "Day-" + DateTime.Today.DayOfWeek.ToString();
            if (TagToSet.StartsWith("sfKey=")) TagToSet = string.Format("sfKey={0}", Guid.NewGuid());
            StatusMsg("Storing Tag: " + TagToSet);
            await Cloud_Elements_API.TagOperations.SetTag(APIConnector, Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.ID, currentRow.id, TagToSet);
            if (!chkWithTags.Checked) chkWithTags.Checked = true;
            await RefreshCurrentFolder();
        }









    }
}
