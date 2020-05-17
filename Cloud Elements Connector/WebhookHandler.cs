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
            String user = "";
            String password = "";  

            if (!string.IsNullOrWhiteSpace(basicCreds))
            {
                // *****Decode the authorisation String*****
                byte[] e = System.Convert.FromBase64String(basicCreds.Substring(6));
                String usernpass = new System.Text.ASCIIEncoding().GetString(e);

                // *****Split the username from the password*****
                  user = usernpass.Substring(0, usernpass.IndexOf(":"));
                  password = usernpass.Substring(usernpass.IndexOf(":") + 1);
                // check username and password
            }


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
                else if ( !ExternalDAL.CredentialsAreValid(basicCreds, user, password))
                {
                    response.StatusCode = HttpStatusCode.Forbidden;
                    response.Content = "not Authorized";
                }
                else if (!ExternalDAL.InstanceNameIsValid(Request.message.instanceName))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Content = "Unrecognized Instance Name";
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
            bool logEventBody = false;
            foreach (Event reqEvent in Request.message.events)
            {
                if (reqEvent.eventType == "UNKNOWN")
                {
                    string InferredEventType = reqEvent.eventType;
                    System.Diagnostics.Trace.WriteLine(string.Format("CloudElementsConnector:WebhookHandler.ProcessRequest() - UNKNOWN eventType, Inferred: [{0}]", InferredEventType));
                    logEventBody = true;
                    if (Request.message.elementKey.StartsWith("box", StringComparison.CurrentCultureIgnoreCase) ) {
                        if (Request.message.raw.source.path_collection != null && (Request.message.raw.source.path_collection.total_count > 1))
                        {
                            reqEvent.newPath = "";
                            foreach (var item in Request.message.raw.source.path_collection.entries)
                            {
                                if (!string.IsNullOrWhiteSpace(item.id) && item.id != "0") reqEvent.newPath += "/" + item.name;
                            }
                        }
                        switch (Request.message.raw.trigger)
                        {
                            case "FILE.UPLOADED":
                                reqEvent.eventType = "UPDATED";
                                break;
                            case "FILE.DELETED":
                            case "FILE.TRASHED":
                                reqEvent.eventType = "DELETED";
                                break;
                            case "METADATA_INSTANCE.CREATED":
                                reqEvent.eventType = "CREATED";
                                break;
                            default:
                                System.Diagnostics.Trace.Write(string.Format("CloudElementsConnector:WebhookHandler.ProcessRequest() - unsupported raw box trigger: [{0}]", Request.message.raw.trigger));
                                reqEvent.eventType = "UPDATED";
                                logEventBody = true;
                                break;
                        }

                    }
                    else {
                        System.Diagnostics.Trace.WriteLine(string.Format("CloudElementsConnector:WebhookHandler.ProcessRequest() - UNKNOWN eventType not handled for [{0}]", Request.message.elementKey));
                    }
                }



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
                     
                    default:
                        System.Diagnostics.Trace.Write(string.Format("CloudElementsConnector:WebhookHandler.ProcessRequest() - unsupported eventType: [{0}]", reqEvent.eventType));
                        logEventBody = true;
                        break;
                }
            }
            if(logEventBody)
                System.Diagnostics.Trace.WriteLine(string.Format("CloudElementsConnector:WebhookHandler.ProcessRequest() {0}", RequestBody));

        }


    }
}
