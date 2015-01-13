using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Elements_API
{
    public sealed class FileOperations
    {

        /// <summary>
        /// Changes the name of a file 
        /// </summary>
        /// <param name="connector">The API connector instance</param>
        /// <param name="fileData">Cloud File Data</param>
        /// <param name="targetPath">new path (including name)</param>
        /// <returns>CloudFile reference to the new file </returns>
        public static async Task<CloudFile> Copy(CloudElementsConnector connector, CloudFile sourceFile, string targetPath)
        {

            CloudElementsConnector.DirectoryEntryType deType = sourceFile.EntryType;   
            sourceFile = await connector.Copy(deType, CloudElementsConnector.FileSpecificationType.ID, sourceFile.id, targetPath);
            return sourceFile;
        }


        /// <summary>
        /// Obtains information about a cloud file by ID or path; returns NULL 
        /// </summary>
        /// <param name="connector">connection to use</param>
        /// <param name="specType">specifies format of supplied file specification</param>
        /// <param name="requestFilePath">file specification</param>
        /// <returns>null if file not found (404)</returns>
        public static async Task<CloudFile> GetCloudFileInfo(CloudElementsConnector connector,
                                                               CloudElementsConnector.FileSpecificationType specType,
                                                              string requestFilePath)
        {
            CloudFile CloudFileInfo;
            try
            {
                CloudFileInfo = await connector.GetFileMetaData(specType, requestFilePath);
            }
            catch (System.Net.Http.HttpRequestException hx)
            {
                if (hx.Message.IndexOf("404") > 0)
                {
                    CloudFileInfo = null;
                }
                else throw hx;
            }
            return CloudFileInfo;
        }


        /// <summary>
        /// Changes the name of a file 
        /// </summary>
        /// <param name="connector">The API connector instance</param>
        /// <param name="fileData">Cloud File Data</param>
        /// <param name="newPath">new path</param>
        /// <returns>Update CloudFile</returns>
        public static async Task<CloudFile> MoveFile(CloudElementsConnector connector, CloudFile sourceFile, string newPath)
        {

            CloudElementsConnector.DirectoryEntryType deType = sourceFile.EntryType;
             
            CloudFile PatchData = new CloudFile();

            PatchData.id = sourceFile.id;
            PatchData.path = newPath;
            sourceFile = await connector.PatchDocEntryMetaData(deType, CloudElementsConnector.FileSpecificationType.ID, sourceFile.id, PatchData);
            return sourceFile;
        }

        /// <summary>
        /// Changes the name of a file 
        /// </summary>
        /// <param name="connector">The API connector instance</param>
        /// <param name="fileData">Cloud File Data</param>
        /// <param name="newFilename">new name</param>
        /// <returns>Update CloudFile</returns>
        public static async Task<CloudFile> RenameFile(CloudElementsConnector connector, CloudFile targetFile, string newFilename)
        {
            if (newFilename.IndexOf("/") >= 0) throw new ArgumentException("newFilename cannot include path information");
            CloudElementsConnector.DirectoryEntryType deType = targetFile.EntryType;
            CloudFile PatchData = new CloudFile();

            PatchData.id = targetFile.id;
            PatchData.name = newFilename;
            targetFile = await connector.PatchDocEntryMetaData(deType, CloudElementsConnector.FileSpecificationType.ID, targetFile.id, PatchData);
            return targetFile;
        }



    }
}
