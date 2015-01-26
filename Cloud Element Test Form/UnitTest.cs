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

        public System.IO.Stream GenerateStreamFromString(string s)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            System.IO.StreamWriter writer = new System.IO.StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        private async Task RunUnitTest()
        {
            TestStatusMsg("Beginning test suite...");
            TestStatusMsg("Testing with " + toolStripTxtConnectionNow.Text);
            string basepath = txtFolderPath.Text;
            if (basepath.EndsWith("/")) { basepath.TrimEnd(new char[] { '/' }); }
            string tfoldername = basepath + "/Cloud Elements API Test Folder";
            Boolean remnant = false;
            Boolean fremnant = false;
            DateTime StartTime = DateTime.Now;
            Cloud_Elements_API.CloudFile TestFileStore;

            //FIRST TEST: Check for folder. If no folder, create test folder.
            try
            {
                // List<Cloud_Elements_API.CloudFile> fe = await APIConnector.ListFolderContents(Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.Path, tfoldername, chkWithTags.Checked);
                Cloud_Elements_API.CloudFile Result = await Cloud_Elements_API.FileOperations.GetCloudFileInfo(APIConnector, Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.Path, tfoldername);
                if (Result != null)
                {
                    remnant = true;
                    TestStatusMsg("Using existing test folder");
                }
                else { remnant = false; }
            }
            //catch (System.Net.Http.HttpRequestException e)
            //{
            //    //NOTE: handle different http exceptions!
            //    remnant = false;
            //}
            catch (Exception ex)
            {
                TestStatusMsg("Problem checking if test folder exists: " + ex.Message);
                throw ex;
            }
            if (!remnant)
            {
                TestStatusMsg("Test: Creating folder...");
                try
                {
                    Cloud_Elements_API.CloudFile newFolder = new Cloud_Elements_API.CloudFile();
                    newFolder.path = tfoldername;
                    //newFolder.tags = new string[] { "sfCE.NET" };
                    Cloud_Elements_API.CloudFile Result = await APIConnector.CreateFolder(newFolder);
                    TestStatusMsg(string.Format("Created {0}, id={1}", Result.path, Result.id));
                    Task ignoredRefreshFolderTask = RefreshCurrentFolder(); // very naughty, ignore exceptions; ref http://stackoverflow.com/questions/14903887/warning-this-call-is-not-awaited-execution-of-the-current-method-continues
                }
                catch (Exception ec)
                {
                    TestStatusMsg("Create Folder failed: " + ec.Message);
                    throw ec;
                }
            }

            TestStatusMsg("Checking for test file...");
            try
            {
                string TargetPath = tfoldername + "/SFCE_test_file_prime.txt";
                Cloud_Elements_API.CloudFile Result = await Cloud_Elements_API.FileOperations.GetCloudFileInfo(APIConnector, Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.Path, TargetPath);
                if (Result != null)
                {
                    TestStatusMsg(string.Format("Test file exists: {0}, id={1}", Result.path, Result.id));
                    fremnant = true;
                    TestFileStore = Result;
                }
                else { TestStatusMsg("Test file not found"); TestFileStore = null; }


            }
            catch (Exception ec)
            {
                TestStatusMsg("Check for file failed: " + ec.Message);
                TestFileStore = null;
                //     throw ec;
            }

            double UploadRequiredMS = 0d;
            if (!fremnant)
            {

                //SECOND TEST: Create file.
                TestStatusMsg("Test: Uploading file...");
                try
                {

                    using (System.IO.Stream teststream = GenerateStreamFromString("This is a dummy test file; its diminutive size is quite reasonable."))
                    {
                        string MIMEType = Cloud_Elements_API.Tools.FileTypeToMimeContentType("txt");
                        List<String> TagList = new List<String>();
                        var sizeInBytes = teststream.Length;
                        string TargetPath = tfoldername + "/SFCE_test_file_prime.txt";
                        DateTime Started = DateTime.Now;
                        Cloud_Elements_API.CloudFile Result = await APIConnector.PostFile(teststream, MIMEType,
                                                                   TargetPath, "Temporary test file",
                                                                   TagList.ToArray(), false, sizeInBytes);
                        UploadRequiredMS = DateTime.Now.Subtract(Started).TotalMilliseconds;
                        TestStatusMsg(string.Format("Uploaded {0}, id={1}", Result.path, Result.id));

                        TestFileStore = Result;

                    }


                }
                catch (Exception eu)
                {
                    TestStatusMsg("Upload file failed: " + eu.Message);
                    TestFileStore = null;
                }
            }
            else
            {
                TestStatusMsg("Skipping file upload");

            }

            //THIRD TEST: Copy File.
            TestStatusMsg("Test: Copying file...");
            double CopyRequiredMS = 0d;
            try
            {
                if (TestFileStore == null) { TestFileStore = await Cloud_Elements_API.FileOperations.GetCloudFileInfo(APIConnector, Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.Path, tfoldername + "/SFCE_test_file_prime.txt"); }
                string CopyFileName = tfoldername + "/SFCE_test_file_copy.txt";
                Cloud_Elements_API.CloudFile currentRow = TestFileStore;
                DateTime Started = DateTime.Now;
                Cloud_Elements_API.CloudFile Result = await Cloud_Elements_API.FileOperations.Copy(APIConnector, currentRow, CopyFileName);
                CopyRequiredMS = DateTime.Now.Subtract(Started).TotalMilliseconds;
                TestStatusMsg(string.Format("Copied [{0}] to {1}", currentRow.name, Result.path));
                if (UploadRequiredMS > 0) TestStatusMsg(string.Format("Comparison: Upload {0:F1}ms; Copy {1:F1}ms; Pct {2:P2} ", UploadRequiredMS, CopyRequiredMS, CopyRequiredMS / UploadRequiredMS));
            }
            catch (Exception ecopy)
            {
                TestStatusMsg("Copy File failed: " + ecopy.Message);

            }


            //FOURTH TEST: Rename File.
            TestStatusMsg("Test: Renaming file (by path)...");
            try
            {
                string oldName = "SFCE_test_file_prime.txt";
                string oldPath = tfoldername + "/" + oldName;
                Cloud_Elements_API.CloudElementsConnector.DirectoryEntryType ceType = Cloud_Elements_API.CloudElementsConnector.DirectoryEntryType.File;
                Cloud_Elements_API.CloudFile currentRow = TestFileStore;
                currentRow.path = tfoldername + "/SFCE_test_file_rename_path.txt";
                Cloud_Elements_API.CloudFile Result = await APIConnector.PatchDocEntryMetaData(ceType, Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.Path, oldPath, currentRow);

                TestStatusMsg(string.Format("Renamed [{0}] to {1}", oldName, Result.name));
                TestFileStore = Result;
            }
            catch (Exception er)
            {
                TestStatusMsg("Rename file by path failed: " + er.Message);
            }
            TestStatusMsg("Test: Renaming file (by ID)...");
            try
            {

                Cloud_Elements_API.CloudElementsConnector.DirectoryEntryType ceType = Cloud_Elements_API.CloudElementsConnector.DirectoryEntryType.File;
                Cloud_Elements_API.CloudFile currentRow = TestFileStore;
                string oldName = TestFileStore.name;
                currentRow.path = tfoldername + "/SFCE_test_file_rename_id.txt";

                Cloud_Elements_API.CloudFile PatchData = new Cloud_Elements_API.CloudFile();
                PatchData.id = currentRow.id;
                PatchData.path = currentRow.path;
                Cloud_Elements_API.CloudFile Result = await APIConnector.PatchDocEntryMetaData(ceType, Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.ID, TestFileStore.id, PatchData);

                TestStatusMsg(string.Format("Renamed [{0}] to {1}", oldName, Result.name));
                TestFileStore = Result;
            }
            catch (Exception er)
            {
                TestStatusMsg("Rename file failed: " + er.Message);
            }

            //FIFTH TEST: Add two Tags
            TestStatusMsg("Test: Adding Tags...");

            try
            {

                for (int i = 1; i <= 2; i++)
                {
                    string TagToSet = "API Test Tag #" + i.ToString();
                    Cloud_Elements_API.CloudFile currentRow = TestFileStore;
                    TestFileStore = await Cloud_Elements_API.TagOperations.SetTag(APIConnector, currentRow, TagToSet);

                    TestStatusMsg(string.Format("Tag <{0}> added to [{1}] ", TagToSet, currentRow.name));

                }


            }
            catch (Exception etag)
            {
                TestStatusMsg("Add Tag failed: " + etag.Message);
            }

            //SIXTH TEST: Remove a Tag
            TestStatusMsg("Test: Deleting Tags...");

            try
            {
                for (int i = 1; i <= 2; i++)
                {
                    string TagToDel = "API Test Tag #" + i.ToString();
                    Cloud_Elements_API.CloudFile currentRow = TestFileStore;
                    TestFileStore = await Cloud_Elements_API.TagOperations.DeleteTag(APIConnector, currentRow, TagToDel);
                    TestStatusMsg(string.Format("Tag <{0}> removed from [{1}] ", TagToDel, currentRow.name));
                }
            }
            catch (Exception edt)
            {
                TestStatusMsg("Delete Tag failed: " + edt.Message);
            }

            //SEVENTH TEST: Async Uploads
            TestStatusMsg("Test: Multiple ASYNC uploads...");
            try
            {
                string info;
                // we need to run away from the UI
                System.Func<Task<string>> UploadTestAction = new System.Func<Task<string>>(TestMultiFileAsyncUploads);

                Task<Task<string>> AsyncUploadTest = new Task<Task<string>>(UploadTestAction);
                AsyncUploadTest.Start();
                Task<string> ResultTask = await AsyncUploadTest.ConfigureAwait(false);
                info = await ResultTask;
                TestStatusMsg(info);
            }
            catch (Exception eu)
            {
                TestStatusMsg("Async Uploads   failed: " + eu.Message);

            }


            //EIGHTH TEST: Download File
            TestStatusMsg("Test: Downloading file...");
            try
            {
                string TargetPath = tfoldername + string.Format("/SFCE_test_file_{0}.txt", "1");
                Cloud_Elements_API.CloudFile FileRow = await Cloud_Elements_API.FileOperations.GetCloudFileInfo(APIConnector, Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.Path, TargetPath);
                if (FileRow != null)
                {
                    Cloud_Elements_API.FileContent Result = await APIConnector.GetFile(FileRow.id);
                    string fn = System.IO.Path.Combine(WorkPath, Result.Disposition);
                    if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(fn)))
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fn));
                    }
                    if (System.IO.File.Exists(fn))
                    {
                        System.IO.File.Delete(fn);
                    }
                    System.IO.Stream Target = new System.IO.FileStream(fn, System.IO.FileMode.Create, System.Security.AccessControl.FileSystemRights.FullControl, System.IO.FileShare.None, 16384, System.IO.FileOptions.Asynchronous, null);
                    await Cloud_Elements_API.Tools.StreamCopyWithProgress(Result.ContentStream, Target, Result.ContentLength);
                    Result.ContentStream.Close();
                    Target.Close();
                    System.IO.FileInfo finfo = new System.IO.FileInfo(fn);
                    TestStatusMsg(string.Format("Stored {1}: {0}", Cloud_Elements_API.Tools.SizeInBytesToString(finfo.Length), Result.Disposition));
                    int DownloadedHash = Cloud_Elements_API.Tools.FileToString(fn).GetHashCode();
                    if (DownloadedHash != DownloadTestExpectedHash)
                    {
                        TestStatusMsg(string.Format("Warning: Hash of {0} does not match", fn));
                    }
                }
                else
                {
                    TestStatusMsg(string.Format("Could not find file to download ({0})", TargetPath));
                }

            }
            catch (Exception ed)
            {
                TestStatusMsg("File download failed: " + ed.Message);
            }

            TestStatusMsg(string.Format("Test suite complete: Elapsed time: {0:F1}s", DateTime.Now.Subtract(StartTime).TotalSeconds));

        }


        private async Task CleanupUnitTest()
        {

            TestStatusMsg("Cleaning up test...");
            string basepath = txtFolderPath.Text;
            if (basepath.EndsWith("/")) { basepath.TrimEnd(new char[] { '/' }); }
            string tfoldername = basepath +  "/Cloud Elements API Test Folder";

            //check for file on disk
            string fn = System.IO.Path.Combine(WorkPath, "SFCE_test_file_1.txt");
            if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(fn)))
            {
               if (System.IO.File.Exists(fn))
                   {
                       System.IO.File.Delete(fn);
                       TestStatusMsg("Downloaded test file deleted");
                   }
            }
            

            //check for folder
            try
            {
                List<Cloud_Elements_API.CloudFile> fe = await APIConnector.ListFolderContents(Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.Path, tfoldername, chkWithTags.Checked);

                //if exists, delete
                try
                {
                    Cloud_Elements_API.CloudFile Result = await APIConnector.DeleteFolder(tfoldername, false);
                    TestStatusMsg("Test folder deleted");
                }
                catch (Exception e)
                {
                    TestStatusMsg("Delete Folder failed: " + e.Message);
                    throw e;
                }

            }
            catch (System.Net.Http.HttpRequestException e)
            {
                TestStatusMsg("Nothing to clean: " + e.Message);
            }

            TestStatusMsg("Test cleanup complete");

        }


        public static string GenerateRandomContent(int bitLen)
        {
            if (bitLen <= 8) bitLen = 128;
            if (bitLen % 8 != 0) throw new ArgumentException("bitLen must be a multiple of 8");
            int bytesNeeded = Convert.ToInt32(bitLen / 8) - 1;
            byte[] buff = new byte[bytesNeeded + 1];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(buff);
            StringBuilder sb = new StringBuilder(bytesNeeded * 2);
            int i = 0;
            for (i = 0; i <= buff.Length - 1; i++)
            {
                sb.AppendFormat("{0:X2}", buff[i]);
            }
            return sb.ToString();
        }


        int DownloadTestExpectedHash;

        async Task<string> TestMultiFileAsyncUploads()
        {
            string basepath = txtFolderPath.Text;
            if (basepath.EndsWith("/")) { basepath.TrimEnd(new char[] { '/' }); }
            string tfoldername = basepath + "/Cloud Elements API Test Folder";
            System.Collections.Generic.Queue<Task<Cloud_Elements_API.CloudFile>> UploadTasks = new System.Collections.Generic.Queue<Task<Cloud_Elements_API.CloudFile>>();
            //System.Collections.Generic.Queue<System.Runtime.CompilerServices.ConfiguredTaskAwaitable<Cloud_Elements_API.CloudFile>> UploadTasks = new System.Collections.Generic.Queue<System.Runtime.CompilerServices.ConfiguredTaskAwaitable<Cloud_Elements_API.CloudFile>>();
            DateTime Started = DateTime.Now;
            int uploadCount = 0;
            System.IO.Stream teststream;
            string RandomContent = GenerateRandomContent(128 * 1024 * 8);
            DownloadTestExpectedHash = RandomContent.GetHashCode();
            long TotalBytes = 0;
            for (int i = 1; i <= 8; i++)
            {
                teststream = GenerateStreamFromString(RandomContent);
                RandomContent = RandomContent.Substring(RandomContent.Length / 2);
                string MIMEType = Cloud_Elements_API.Tools.FileTypeToMimeContentType("txt");
                List<String> TagList = new List<String>();
                long sizeInBytes = teststream.Length;
                TotalBytes += sizeInBytes;
                string TargetPath = tfoldername + string.Format("/SFCE_test_file_{0}.txt", i);
                Task<Cloud_Elements_API.CloudFile> AnUpload = APIConnector.PostFile(teststream, MIMEType,
                                                           TargetPath, "Temporary test file",
                                                           TagList.ToArray(), false, sizeInBytes);
                //System.Runtime.CompilerServices.ConfiguredTaskAwaitable<Cloud_Elements_API.CloudFile> AwaitableUpload;
                //AwaitableUpload                  =    AnUpload.ConfigureAwait(false);

                UploadTasks.Enqueue(AnUpload);
                uploadCount += 1;
            }

            int rewaitCnt = 0;
            while (UploadTasks.Count > 0)
            {
                Task<Cloud_Elements_API.CloudFile> AnUpload = UploadTasks.Dequeue();

                if (AnUpload.GetAwaiter().IsCompleted)
                {
                    Cloud_Elements_API.CloudFile Result = await AnUpload;
                    TestStatusMsg(string.Format("Task {2} Uploaded {4:F1}KB to {0}, id={1}, ok={3}", Result.name, Result.id, AnUpload.Id, !AnUpload.IsFaulted, Result.size / 1024d));
                }
                else
                {
                    //   if (AnUpload.Status == TaskStatus.WaitingForActivation) AnUpload.ci
                    UploadTasks.Enqueue(AnUpload);
                    rewaitCnt += 1;
                    System.Threading.Thread.Sleep(128);
                }

            }

            double RequiredMS = DateTime.Now.Subtract(Started).TotalMilliseconds;
            return string.Format("Uploaded {0} files in {1:F2}s, average {2:F2}s / file, {4:F2}ms/KB; Async Waits: {3}", uploadCount, RequiredMS / 1000d, RequiredMS / (1000d * uploadCount),
                                             rewaitCnt, (RequiredMS) / (TotalBytes / 1024d));

        }


    }
}
