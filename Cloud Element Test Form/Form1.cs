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

        public delegate void TestMsgDelegate(string msg);
        private void TestStatusMsg(string info)
        {
            if (InvokeRequired)
            {
                this.Invoke(new TestMsgDelegate(TestStatusMsg), new object[] { info });
                return;
            }
            lock (tbTestOutput)
            {
                if (tbTestOutput.Text.Length > 64000)
                {
                    tbTestOutput.Text = tbTestOutput.Text.Substring(32000);
                }
                tbTestOutput.Text += string.Format("{0}: {1}\r\n", Cloud_Elements_API.Tools.TraceTimeNow(), info);
            }
        }

        private void HandleDiagEvent(object sender, string info)
        {
            StatusMsg(info);
            if (!cmdTestButton.Enabled) TestStatusMsg(info);
        }


        private string DefaultSecretsFN;
        private   void Form1_Load(object sender, EventArgs e)
        {
            txtWorkFolder.Text = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Cloud-Elements.NET Connector Demo\\";
            if (!System.IO.Directory.Exists(txtWorkFolder.Text)) System.IO.Directory.CreateDirectory(txtWorkFolder.Text);
            DefaultSecretsFN = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Cloud-Elements.NET Connector Demo\\Default Secrets.json";
            if (System.IO.File.Exists(DefaultSecretsFN))
            {
                 LoadSecretsFromFile(DefaultSecretsFN);
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

        private async void LoadSecretsFromFile(string secretsFN)
        {
            APIAuthorization = new Cloud_Elements_API.CloudAuthorization(Cloud_Elements_API.Tools.FileToString(secretsFN));
            StatusMsg("Loaded secrets from " + secretsFN);
            this.Show();
            await PingService();
            Task ignoredRefreshFolderTask = RefreshCurrentFolder(); // very naughty, ignore exceptions; ref http://stackoverflow.com/questions/14903887/warning-this-call-is-not-awaited-execution-of-the-current-method-continues
        }

        private void UIState(Boolean APIIsConnected)
        {
            tsBtnPing.Enabled = APIIsConnected;
            tsBtnNewFolder.Enabled = APIIsConnected && (tsTxtFolderName.Text.Length > 0);
            tsBtnUpload.Enabled = APIIsConnected;
            saveCurrentSecretsAsToolStripMenuItem.Enabled = APIIsConnected;
            cmdGetFolderContents.Enabled = APIIsConnected;
            cmdGetID.Enabled = APIIsConnected;

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
            APIConnector.DiagTrace += new Cloud_Elements_API.CloudElementsConnector.DiagTraceEventHanlder(HandleDiagEvent);
            Boolean result = false;
            try
            {
                Cloud_Elements_API.Pong PongResult = await APIConnector.Ping();
                // here is one of the few example where we really go async within a single method...
                Task<Cloud_Elements_API.CloudStorage> StorageTask = APIConnector.GetStorageAvailable();
                StatusMsg(PongResult.ToString());
                UpdateSecretsFile(PongResult.ToString());
                toolStripTxtConnectionNow.Text = PongResult.endpoint;
                Cloud_Elements_API.CloudStorage StorageResult = await StorageTask;
                StatusMsg(string.Format("Storage - Total {0}; Shared {1}; Used {2}", Cloud_Elements_API.Tools.SizeInBytesToString(  StorageResult.total),
                    Cloud_Elements_API.Tools.SizeInBytesToString(StorageResult.shared),
                    Cloud_Elements_API.Tools.SizeInBytesToString(StorageResult.used)));

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
                List<Cloud_Elements_API.CloudFile> Result = await APIConnector.ListFolderContents(Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.Path, txtFolderPath.Text, chkWithTags.Checked);
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
                if (System.Windows.Forms.MessageBox.Show(string.Format("Download {0}", currentRow.path), "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
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

            tsDeleteFolderMenuItem.Enabled = true;
            tsGetFileMenuItem.Enabled = !currentRow.directory;
            if (currentRow.directory)
            {
                tsDeleteFolderMenuItem.Text = "Delete this folder...";
            }
            else tsDeleteFolderMenuItem.Text = "Delete this file...";

            tsTxtObjectName.Text = currentRow.name;
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

                StatusMsg(string.Format(">> Get File Result: content-length: {0}; disposition: [{1}]", Result.ContentLength, Result.Disposition));
                string fn = System.IO.Path.Combine(WorkPath, Result.Disposition);
                if (Result.Disposition.Length == 0)
                {
                    fn += currentRow.name;
                }
                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(fn)))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fn));
                }
                if (System.IO.File.Exists(fn))
                {
                    // HINT: would be better to check this before downloading...but we are using the response disposition to illustrate its existence....
                    if (System.Windows.Forms.MessageBox.Show(string.Format("Download will replace existing file {0}\n\nOkay?", fn), "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
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
                Target.Seek(0, System.IO.SeekOrigin.Begin);

                StatusMsg(string.Format("Downloaded content for {1}: MD5={0}", Cloud_Elements_API.Tools.HashForBuffer(Target, "MD5"), System.IO.Path.GetFileName(fn)));
                Target.Seek(0, System.IO.SeekOrigin.Begin);
                StatusMsg(string.Format("Downloaded content for {1}: SHA1={0}", Cloud_Elements_API.Tools.HashForBuffer(Target, "SHA1"), System.IO.Path.GetFileName(fn)));
                Target.Close();
                Cloud_Elements_API.Tools.Progress -= Tools_Progress;
                StatusMsg(string.Format("Stored {1}: {0}", Cloud_Elements_API.Tools.SizeInBytesToString(Result.ContentLength), System.IO.Path.GetFileName(fn)));

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
                Cloud_Elements_API.CloudFile currentRow = null;

                if (!HasCurrentCloudFile(ref currentRow)) return;

                if (currentRow.directory)
                {
                    if (currentRow.size > 0)
                    {
                        if (System.Windows.Forms.MessageBox.Show(string.Format("Folder is not empty!  Delete [{0}] anyway?", currentRow.path), "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                        {
                            return;
                        }
                    }

                    StatusMsg(string.Format("Deleting {0}, size={1}", currentRow.path, Cloud_Elements_API.Tools.SizeInBytesToString(currentRow.size)));
                   bool Result = await APIConnector.DeleteFolder(Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.Path, currentRow.path, false);
                }
                else
                {
                    if (currentRow.size > 0)
                    {
                        if (System.Windows.Forms.MessageBox.Show(string.Format("File is not empty!  Delete [{0}] anyway?", currentRow.path), "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                        {
                            return;
                        }
                    }

                    StatusMsg(string.Format("Deleting {0}, size={1}", currentRow.path, Cloud_Elements_API.Tools.SizeInBytesToString(currentRow.size)));
                    bool Result = await APIConnector.DeleteFile(Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.ID, currentRow.id, false);
                }

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

            if (!chkWithTags.Checked || !currentRow.HasTags)
            {
                StatusMsg("Getting current Tag(s).... ");
                currentRow = await APIConnector.GetDocEntryMetaData(currentRow.EntryType, Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.ID, currentRow.id,false);
            }

            StatusMsg("Storing Tag: " + TagToSet);

            await Cloud_Elements_API.TagOperations.SetTag(APIConnector, currentRow, TagToSet);
            if (!chkWithTags.Checked) chkWithTags.Checked = true;
            await RefreshCurrentFolder();
        }

        private async void tsGetFileLink_Click(object sender, EventArgs e)
        {
            if (!HasGottenFolder()) return;
            Cloud_Elements_API.CloudFile currentRow = null;
            if (!HasCurrentCloudFile(ref currentRow)) return;
            if (currentRow.directory)
            {
                StatusMsg("API does not support a link to a folder");
            }

            Cloud_Elements_API.CloudLink Result = await APIConnector.FileLinks(Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.ID, currentRow.id);
            StatusMsg(string.Format("ceLink: {0}", Result.cloudElementsLink));
            StatusMsg(string.Format("Provider View Link: {0}", Result.providerViewLink));
            StatusMsg(string.Format("Provider Link: {0}", Result.providerLink));

            StatusMsg("See log for links...");
        }


        private async void RenameCurrentCloudFile()
        {
            if (!HasGottenFolder()) return;
            Cloud_Elements_API.CloudFile currentRow = null;
            if (!HasCurrentCloudFile(ref currentRow)) return;
            if (currentRow.name == tsTxtObjectName.Text)
            {
                StatusMsg("Name not changed, ignored");
            }
            string oldName = currentRow.name;
            Cloud_Elements_API.CloudElementsConnector.DirectoryEntryType ceType = Cloud_Elements_API.CloudElementsConnector.DirectoryEntryType.File;
            if (currentRow.directory) ceType = Cloud_Elements_API.CloudElementsConnector.DirectoryEntryType.Folder;
            currentRow.path = txtFolderPath.Text + tsTxtObjectName.Text;
            Cloud_Elements_API.CloudFile Result = await APIConnector.PatchDocEntryMetaData(ceType, Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.ID, currentRow.id, currentRow);
            StatusMsg(string.Format("Renamed [{0}] to {1}", oldName, Result.name));
            Task refresh = RefreshCurrentFolder();
        }

        private async void getMetadataByPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!HasGottenFolder()) return;
            Cloud_Elements_API.CloudFile currentRow = null;
            Cloud_Elements_API.CloudFile CloudFileInfoByPath = null;
            if (!HasCurrentCloudFile(ref currentRow)) return;
            Cloud_Elements_API.CloudElementsConnector.TraceLevel diagTraceWas = Cloud_Elements_API.CloudElementsConnector.DiagOutputLevel;
            try
            {
                Cloud_Elements_API.CloudElementsConnector.DiagOutputLevel = Cloud_Elements_API.CloudElementsConnector.TraceLevel.All;
                CloudFileInfoByPath = await Cloud_Elements_API.FileOperations.GetCloudFileInfo(APIConnector, Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.Path, currentRow.path);
                if (CloudFileInfoByPath == null) StatusMsg("Nothing Returned!  (not expecting not found)");
                else
                {
                    StatusMsg(string.Format("OK: ID is {0}, by [{2}], hash {1}", CloudFileInfoByPath.id, Cloud_Elements_API.FileOperations.ContentHash(APIConnector, CloudFileInfoByPath),
                        Cloud_Elements_API.FileOperations.LastWrittenBy(APIConnector, CloudFileInfoByPath)));
                }
            }
            catch (Exception ex)
            {
                StatusMsg(string.Format("FAILED: {0}", ex.Message));
            }
            finally
            {
                Cloud_Elements_API.CloudElementsConnector.DiagOutputLevel = diagTraceWas;
            }
        }

        private void tsTxtObjectName_Click(object sender, EventArgs e)
        {
            StatusMsg("Update name and press enter to rename...");
        }

        private void tsTxtObjectName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') {
                RenameCurrentCloudFile();
            }
        }

      

        private void saveCurrentSecretsAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveSecretsFileDialog1.InitialDirectory = WorkPath;
            saveSecretsFileDialog1.FileName = "Secrets - " + toolStripTxtConnectionNow.Text;
            if (saveSecretsFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cloud_Elements_API.Tools.StringToFile(APIAuthorization.ToJSonString(toolStripTxtConnectionNow.Text), saveSecretsFileDialog1.FileName);

            }

        }

        private void loadSecretsFromToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openSecretsFileDialog.InitialDirectory = WorkPath;
            openSecretsFileDialog.FileName = "";
            if (openSecretsFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadSecretsFromFile(openSecretsFileDialog.FileName);

            }
        }

        private async void cmdTestButton_Click(object sender, EventArgs e)
        {
          Task UnitTestTask = RunUnitTest();
          cmdTestButton.Enabled = false;
          await UnitTestTask;

          Boolean broken =  UnitTestTask.IsFaulted;

          if (!broken)
          {
              if (chkTestCleanup.Checked == true)
              {
                  Task UnitTestClean = CleanupUnitTest();
                  await UnitTestClean;
              }
          }

          TestStatusMsg("Summary: " + APIConnector.GetStatisticsSummary());

          if (chkAutoSaveLog.Checked)
          {
              string fn = System.IO.Path.Combine(WorkPath, "Test Log for " + toolStripTxtConnectionNow.Text) + ".log" ;
              Cloud_Elements_API.Tools.StringToFile(tbTestOutput.Text,fn );
          }
          cmdTestButton.Enabled = true;
        }

        private void cmdTestClearLog_Click(object sender, EventArgs e)
        {
            tbTestOutput.Text = "";
        }

        private async void cmdForceClean_Click(object sender, EventArgs e)
        {
            Task UnitTestClean = CleanupUnitTest();
            await UnitTestClean;
           
        }

        private async void cmdGetID_Click(object sender, EventArgs e)
        {
            frmGetCloudFileID frmGet = new frmGetCloudFileID();
            cmdGetID.Enabled = false;
            if (frmGet.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cloud_Elements_API.CloudElementsConnector.TraceLevel diagTraceWas = Cloud_Elements_API.CloudElementsConnector.DiagOutputLevel;
                try
                {
                    TestStatusMsg("Checking: " + frmGet.FileID);
                    Cloud_Elements_API.CloudFile CloudFileInfoByID;
                    Cloud_Elements_API.CloudElementsConnector.DiagOutputLevel = Cloud_Elements_API.CloudElementsConnector.TraceLevel.All;
                    CloudFileInfoByID = await Cloud_Elements_API.FileOperations.GetCloudFileInfo(APIConnector, Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.ID, frmGet.FileID);
                    if (CloudFileInfoByID == null) StatusMsg("Nothing Returned!  (not expecting not found)");
                    else
                    {
                        if (CloudFileInfoByID.directory)
                        {
                            txtFolderPath.Text = CloudFileInfoByID.path;
                        }
                        else {
                            txtFolderPath.Text = CloudFileInfoByID.path.Substring(0, CloudFileInfoByID.path.LastIndexOf("/"));
                        }
                        TestStatusMsg("Getting: " + CloudFileInfoByID.path);
                        Task refresh = RefreshCurrentFolder();
                    }
                }
                catch (Exception ex)
                {
                    StatusMsg(string.Format("FAILED: {0}", ex.Message));
                }
                finally
                {
                    Cloud_Elements_API.CloudElementsConnector.DiagOutputLevel = diagTraceWas;
                }
          
            }
            cmdGetID.Enabled = true;
        }

     


    }
}
