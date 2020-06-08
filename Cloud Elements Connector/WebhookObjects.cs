using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Elements_API
{

    public class WebhookBaseObject
    {
        public string severity { get; set; }
        public string createdDate { get; set; }
        public string topic { get; set; }
        public string action { get; set; }
        public string id { get; set; }
        public string user { get; set; }
    }

    public class BoxWebhookObject : WebhookBaseObject
    {
        public   BoxMessage message { get; set; }
    }
    public class ShareFileWebhookObject : WebhookBaseObject
    {
        public   ShareFileMessage message { get; set; }
    }

    public class MessageBase
    {
        public string elementKey { get; set; }
        public int accountId { get; set; }
        public string eventId { get; set; }
        public int companyId { get; set; }
        public int instanceId { get; set; }
        public int instance_id { get; set; }
        public string instanceName { get; set; }
        public string[] instanceTags { get; set; }
        
        public int userId { get; set; }
        public Event[] events { get; set; }
    }

    public class BoxMessage : MessageBase
    {
       
        public   BoxRaw raw { get; set; }
       
    }

    public class ShareFileMessage : MessageBase
    {
        public   ShareFileRaw raw { get; set; }
    }


    public class RawBase
    {
        public string itemId { get; set; }
        public string itemType { get; set; }
        public Version[] versions { get; set; }
        public string newItemId { get; set; }
        
        public string userId { get; set; }
        public Source source { get; set; }
        public string trigger { get; set; }
      
    }

    public class BoxRaw : RawBase
    {
        public string item_parent_folder_id { get; set; }
        public string _event { get; set; }
    }

    public class ShareFileRaw : RawBase
    {
        public CitrixShareFileRawEvent Event { get; set; }
    }


    public class CitrixShareFileResource
    {
        public Parent Parent { get; set; }
    }
    public class CitrixShareFileRawEvent
    {
       
        public string OperationName { get; set; }
        public CitrixShareFileResource Resource { get; set; }
    }


    public class Source
    {
        public PathCollection path_collection { get; set; }
        public string name { get; set; }
        public Parent parent { get; set; }
    }

    public class Parent
    {
        public string id { get; set; }  // BOX
        public string Id { get; set; }  // Citrix ShareFile
    }

    public class PathCollection
    {
        public PathEntry[] entries { get; set; }
        public int total_count { get; set; }

    }


    public class PathEntry
    {
        public string id { get; set; }
        public string name { get; set; }

    }

    public class Event
    {
        public string elementKey { get; set; }
        public string eventType { get; set; }
        public string newPath { get; set; }
        public string hubKey { get; set; }
        public string objectId { get; set; }
        public string objectType { get; set; }
        public string parentObjectId { get; set; }

    }


    public class Version
    {
        public string sha1 { get; set; }
        public int size { get; set; }
        public Modified_By modified_by { get; set; }
        public string name { get; set; }
        public DateTime? purged_at { get; set; }
        public DateTime created_at { get; set; }
        public string id { get; set; }
        public DateTime modified_at { get; set; }
        public string type { get; set; }
        public DateTime? trashed_at { get; set; }
    }

    public class Modified_By
    {
        public string name { get; set; }
        public string id { get; set; }
        public string login { get; set; }
        public string type { get; set; }
    }


}
