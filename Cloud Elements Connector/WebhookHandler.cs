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
        protected String RequestBody;
        public WebhookHandler()
        {

        }

        public RestSharp.RestResponse ValidateRequest(string requestBody, string basicCreds)
        {

            if (ExternalDAL == null)
                throw new Exception("ExternalDAL must be set.");

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
                    RequestBody = requestBody;
                    Request = Newtonsoft.Json.JsonConvert.DeserializeObject<WebhookBaseObject>(requestBody);
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
                else if ((string.IsNullOrEmpty(basicCreds))
                    &&(ExternalDAL.CredentialsAreValid(basicCreds))
                    &&(ExternalDAL.InstanceNameIsValid(Request.message.instanceName)))  
                {
                    response.StatusCode = HttpStatusCode.Forbidden;
                    response.Content = "not Authorized";
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
                        ExternalDAL.Created(reqEvent.objectId, reqEvent.objectType, reqEvent.eventType,
                            Request.message.instanceName, reqEvent.newPath);
                        break;
                    case "UPDATED":  //version updates (in Box directly)
                        ExternalDAL.Updated(reqEvent.objectId, reqEvent.objectType, reqEvent.eventType,
                            Request.message.instanceName, reqEvent.newPath);
                        break;
                    case "DELETED":
                        ExternalDAL.Deleted(reqEvent.objectId, reqEvent.objectType, reqEvent.eventType,
                            Request.message.instanceName);
                        // put the file back if they delete a file.
                        break;
                    case "UNKNOWN":
                        // wah           
                        string InferredEventType;
                        if (RequestBody.IndexOf("FILE.TRASHED", StringComparison.CurrentCultureIgnoreCase) > 0)
                        {
                            InferredEventType = "TRASHED";
                        }
                        else
                        { // FILE.UPLOADED etc
                            InferredEventType = "UPLOADED";
                        }
                        System.Diagnostics.Trace.Write(string.Format("CloudElementsConnector:WebhookHandler.ProcessRequest() - UNKNOWN eventType, Inferred: [{0}]", InferredEventType));

                        break;
                    default:
                        System.Diagnostics.Trace.Write(string.Format("CloudElementsConnector:WebhookHandler.ProcessRequest() - unsupported eventType: [{0}]", reqEvent.eventType));
                        break;
                }
            }
        }

        
    }
}
