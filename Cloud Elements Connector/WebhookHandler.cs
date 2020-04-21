using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;

namespace Cloud_Elements_API
{
    public class WebhookHandler
    {
        protected IWebhookActions ExternalDAL;
        protected WebhookBaseObject Request;
        public WebhookHandler()
        {

        }

        public RestSharp.RestResponse ValidateRequest(string requestBody)
        {

            RestSharp.RestResponse response = new RestSharp.RestResponse();
            response.ContentEncoding = "application/json";
            response.StatusCode = HttpStatusCode.Accepted;
            response.Content = "Success";

            if (string.IsNullOrEmpty(requestBody))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = "Request body null or empty.";
            }
            else
            {
                try
                {
                    RestSharp.Deserializers.JsonDeserializer deserializer = new RestSharp.Deserializers.JsonDeserializer();
                    Request = SimpleJson.SimpleJson.DeserializeObject<WebhookBaseObject>(requestBody);
                }
                catch (Exception ex)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Content = "Request was in wrong format. -> " + ex.Message;
                }
                if (Request == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Content = "Request object was null";
                }
                else if (Request.message == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Content = "Request Message was null";
                }
                else if ((Request.message.events == null) || (Request.message.events.Length == 0))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Content = "Request events array is null or empty.";
                }
            }

            return response;
        }

        public void SetExternalDAL(IWebhookActions actionsDAL)
        {
            ExternalDAL = actionsDAL;
        }


        public void ProcessRequest()
        {
            foreach (Event reqEvent in Request.message.events)
            {
                switch (reqEvent.eventType)
                {
                    case "CREATED":
                        //ProcessCreate(Request, reqEvent);
                        ExternalDAL.Created(reqEvent.objectId, reqEvent.objectType, reqEvent.eventType,
                            Request.message.instanceName, reqEvent.newPath);
                        break;
                    case "UPDATED":  //TODO: find out where this value comes from.  Is this what comes in from an update?
                        ExternalDAL.Updated(reqEvent.objectId, reqEvent.objectType, reqEvent.eventType,
                            Request.message.instanceName, reqEvent.newPath);
                        break;
                    case "DELETED":
                        ExternalDAL.Deleted(reqEvent.objectId, reqEvent.objectType, reqEvent.eventType,
                            Request.message.instanceName);
                        // put the file back if they delete a file.
                        break;
                    default: break;
                }
            }
        }

        //protected void ProcessCreate(WebhookBaseObject request, Event requestEvent)
        //{
            //if (requestEvent.objectType == RequestObjectTypes.File)
            //{

            //}
            //else if (requestEvent.objectType == RequestObjectTypes.Folder)
            //{

            //}
        //}

        //protected void ProcessUpdate(WebhookBaseObject request, Event requestEvent)
        //{
        //    if (requestEvent.objectType == RequestObjectTypes.File)
        //    {

        //    }
        //    else if (requestEvent.objectType == RequestObjectTypes.Folder)
        //    {

        //    }

        //}
        //protected void ProcessDelete(WebhookBaseObject request, Event requestEvent)
        //{
        //    if (requestEvent.objectType == RequestObjectTypes.File)
        //    {

        //    }
        //    else if (requestEvent.objectType == RequestObjectTypes.Folder)
        //    {

        //    }
        //}

        
    }
}
