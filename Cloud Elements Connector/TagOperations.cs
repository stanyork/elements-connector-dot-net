using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Elements_API
{
    public sealed class TagOperations
    {
        public static async Task SetTag(CloudElementsConnector connector, CloudElementsConnector.FileSpecificationType fileSpecType, string identifier, string tagValue)
        {
            CloudFile fileData;
            fileData = await connector.GetFileMetaData(  fileSpecType,   identifier);
            if (fileData.UpdateTag( tagValue))
            {
                // store
                fileData = await connector.PatchFileMetaData(CloudElementsConnector.FileSpecificationType.ID, fileData.id, fileData);
            }

             
        }
         

    }
}
