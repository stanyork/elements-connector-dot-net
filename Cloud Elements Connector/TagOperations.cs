using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Elements_API
{
    public sealed class TagOperations
    {
        /// <summary>
        /// Gets CloudFile Meta Data, updates tags and stores if necessary
        /// </summary>
        /// <param name="connector">The API connector instance</param>
        /// <param name="fileSpecType">Indicates if ID or Path</param>
        /// <param name="identifier">ID or Path</param>
        /// <param name="tagValue">tag to be stored</param>
        /// <returns>Update CloudFile</returns>
        public static async Task<CloudFile> SetTag(CloudElementsConnector connector, CloudElementsConnector.FileSpecificationType fileSpecType, string identifier, string tagValue)
        {
            List<string> tagValues = new List<string>();
            tagValues.Add(tagValue);
            CloudFile fileData;
            fileData = await SetTag(connector, fileSpecType, identifier, tagValues);
            return fileData;
        }

        /// <summary>
        /// Gets CloudFile Meta Data, updates tags and stores if necessary
        /// </summary>
        /// <param name="connector">The API connector instance</param>
        /// <param name="fileSpecType">Indicates if ID or Path</param>
        /// <param name="identifier">ID or Path</param>
        /// <param name="tagValues">list of tags to be stored</param>
        /// <returns>Update CloudFile</returns>
        public static async Task<CloudFile> SetTag(CloudElementsConnector connector, CloudElementsConnector.FileSpecificationType fileSpecType, string identifier, List<string> tagValues)
        {
            CloudFile fileData;
            fileData = await connector.GetFileMetaData(fileSpecType, identifier);
            fileData = await SetTag(connector, fileData, tagValues);
            return fileData;
        }

        /// <summary>
        /// Updates tags and stores if necessary 
        /// </summary>
        /// <param name="connector">The API connector instance</param>
        /// <param name="fileData">Cloud File Data, including current tags (if any)</param>
        /// <param name="tagValue">tag to be stored</param>
        /// <returns>Update CloudFile</returns>
        public static async Task<CloudFile> SetTag(CloudElementsConnector connector,  CloudFile fileData, string tagValue)
        {
            List<string> tagValues = new List<string>();
            tagValues.Add(tagValue);
            fileData = await SetTag(connector, fileData, tagValues);
            return fileData;
        }

        /// <summary>
        /// Updates tags and stores if necessary 
        /// </summary>
        /// <param name="connector">The API connector instance</param>
        /// <param name="fileData">Cloud File Data, including current tags (if any)</param>
        /// <param name="tagValues">list of tags to be stored</param>
        /// <returns>Update CloudFile</returns>
        public static async Task<CloudFile> SetTag(CloudElementsConnector connector, CloudFile fileData, List<string> tagValues)
        {
            bool mustStore = false;
            if (!connector.EndpointOptions.SupportsTags)
            {
                if (fileData.HasTags) fileData.tags = null;
                return fileData;
            }
            foreach (var tagItem in tagValues)
            {
                if (fileData.UpdateTag(tagItem)) if (!mustStore) mustStore = true;    
            }
            
            if (mustStore)
            {
                // store
                CloudElementsConnector.DirectoryEntryType deType =  CloudElementsConnector.DirectoryEntryType.File ;
                if (fileData.directory)
                {
                    deType = CloudElementsConnector.DirectoryEntryType.Folder;
                    throw new ArgumentException("CloudFile must point to a file; folders do not support tags");
                }
                CloudFile PatchData = new CloudFile();
                PatchData.id = fileData.id;
                PatchData.tags = fileData.tags;
                fileData = await connector.PatchDocEntryMetaData(deType, CloudElementsConnector.FileSpecificationType.ID, fileData.id, PatchData);
            }
            return fileData;
        }

        /// <summary>
        /// Removes tags and stores if necessary 
        /// </summary>
        /// <param name="connector">The API connector instance</param>
        /// <param name="fileData">Cloud File Data, including current tags (if any)</param>
        /// <param name="tagValue">tag to be removed</param>
        /// <returns>Update CloudFile</returns>
        public static async Task<CloudFile> DeleteTag(CloudElementsConnector connector, CloudFile fileData, string tagValue)
        {
            List<string> tagValues = new List<string>();
            tagValues.Add(tagValue);
            fileData = await DeleteTag(connector, fileData, tagValues);
            return fileData;
        }

        /// <summary>
        /// Removes a tag and updates if necessary
        /// </summary>
        /// <param name="connector">The API connector instance</param>
        /// <param name="fileData">Cloud File Data, including current tags (if any)</param>
        /// <param name="tagValues">list of tags to be removed</param>
        /// <returns>Update CloudFile</returns>
        public static async Task<CloudFile> DeleteTag(CloudElementsConnector connector, CloudFile fileData, List<string> tagValues)
        {
            bool mustStore = false;
            foreach (var tagItem in tagValues)
            {
                if (fileData.RemoveTag(tagItem)) if (!mustStore) mustStore = true;
            }

            if (mustStore)
            {
                // store
                CloudElementsConnector.DirectoryEntryType deType = CloudElementsConnector.DirectoryEntryType.File;
                if (fileData.directory)
                {
                    deType = CloudElementsConnector.DirectoryEntryType.Folder;
                    throw new ArgumentException("CloudFile must point to a file; folders do not support tags");
                }
                CloudFile PatchData = new CloudFile();
                PatchData.id = fileData.id;
                PatchData.tags = fileData.tags;
                fileData = await connector.PatchDocEntryMetaData(deType, CloudElementsConnector.FileSpecificationType.ID, fileData.id, PatchData);
            }
            return fileData;
        }
    }
}
