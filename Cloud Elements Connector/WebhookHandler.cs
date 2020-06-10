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
        protected BoxWebhookObject BoxRequest;
        protected ShareFileWebhookObject ShareFileRequest;
        protected string InstanceName;
        protected static System.Collections.Generic.Dictionary<string, int> FYISeen = new System.Collections.Generic.Dictionary<string, int>();
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
                    if (requestBody.IndexOf("\"elementKey\":\"box\"") > 0)
                    {
                        BoxRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<BoxWebhookObject>(requestBody);
                    }

                    else
                    {
                        ShareFileRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<ShareFileWebhookObject>(requestBody);
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Content = "Request was in wrong format. -> " + ex.Message;
                    Cloud_Elements_API.Tools.TraceWrite(string.Format("<?> CloudElementsConnector:WebhookHandler.ValidateRequest() - [{0}]", response.Content));
                }
                if (BoxRequest == null && ShareFileRequest == null)
                {
                    if (response.StatusCode != HttpStatusCode.Accepted)
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Content = "Request object was null";
                    }
                }
                else if ((BoxRequest != null && BoxRequest.message == null) || (ShareFileRequest != null && ShareFileRequest.message == null))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Content = "Request Message was null";
                }
                if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    if (BoxRequest != null)
                    {
                        if ((BoxRequest.message.events == null) || (BoxRequest.message.events.Length == 0))
                        {
                            response.StatusCode = HttpStatusCode.BadRequest;
                            response.Content = "Request events array is null or empty.";
                        }
                        InstanceName = BoxRequest.message.instanceName;
                    }

                    if (ShareFileRequest != null)
                    {
                        if ((ShareFileRequest.message.events == null) || (ShareFileRequest.message.events.Length == 0))
                        {
                            response.StatusCode = HttpStatusCode.BadRequest;
                            response.Content = "Request events array is null or empty.";
                        }
                        InstanceName = ShareFileRequest.message.instanceName;
                    }

                    if (!ExternalDAL.CredentialsAreValid(basicCreds, user, password))
                    {
                        response.StatusCode = HttpStatusCode.NonAuthoritativeInformation;
                        response.Content = "not Authorized";
                    }
                    else if (!ExternalDAL.InstanceNameIsValid(InstanceName))
                    {
                        response.StatusCode = HttpStatusCode.NonAuthoritativeInformation;
                        response.Content = "Unrecognized Instance Name";
                    }
                }
            }

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                Cloud_Elements_API.Tools.TraceWrite(string.Format("<?> CloudElementsConnector:WebhookHandler.ValidateRequest() - [{0}]", response.Content));

            }

            return response;
        }

        public void SetExternalDAL(IWebhookActions actionsDAL)
        {
            ExternalDAL = actionsDAL;
        }

        private bool ProcessShareFileRequest()
        {
            bool logEventBody = false;
            foreach (Event reqEvent in ShareFileRequest.message.events)
            {
                if (string.IsNullOrWhiteSpace(reqEvent.parentObjectId))
                { 
                        if (ShareFileRequest.message.raw.Event != null &&
                            ShareFileRequest.message.raw.Event.Resource != null &&
                            ShareFileRequest.message.raw.Event.Resource.Parent != null &&
                            !string.IsNullOrWhiteSpace(ShareFileRequest.message.raw.Event.Resource.Parent.Id))
                        {
                            reqEvent.parentObjectId = ShareFileRequest.message.raw.Event.Resource.Parent.Id;
                        }
                }

                logEventBody = logEventBody | ProcessEvent(reqEvent);
            }
            return logEventBody;
        }
        private bool ProcessBoxRequest()
        {
            bool logEventBody = false;
            try
            {
                foreach (Event reqEvent in BoxRequest.message.events)
                {
                    if (reqEvent.eventType == "UNKNOWN")
                    {
                        string InferredEventType = reqEvent.eventType;
                        string RawEventType = BoxRequest.message.raw.trigger;
                        string RawEventSource = "trigger";
                        if (string.IsNullOrWhiteSpace(RawEventType))
                        {
                            RawEventType = BoxRequest.message.raw.event_type;
                            RawEventSource = "event_type";
                        }
                        logEventBody = true;
                        switch (RawEventType)
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
                            case "commented":
                                reqEvent.eventType = "COMMENTED";
                                break;
                            default:
                                Cloud_Elements_API.Tools.TraceWrite(string.Format("<?> CloudElementsConnector:WebhookHandler.ProcessBoxRequest() - unsupported raw box trigger: [{0}.{1}]", RawEventSource, RawEventType));
                                reqEvent.eventType = "UPDATED";
                                logEventBody = true;
                                break;
                        }
                        Cloud_Elements_API.Tools.TraceWrite(string.Format("CloudElementsConnector:WebhookHandler.ProcessBoxRequest() - UNKNOWN {1} {2} {3} eventType, Inferred: [{0}]", InferredEventType, BoxRequest.message.elementKey,RawEventSource, RawEventType));
                    }

                    if (BoxRequest.message.raw != null)
                    {
                        if (string.IsNullOrWhiteSpace(reqEvent.newPath))
                        {
                            if (BoxRequest.message.raw.source != null && BoxRequest.message.raw.source.path_collection != null && (BoxRequest.message.raw.source.path_collection.total_count > 1))
                            {
                                reqEvent.newPath = "";
                                foreach (var item in BoxRequest.message.raw.source.path_collection.entries)
                                {
                                    if (!string.IsNullOrWhiteSpace(item.id) && item.id != "0") reqEvent.newPath += "/" + item.name;
                                }
                            }
                        }
                        if (string.IsNullOrWhiteSpace(reqEvent.parentObjectId))
                        {
                            if (BoxRequest.message.raw.source != null && BoxRequest.message.raw.source.parent != null && (!string.IsNullOrWhiteSpace(BoxRequest.message.raw.source.parent.id)))
                            {
                                reqEvent.parentObjectId = BoxRequest.message.raw.source.parent.id;
                            }
                            else if (BoxRequest.message.raw.item_parent_folder_id != null && (!string.IsNullOrWhiteSpace(BoxRequest.message.raw.item_parent_folder_id)))
                            {
                                reqEvent.parentObjectId = BoxRequest.message.raw.item_parent_folder_id;
                            }
                        }
                    }
                    else
                    {
                        Cloud_Elements_API.Tools.TraceWrite(string.Format("<?> CloudElementsConnector:WebhookHandler.ProcessBoxRequest() - {0} missing RAW", "", BoxRequest.message.elementKey));
                        logEventBody = true;
                    }
                    logEventBody = logEventBody | ProcessEvent(reqEvent);
                }
            } 
            catch (Exception ex)
            {
                Cloud_Elements_API.Tools.TraceWrite(string.Format("<!> CloudElementsConnector:WebhookHandler.ProcessBoxRequest() - Failed: {0}", ex.ToString()));
                logEventBody = true;
            }
            return logEventBody;
        }
         
        private   bool ProcessEvent(Event reqEvent)
        {
            bool logEventBody = false;
            switch (reqEvent.eventType)
            {
                case "CREATED":
                    ExternalDAL.Created(reqEvent.objectId, reqEvent.objectType, reqEvent.eventType,
                        InstanceName, reqEvent.parentObjectId, reqEvent.newPath);
                    break;
                case "UPDATED":  //version updates (often arrive as CREATED anyhow)
                    ExternalDAL.Updated(reqEvent.objectId, reqEvent.objectType, reqEvent.eventType,
                        InstanceName, reqEvent.parentObjectId, reqEvent.newPath);
                    break;
                case "DELETED":
                    ExternalDAL.Deleted(reqEvent.objectId, reqEvent.objectType, reqEvent.eventType,
                        InstanceName, reqEvent.parentObjectId);
                    // put the file back if they delete a file.
                    break;
                case "RETRIEVED":
                case "COMMENTED":
                    int SeenCount = 0;
                    lock (FYISeen)
                    {
                        if (!FYISeen.ContainsKey(reqEvent.eventType)) FYISeen.Add(reqEvent.eventType, SeenCount);
                        SeenCount = FYISeen[reqEvent.eventType];
                        SeenCount += 1;
                        FYISeen[reqEvent.eventType] = SeenCount;
                    }
                    if ((SeenCount % 10) == 1)
                        Cloud_Elements_API.Tools.TraceWrite(string.Format("CloudElementsConnector:WebhookHandler.ProcessEvent([{0}]) - FYI, Count={1}", reqEvent.eventType,SeenCount));
                    break;
                default:
                    Cloud_Elements_API.Tools.TraceWrite(string.Format("<?> CloudElementsConnector:WebhookHandler.ProcessEvent() - unsupported eventType: [{0}]", reqEvent.eventType));
                    logEventBody = true;
                    break;
            }
            return   logEventBody  ;
        }


        public void ProcessRequest()
        {
            bool logEventBody = false;
            if (ShareFileRequest != null) logEventBody = ProcessShareFileRequest();
            else if (BoxRequest != null) logEventBody = ProcessBoxRequest();

 
            if (logEventBody)
                Cloud_Elements_API.Tools.TraceWrite(string.Format("CloudElementsConnector:WebhookHandler.ProcessRequest() {0}", RequestBody));

        }


    }
}
