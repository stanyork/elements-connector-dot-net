﻿using System;
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
        public Message message { get; set; }
        public string user { get; set; }
    }

    public class Message
    {
        public string elementKey { get; set; }
        public int accountId { get; set; }
        public string eventId { get; set; }
        public int companyId { get; set; }
        public int instanceId { get; set; }
        public int instance_id { get; set; }
        public string instanceName { get; set; }
        public string[] instanceTags { get; set; }
        public Raw raw { get; set; }
        public int userId { get; set; }
        public Event[] events { get; set; }
    }

    public class Raw
    {
        public string itemId { get; set; }
        public string itemType { get; set; }
        public object[] versions { get; set; }
        public string newItemId { get; set; }
        public string _event { get; set; }
        public string userId { get; set; }
    }

    public class Event
    {
        public string elementKey { get; set; }
        public string eventType { get; set; }
        public string newPath { get; set; }
        public string hubKey { get; set; }
        public string objectId { get; set; }
        public string objectType { get; set; }
    }

}
