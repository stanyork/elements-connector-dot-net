using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Elements_API
{
    public interface IWebhookActions
    {
        void Created(string objectID, string objectType, string eventType, string instanceName, string newPath);
        //void Updated(string objectID, string objectType, string eventType, string instanceName, string newPath);
        void Deleted(string objectID, string objectType, string eventType, string instanceName);

    }
}
