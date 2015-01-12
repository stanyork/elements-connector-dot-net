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
            string tfoldername = "/Cloud Elements API Test Folder";
            Boolean remnant = false;
            DateTime StartTime = DateTime.Now;
            Cloud_Elements_API.CloudFile TestFileStore;
        
            //FIRST TEST: Check for folder. If no folder, create test folder.
            try
            {
                List<Cloud_Elements_API.CloudFile> fe = await APIConnector.ListFolderContents(Cloud_Elements_API.CloudElementsConnector.FileSpecificationType.Path, tfoldername, chkWithTags.Checked);
                remnant = true;
                TestStatusMsg("Using existing test folder");
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                //NOTE: handle different http exceptions!
                remnant = false;
            }
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
            //SECOND TEST: Create file.
            TestStatusMsg("Test: Uploading file...");
            try
            {

                using (System.IO.Stream teststream = GenerateStreamFromString("This is a dummy test file"))
                {
                    string MIMEType = Cloud_Elements_API.Tools.FileTypeToMimeContentType("txt");
                    List<String> TagList = new List<String>();
                    var sizeInBytes = teststream.Length;
                    string TargetPath = tfoldername + "/SFCE_test_file_prime.txt";
                    Cloud_Elements_API.CloudFile Result = await APIConnector.PostFile(teststream, MIMEType,
                                                               TargetPath, "Temporary test file",
                                                               TagList.ToArray(), false, sizeInBytes);
                    TestStatusMsg(string.Format("Uploaded {0}, id={1}", Result.path, Result.id));
                  
                    TestFileStore = Result;

                }


            }
            catch (Exception eu)
            {
                TestStatusMsg("Upload file failed: " + eu.Message);
                TestFileStore = null;
            }
            

            //THIRD TEST: Copy File.
            TestStatusMsg("Test: Copying file...");
            try
            {
                //System.IO.Stream newStream;
                //Cloud_Elements_API.Tools.StreamCopyWithProgress(

                TestStatusMsg("Skipping copy test for now.");
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

            //FIFTH TEST: Add a Tag
            TestStatusMsg("Test: Adding Tag...");

            try
            {
                string TagToSet = "API Test Tag";
                Cloud_Elements_API.CloudFile currentRow = TestFileStore;
                await Cloud_Elements_API.TagOperations.SetTag(APIConnector, currentRow, TagToSet);

                TestStatusMsg("Tag added: " + TagToSet);
               
            }
            catch (Exception etag)
            {
                TestStatusMsg("Add Tag failed: " + etag.Message);
            }

            ////SIXTH TEST: Remove a Tag
            //TestStatusMsg("Test: Adding (another) Tag...");
            //try
            //{
            //    string DelTagToSet = "Delete This Tag!";
            //    Cloud_Elements_API.CloudFile currentRow = TestFileStore;
            //    await Cloud_Elements_API.TagOperations.SetTag(APIConnector, currentRow, DelTagToSet);

            //    TestStatusMsg("Tag added: " + DelTagToSet);

            //}
            //catch (Exception etag)
            //{
            //    TestStatusMsg("Add Tag failed: " + etag.Message);
            //}
            //try
            //{
            //    string TagToDel = "Delete This Tag!";
            //    Cloud_Elements_API.CloudFile currentRow = TestFileStore;
            //    await Cloud_Elements_API.TagOperations.REMOVETAG(APIConnector, currentRow, TagToDel);
            //}
            //catch (Exception edt)
            //{
            //    TestStatusMsg("Delete Tag failed: " + edt.Message);
            //}



            TestStatusMsg(string.Format("Test suite complete: Elapsed time: {0:F1}s", DateTime.Now.Subtract(StartTime).TotalSeconds));
            
        }


        private async Task CleanupUnitTest()
        {

            TestStatusMsg("Cleaning up test...");
            string tfoldername = "/Cloud Elements API Test Folder";
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





    }
}
