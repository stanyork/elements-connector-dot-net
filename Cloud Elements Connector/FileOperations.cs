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
        /// Copies a file (or folder)
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
        /// Obtains information about a cloud file by ID or path; returns NULL for not found
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
        /// Moves (and/or renames) a file (or folder)
        /// </summary>
        /// <param name="connector">The API connector instance</param>
        /// <param name="fileData">file object</param>
        /// <param name="newPath">new path</param>
        /// <returns>Updated CloudFile</returns>
        public static async Task<CloudFile> Move(CloudElementsConnector connector, CloudFile sourceFile, string newPath)
        {

            CloudElementsConnector.DirectoryEntryType deType = sourceFile.EntryType;
             
            CloudFile PatchData = new CloudFile();

            PatchData.id = sourceFile.id;
            PatchData.path = newPath;
            sourceFile = await connector.PatchDocEntryMetaData(deType, CloudElementsConnector.FileSpecificationType.ID, sourceFile.id, PatchData);
            return sourceFile;
        }

        /// <summary>
        /// Changes the name of a file (or folder)
        /// </summary>
        /// <param name="connector">The API connector instance</param>
        /// <param name="fileData">Cloud File Data</param>
        /// <param name="newFilename">new name</param>
        /// <returns>Updated CloudFile</returns>
        public static async Task<CloudFile> Rename(CloudElementsConnector connector, CloudFile targetFile, string newFilename)
        {
            if (newFilename.IndexOf("/") >= 0) throw new ArgumentException("newFilename cannot include path information");
            CloudElementsConnector.DirectoryEntryType deType = targetFile.EntryType;
            CloudFile PatchData = new CloudFile();

            PatchData.id = targetFile.id;
            PatchData.name = newFilename;
            targetFile = await connector.PatchDocEntryMetaData(deType, CloudElementsConnector.FileSpecificationType.ID, targetFile.id, PatchData);
            return targetFile;
        }

        /// <summary>
        /// Returns SHA1, if available 
        /// </summary>
        /// <returns></returns>
        public static string ContentHash(CloudElementsConnector connector, CloudFile targetFile)
        {
            if (!targetFile.HasRaw) return string.Empty;
            if (!connector.EndpointOptions.HasFileHashAlgorithm) return string.Empty;
            Newtonsoft.Json.Linq.JToken valueToken = targetFile.RawValue(connector.EndpointOptions.FileHashRawIDPath); // targetFile.raw.GetValue("sha1");
            if (valueToken == null) return string.Empty;
            return valueToken.ToString();
        }

        /// <summary>
        /// Returns email address of last file writer, if available
        /// </summary>
        /// <returns></returns>
        public static string LastWrittenBy(CloudElementsConnector connector, CloudFile targetFile)
        {
            if (!targetFile.HasRaw) return string.Empty;
            if (!connector.EndpointOptions.HasModifiedBy) return string.Empty;
            return targetFile.RawValue(connector.EndpointOptions.ModifiedByRawIDPath);
        }

    }
}
