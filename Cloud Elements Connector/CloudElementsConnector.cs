﻿using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;


namespace Cloud_Elements_API
{
    public class CloudElementsConnector
    {
        #region "privates"
        //static readonly string DefaultElementsPublicURL = "https://api.cloud-elements.com/elements/api-v2/";
        //static readonly string DefaultElementsPublicURL = "https://staging.cloud-elements.com/elements/api-v2/";
        //"https://staging.cloud-elements.com/elements/api-v2/"
        private readonly static string StaticLockObject = "";
        // note: InstanceLockObject is APIClient;
        private static int ConnectorInstanceCounter = 0;
        private static int HttpClientInstanceCounter = 0;
        private static int RequestCounter = 0;
        private static double TotalRequestMS = 0d;
        private int ConnectorInstanceNumber = 0;
        private int InstanceRequestCounter = 0;
        private int InstanceThrottleDelayCnt = 0;
        private double InstanceTotalRequestMS = 0d;
        private double InstanceThrottleDelayMS = 0d;
        private static System.Collections.Generic.Dictionary<string, EndpointOptions> EndpointSettings = new System.Collections.Generic.Dictionary<string, EndpointOptions>();
        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Queue<DateTime>> EndpointRecentRequests = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Queue<DateTime>>();
        private static System.Collections.Generic.Queue<CloudElementsConnector> FactoryRefurbishedQueue = new System.Collections.Generic.Queue<CloudElementsConnector>();
        // ...hubs/documents/folders/contents?path=%2FSQL&fetchTags=true"
        // ...hubs/documents/files/21794645297/metadata
        private Cloud_Elements_API.CloudAuthorization AuthorizationData;
        private HttpClient APIClient;
        private DateTime InstanceCreated;
        private string Endpoint = "Unknown";
        private string LastFailureInformation = "";
        #endregion

        public static bool WriteDiagTrace = true;
        public static bool SimplifyLoggedURIs = true;
        public static int MaxTimeOutMS = 1680000; //  28 minutes
        public static TraceLevel DiagOutputLevel = TraceLevel.NonSuccess;
        public delegate void DiagTraceEventHanlder(object sender, string info);
        public event DiagTraceEventHanlder DiagTrace;

        public string ElementsPublicUrl { get; set; }
        public Cloud_Elements_API.CloudAuthorization APIAuthorization
        {
            set
            {
                AuthorizationData = value;

                if (value == null)
                {
                    APIClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("basic", "empty");
                }
                else
                {
                    APIClient.DefaultRequestHeaders.Authorization = AuthorizationData.GetHeaderValue();
                    AssureEnpointControlData(Endpoint);
                    ElementsPublicUrl = AuthorizationData.ApiUrl;
                }
            }
        }

        /// <summary>
        /// Returns max requests per second to the current endpoint
        /// </summary>
        public int EndpointMaxRequestsPerSecond
        {
            get
            {
                int result = -1;
                lock (StaticLockObject)
                {
                    if (EndpointSettings.ContainsKey(Endpoint))
                    {
                        EndpointOptions options = EndpointSettings[Endpoint];
                        result = options.MaxRqPerSecond;
                    }
                }
                return result;
            }
            set
            {
                AssureEnpointControlData(Endpoint);
                lock (StaticLockObject)
                {
                    EndpointOptions options = EndpointSettings[Endpoint];
                    options.MaxRqPerSecond = value;
                }
            }
        }

        /// <summary>
        /// Returns the time when the last 'rate exceeded' result was detected by the connector
        /// </summary>
        public DateTime WhenRateLastExceeded
        {
            get
            {
                DateTime result = DateTime.MinValue;
                if (EndpointSettings.ContainsKey(Endpoint))
                {
                    EndpointOptions options = EndpointSettings[Endpoint];
                    result = options.LastRateExceeded;
                }
                return result;
            }
        }

        /// <summary>
        /// Returns the current endpoint options in use by this connector
        /// </summary>
        public EndpointOptions EndpointOptions
        {
            get
            {
                EndpointOptions result = null;
                if (EndpointSettings.ContainsKey(Endpoint))
                {
                    result = EndpointSettings[Endpoint];
                }
                return result;
            }
        }


        #region "Constructors"
        //public CloudElementsConnector(string elementsURL)
        //{
        //    if (!string.IsNullOrEmpty(elementsURL))
        //        Initialize(elementsURL);
        //    else
        //        Initialize(DefaultElementsPublicURL);
        //}


        public CloudElementsConnector(CloudAuthorization auth)
        {
            InstanceCreated = DateTime.Now;
            ElementsPublicUrl = auth.ApiUrl;
            APIClient = NewHttpClient();
            this.APIAuthorization = auth;

            lock (StaticLockObject)
            {
                ++ConnectorInstanceCounter;
                ConnectorInstanceNumber = ConnectorInstanceCounter;
            }
        }

        //protected void Initialize(string elementsURL)
        //{
            
        //}

        /// <summary>
        /// Returns another APIConnector with the same authorization, endpoint optons, and trace handler
        /// </summary>
        /// <returns></returns>
        public CloudElementsConnector Clone()
        {
            CloudElementsConnector result;
            lock (StaticLockObject)
            {
                if (FactoryRefurbishedQueue.Count > 0)
                {
                    result = FactoryRefurbishedQueue.Dequeue();
                    result.APIClient = NewHttpClient();
                }
                else result = new CloudElementsConnector(AuthorizationData);
            }

            result.ElementsPublicUrl = this.ElementsPublicUrl;
            //result.APIAuthorization = this.AuthorizationData;
            result.DiagTrace = this.DiagTrace;
            result.Endpoint = this.Endpoint;
            return result;
        }
        #endregion

        /// <summary>
        /// Test the endpoint and sets the endpoint type, enabling endpoint-specific options such as rate throttling
        /// </summary>
        /// <returns></returns>
        public async Task<Pong> Ping()
        {
            HttpClient UseClient = CloneAPIClient();
            HttpResponseMessage response = await APIExecuteGet(UseClient,"hubs/documents/ping");
            Pong ResultPong = await response.Content.ReadAsAsync<Pong>();
            Endpoint = ResultPong.endpoint;
            AssureEnpointControlData(Endpoint);
            UseClient.Dispose();
            UseClient = null;
            return ResultPong;
        }

        private void AssureEnpointControlData(string endpointName)
        {
            lock (StaticLockObject)
            {
                if (!EndpointSettings.ContainsKey(endpointName))
                {
                    EndpointOptions options = new EndpointOptions();
                    options.LogThrottleDelays = false;
                    options.RequestsPerSecondWindow = 3;
                    options.EndpointType = endpointName;
                    switch (endpointName)
                    {
                        case "amazons3":
                            options.FileHashAlgorithmName = "MD5";
                            options.FileHashRawIDPath = "objectMetadata.rawMetadata.ETag";
                            break;
                        case "box":
                            options.MaxRqPerSecond = 6;
                            options.LogHighwaterThroughput = true;
                            options.FileHashAlgorithmName = "SHA1";
                            options.FileHashRawIDPath = "sha1";
                            options.ModifiedByRawIDPath = "modified_by.login";
                            break;
                        case "googledrive":
                            options.ModifiedByRawIDPath = "lastModifyingUser.emailAddress";
                            options.MaxRqPerSecond = 32;
                            break;
                        case "sharefile":
                            options.MaxRqPerSecond = 24;
                            options.SupportsCopy = true;
                            options.SupportsGetStorage = false;
                            options.SupportsTags = false;
                            break;

                        case "sharepoint":
                            options.MaxRqPerSecond = 32;
                            options.SupportsCopy = false;
                            options.SupportsGetStorage = false;
                            break;
                        case "dropboxbusiness":
                            options.MaxRqPerSecond = 48;
                            options.SetExtraHeaderID("Elements-As-Team-Member");
                            options.SupportsAsync = false; // http://support.cloud-elements.com/hc/requests/2835
                            break;
                        case "dropboxbusinessv2":
                            options.MaxRqPerSecond = 12;
                            options.SetExtraHeaderID("Elements-As-Team-Member");
                            options.SupportsAsync = true;  
                            break;
                        default:
                            options.MaxRqPerSecond = 32;
                            break;
                    }
                    EndpointSettings.Add(endpointName, options);
                }
                if (!EndpointRecentRequests.ContainsKey(endpointName)) EndpointRecentRequests.Add(endpointName, new Queue<DateTime>());

            }
        }


        /// <summary>
        /// (optional) Releases resources and authorization used by this instance and sends summary diag event
        /// </summary>
        public void Close()
        {
            if (InstanceRequestCounter > 0)
            {
                string traceInfo = string.Format("ce(close,) {0}", GetStatisticsSummary());
                //HttpClientInstanceCounter = 0;
                OnDiagTrace(traceInfo);
            }
            this.APIAuthorization = null;
            this.DiagTrace = null;
            InstanceRequestCounter = 0;
            InstanceTotalRequestMS = 0d;
            InstanceThrottleDelayCnt = 0;
            InstanceThrottleDelayMS = 0d;
            lock (StaticLockObject) FactoryRefurbishedQueue.Enqueue(this);
        }

        public string GetLastFailureInformation()
        {
            return LastFailureInformation;
        }

        public string GetStatisticsSummary()
        {
            string result;
            lock (StaticLockObject)
            {
                if (InstanceRequestCounter > 0)
                {
                    double LifeSpanMS = DateTime.Now.Subtract(InstanceCreated).TotalMilliseconds;
                    if (LifeSpanMS == 0) LifeSpanMS = 1;
                    string traceInfo = string.Format("#{0}/{1}  r={2}; Life={7:F1}s; Used={3:F1}s; Avg={4:F1}s; Busy={8:P1}; Throttled {9} by {10:F1}s; Connector Totals: r={5}, Used={6:F1}s ", ConnectorInstanceNumber, ConnectorInstanceCounter,
                                                        InstanceRequestCounter, InstanceTotalRequestMS / 1000d, InstanceTotalRequestMS / InstanceRequestCounter,
                                                        RequestCounter, TotalRequestMS / 1000d,
                                                        LifeSpanMS / 1000d, InstanceTotalRequestMS / LifeSpanMS,
                                                        InstanceThrottleDelayCnt, InstanceThrottleDelayMS / 1000d);
                    //HttpClientInstanceCounter = 0;
                    result = traceInfo;
                }
                else result = "No work has been performed";
            }
            return result;
        }

        #region "documents/files and folders"

        /// <summary>
        /// Retrieves the amount of storage available on your cloud service account
        /// </summary>
        /// <returns></returns>
        public async Task<CloudStorage> GetStorageAvailable()
        {
            CloudStorage Result;
            if (!EndpointOptions.SupportsGetStorage)
            {
                Result = new CloudStorage();
                Result.shared = -1;
                Result.total  = -1;
                Result.used = -1;
                return Result;
            }
            HttpClient UseClient = CloneAPIClient();
            HttpResponseMessage response = await APIExecuteGet(UseClient,"hubs/documents/storage");
            Result = await response.Content.ReadAsAsync<CloudStorage>();
            UseClient.Dispose();
            UseClient = null;
            return Result;
        }


        /// <summary>
        /// Retrieves specific metadata on a file or folder associated with an ID from your cloud service using its specified path. 
        /// </summary>
        /// <param name="entryType">Specifies if the identifier is a file or a folder</param>
        /// <param name="fileSpecType">Specifies if the identifier is an ID or a PATH</param>
        /// <param name="identifier">Specifying an ID that does not exist results in an error response.</param>
        /// <returns></returns>
        public async Task<CloudFile> GetDocEntryMetaData(DirectoryEntryType entryType, FileSpecificationType fileSpecType, string identifier, bool withRaw)
        {
            CloudFile Result;
            HttpResponseMessage response;
            HttpClient UseClient = CloneAPIClient();
            string RequestURL;
            string URLEntryType = "files";

            if (entryType == DirectoryEntryType.Folder) URLEntryType = "folders";
            switch (fileSpecType)
            {
                case FileSpecificationType.ID:
                    RequestURL = "hubs/documents/{1}/{0}/metadata?raw={2}";
                    break;
                case FileSpecificationType.Path:
                    RequestURL = "hubs/documents/{1}/metadata?path={0}&raw={2}";
                    break;
                default:
                    throw new ArgumentException("unsupported File Specification Type - " + fileSpecType.ToString());
            }
            RequestURL = string.Format(RequestURL, Tools.UrlEncode(identifier), URLEntryType,withRaw);

            response = await APIExecuteGet(UseClient, RequestURL);
            Result = await response.Content.ReadAsAsync<CloudFile>();
            UseClient.Dispose();
            UseClient = null;

            return Result;
        }

        /// <summary>
        /// Update a file or folder metadata associated with an ID or path
        /// </summary>
        /// <param name="entryType">Specifies if the identifier is a file or a folder</param>
        /// <param name="fileSpecType">Specifies if the identifier is an ID or a PATH</param>
        /// <param name="identifier">Specifying an ID that does not exist results in an error response.</param>
        /// <param name="fileData"></param>
        /// <returns>update file data</returns>
        /// <remarks>Update a file's metadata (tags and path) associated with an ID. 
        /// For example, if you had a document that was tagged as operations but needed to be tagged as legal, then you would perform a PATCH to update the tag using the tags JSON array field. 
        /// The PATCH method can update the name or directory of a file as well as move a file by using the path JSON field. 
        /// You cannot update the size of a file. Specifying a file associated with an ID that does not exist results in an error response.</remarks>
        public async Task<CloudFile> PatchDocEntryMetaData(DirectoryEntryType entryType, FileSpecificationType fileSpecType, string identifier, CloudFile fileData)
        {

            HttpResponseMessage response;
            HttpClient UseClient = CloneAPIClient();
            string RequestURL;
            string URLEntryType = "files";
            if (entryType == DirectoryEntryType.Folder) URLEntryType = "folders";
            switch (fileSpecType)
            {
                case FileSpecificationType.ID:
                    RequestURL = "hubs/documents/{1}/{0}/metadata";
                    break;
                case FileSpecificationType.Path:
                    RequestURL = "hubs/documents/{1}/metadata?path={0}";
                    break;
                default:
                    throw new ArgumentException("unsupported File Specification Type - " + fileSpecType.ToString());
            }

            RequestURL = string.Format(RequestURL, Tools.UrlEncode(identifier), URLEntryType);
            var content = CloudFileRequestContent(fileData);
            response = await APIExecutePatch(UseClient, RequestURL, content);
            fileData = await response.Content.ReadAsAsync<CloudFile>();
            UseClient.Dispose();
            UseClient = null;

            return fileData;
        }
        #endregion

        #region "documents/folders/..."
        /// <summary>
        /// Returns a list of (up to 2^31) CloudFile objects
        /// </summary>
        /// <param name="fileSpecType"></param>
        /// <param name="path"></param>
        /// <param name="withTags"></param>
        /// <returns></returns>
        public async Task<List<CloudFile>> ListFolderContents(FileSpecificationType fileSpecType, string path, bool withTags)
        {
            return await ListFolderContents(fileSpecType, path, int.MaxValue, 1, withTags);
        }

        /// <summary>
        /// Returns a partial list of files in the specified folder
        /// </summary>
        /// <param name="fileSpecType">ID or Path</param>
        /// <param name="path"></param>
        /// <param name="pageSize">from 10 to int.MaxValue</param>
        /// <param name="pageNum">from 1 to n</param>
        /// <param name="withTags"></param>
        /// <returns></returns>
        public async Task<List<CloudFile>> ListFolderContents(FileSpecificationType fileSpecType, string path, int pageSize, int pageNum ,bool withTags)
        {
            HttpResponseMessage response;
            HttpClient UseClient = CloneAPIClient();
            string RequestURL;
            if (pageSize < 10) pageSize = 10;
            //if (pageSize > Int16.MaxValue) pageSize = Int16.MaxValue;
            if (pageNum < 1) pageNum = 1;
            switch (fileSpecType)
            {
                case FileSpecificationType.ID:
                    RequestURL = "hubs/documents/folders/{0}/contents?&fetchTags={1}&pageSize={2}&page={3}";
                    break;
                case FileSpecificationType.Path:
                    RequestURL = "hubs/documents/folders/contents?path={0}&fetchTags={1}&pageSize={2}&page={3}";
                    break;
                default:
                    throw new ArgumentException("Unsupported Folder Specification Type - " + fileSpecType.ToString());
            }

            response = await APIExecuteGet(UseClient, string.Format(RequestURL, Tools.UrlEncode(path), withTags, pageSize, pageNum));
            List<CloudFile> ResultList = await response.Content.ReadAsAsync<List<CloudFile>>();
            UseClient.Dispose();
            UseClient = null;

            return ResultList;
        }

        public async Task<CloudFile> Copy(CloudFile sourceFile, string identifier, string targetPath)
        {
            return await Copy(sourceFile.EntryType, FileSpecificationType.ID, sourceFile.id, targetPath);
        }

        /// <summary>
        /// Copies a file or folder to the specified target
        /// </summary>
        /// <param name="entryType"></param>
        /// <param name="fileSpecType"></param>
        /// <param name="identifier"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public async Task<CloudFile> Copy(DirectoryEntryType entryType, FileSpecificationType fileSpecType, string identifier, string targetPath)
        {

            HttpResponseMessage response;
            HttpClient UseClient = CloneAPIClient();
            string RequestURL;
            string URLEntryType = "files";
            if (entryType == DirectoryEntryType.Folder) URLEntryType = "folders";
            switch (fileSpecType)
            {
                case FileSpecificationType.ID:
                    RequestURL = "hubs/documents/{1}/{0}/copy";
                    break;
                case FileSpecificationType.Path:
                    RequestURL = "hubs/documents/{1}/copy?path={0}";
                    break;
                default:
                    throw new ArgumentException("unsupported File Specification Type - " + fileSpecType.ToString());
            }

            RequestURL = string.Format(RequestURL, Tools.UrlEncode(identifier), URLEntryType);
            CloudFile fileData = new CloudFile();
            fileData.path = targetPath;
            var content = CloudFileRequestContent(fileData);
            response = await APIExecutePost(UseClient, RequestURL, content);
            fileData = await response.Content.ReadAsAsync<CloudFile>();
            UseClient.Dispose();
            UseClient = null;

            return fileData;
        }


        /// <summary>
        /// Creates a folder
        /// </summary>
        /// <param name="path">full path name of folder, must begin with a slash</param>
        /// <param name="tags">optional tags</param>
        /// <returns>Metadata for new folder</returns>
        /// <remarks></remarks>
        public async Task<CloudFile> CreateFolder(string path, string[] tags)
        {
            CloudFile newFolder = new CloudFile();
            newFolder.path = path;
            newFolder.tags = tags;
            return await CreateFolder(newFolder);
        }

        /// <summary>
        /// Creates a folder
        /// </summary>
        /// <param name="newFolder">Must have value in path that includes the full path name of folder, must begin with a slash</param>
        /// <returns></returns>
        public async Task<CloudFile> CreateFolder(CloudFile newFolder)
        {
            string URL = "hubs/documents/folders";
            HttpClient UseClient = CloneAPIClient();

            if (!newFolder.path.StartsWith("/")) throw new ArgumentException("Path must begin with a slash");

            var content = CloudFileRequestContent(newFolder);
            HttpResponseMessage response = await APIExecutePost(UseClient, URL, content);
            CloudFile Result = await response.Content.ReadAsAsync<CloudFile>();
            UseClient.Dispose();
            UseClient = null;

            return Result;
        }

        

        /// <summary>
        /// Deletes a specific folder from the cloud service   
        /// </summary>
        /// <param name="fileSpecType">Specifies if the identifier is an ID or a PATH</param>
        /// <param name="identifier">Specifying an ID that does not exist results in an error response.</param>
        /// <param name="emptyTrash">true also empties trash</param>
        /// <returns></returns>
        /// <remarks>Specifying a file associated with an ID that does not exist results in an error response.</remarks>
        public async Task<bool> DeleteFolder(FileSpecificationType fileSpecType, string identifier, bool emptyTrash)
        {
            bool Result;
            HttpResponseMessage response;
            HttpClient UseClient = CloneAPIClient();
            string RequestURL;

            switch (fileSpecType)
            {
                case FileSpecificationType.ID:
                    RequestURL = "hubs/documents/folders/{0}?emptyTrash={1}";
                    break;
                case FileSpecificationType.Path:
                    RequestURL = "hubs/documents/folders?path={0}&emptyTrash={1}";
                    break;
                default:
                    throw new ArgumentException("Unsupported Folder Specification Type - " + fileSpecType.ToString());
            }

            response = await APIExecuteDelete(UseClient, string.Format(RequestURL, Tools.UrlEncode(identifier), emptyTrash));
            UseClient.Dispose();
            UseClient = null;
            Result = true;
            return Result;
        }


        /// <summary>
        /// Retrieves specific metadata on a file associated with an ID from your cloud service using its specified path. 
        /// </summary>
        /// <param name="fileSpecType">Specifies if the identifier is an ID or a PATH</param>
        /// <param name="identifier">Specifying an ID that does not exist results in an error response.</param>
        /// <returns></returns>
        public async Task<CloudFile> GetFolderMetaData(FileSpecificationType fileSpecType, string identifier)
        {
            CloudFile Result;
            Result = await GetDocEntryMetaData(DirectoryEntryType.Folder, fileSpecType, identifier,true);
            return Result;
        }

        /// <summary>
        /// Update a file metadata associated with an ID or path
        /// </summary>
        /// <param name="fileSpecType">Specifies if the identifier is an ID or a PATH</param>
        /// <param name="identifier">Specifying an ID that does not exist results in an error response.</param>
        /// <param name="fileData"></param>
        /// <returns>update file data</returns>
        /// <remarks>Update a file's metadata (tags and path) associated with an ID. 
        /// For example, if you had a document that was tagged as operations but needed to be tagged as legal, then you would perform a PATCH to update the tag using the tags JSON array field. 
        /// The PATCH method can update the name or directory of a file as well as move a file by using the path JSON field. 
        /// You cannot update the size of a file. Specifying a file associated with an ID that does not exist results in an error response.</remarks>
        public async Task<CloudFile> PatchFolderMetaData(FileSpecificationType fileSpecType, string identifier, CloudFile fileData)
        {
            fileData = await PatchDocEntryMetaData(DirectoryEntryType.Folder, fileSpecType, identifier, fileData);
            return fileData;
        }
        #endregion

        #region "documents/files/..."

        /// <summary>
        /// Deletes a specific file from your cloud service   
        /// </summary>
        /// <param name="fileSpecType">Specifies if the identifier is an ID or a PATH</param>
        /// <param name="identifier">Specifying an ID that does not exist results in an error response.</param>
        /// <param name="emptyTrash">true also empties trash</param>
        /// <returns></returns>
        /// <remarks>Specifying a file associated with an ID that does not exist results in an error response.</remarks>
        public async Task<bool> DeleteFile(FileSpecificationType fileSpecType, string identifier, bool emptyTrash)
        {
            bool Result;
            HttpResponseMessage response;
            HttpClient UseClient = CloneAPIClient();
            string RequestURL;

            switch (fileSpecType)
            {
                case FileSpecificationType.ID:
                    RequestURL = "hubs/documents/files/{0}?emptyTrash={1}";
                    break;
                case FileSpecificationType.Path:
                    RequestURL = "hubs/documents/files?path={0}&emptyTrash={1}";
                    break;
                default:
                    throw new ArgumentException("Unsupported Folder Specification Type - " + fileSpecType.ToString());
            }

            response = await APIExecuteDelete(UseClient, string.Format(RequestURL, Tools.UrlEncode(identifier), emptyTrash));
            Result = true;
            UseClient.Dispose();
            UseClient = null;

            return Result;
        }




        /// <summary>
        /// Returns a link that can be used to download the specified file through Cloud Elements. The link can be used to download the file without providing credentials. 
        /// </summary>
        /// <param name="fileSpecType">Specifies if the identifier is an ID or a PATH</param>
        /// <param name="identifier">Specifying an ID that does not exist results in an error response.</param>
        /// <returns></returns>
        /// <remarks>Specifying a file that does not exist results in an error.</remarks>
        public async Task<CloudLink> FileLinks(FileSpecificationType fileSpecType, string identifier)
        {
            CloudLink Result;
            HttpResponseMessage response;
            HttpClient UseClient = CloneAPIClient();
            string RequestURL;
            switch (fileSpecType)
            {
                case FileSpecificationType.ID:
                    RequestURL = string.Format("hubs/documents/files/{0}/links", Tools.UrlEncode(identifier));
                    break;
                case FileSpecificationType.Path:
                    RequestURL = string.Format("hubs/documents/files/links?path={0}", Tools.UrlEncode(identifier));
                    break;
                default:
                    throw new ArgumentException("unsupported File Specification Type - " + fileSpecType.ToString());
            }
            response = await APIExecuteGet(UseClient,RequestURL);
            Result = await response.Content.ReadAsAsync<CloudLink>();
            UseClient.Dispose();
            UseClient = null;

            return Result;
        }


        /// <summary>
        /// Retrieves specific metadata on a file associated with an ID from your cloud service using its specified path. 
        /// </summary>
        /// <param name="fileSpecType">Specifies if the identifier is an ID or a PATH</param>
        /// <param name="identifier">Specifying an ID that does not exist results in an error response.</param>
        /// <returns></returns>
        public async Task<CloudFile> GetFileMetaData(FileSpecificationType fileSpecType, string identifier)
        {
            return await GetDocEntryMetaData(DirectoryEntryType.File, fileSpecType, identifier, true);
        }

        /// <summary>
        /// Update a file metadata associated with an ID or path
        /// </summary>
        /// <param name="fileSpecType">Specifies if the identifier is an ID or a PATH</param>
        /// <param name="identifier">Specifying an ID that does not exist results in an error response.</param>
        /// <param name="fileData"></param>
        /// <returns>update file data</returns>
        /// <remarks>Update a file's metadata (tags and path) associated with an ID. 
        /// For example, if you had a document that was tagged as operations but needed to be tagged as legal, then you would perform a PATCH to update the tag using the tags JSON array field. 
        /// The PATCH method can update the name or directory of a file as well as move a file by using the path JSON field. 
        /// You cannot update the size of a file. Specifying a file associated with an ID that does not exist results in an error response.</remarks>
        public async Task<CloudFile> PatchFileMetaData(FileSpecificationType fileSpecType, string identifier, CloudFile fileData)
        {

            HttpResponseMessage response;
            HttpClient UseClient = CloneAPIClient();
            string RequestURL;
            switch (fileSpecType)
            {
                case FileSpecificationType.ID:
                    RequestURL = string.Format("hubs/documents/files/{0}/metadata", Tools.UrlEncode(identifier));
                    break;
                case FileSpecificationType.Path:
                    RequestURL = string.Format("hubs/documents/files/metadata?path={0}", Tools.UrlEncode(identifier));
                    break;
                default:
                    throw new ArgumentException("unsupported File Specification Type - " + fileSpecType.ToString());
            }

            var content = CloudFileRequestContent(fileData);
            response = await APIExecutePatch(UseClient, RequestURL, content);
            fileData = await response.Content.ReadAsAsync<CloudFile>();
            UseClient.Dispose();
            UseClient = null;

            return fileData;
        }

        public async Task<FileContent> GetFile(string id)
        {
            //HttpResponseMessage response = await APIExecuteGet(string.Format("hubs/documents/files/{0}", System.Net.WebUtility.UrlEncode(id)));
            //FileContent Result = new FileContent(response);
            //Result.ContentStream = await response.Content.ReadAsStreamAsync();
            //return Result;
            return await GetFile(id, 0);
        }

        public async Task<FileContent> GetFile(CloudFile cf)
        {
            return await GetFile(cf.id, cf.size);
        }

        public async Task<FileContent> GetFile(string id, ulong size)
        {

            HttpClient UseClient = CloneAPIClient();
            FileContent Result;
            try
            {
                if (size > 0)
                {
                    UseClient.Timeout = UseClient.Timeout = CalculateTimeoutForBytesPerMS((long)size);
                }
                HttpResponseMessage response = await APIExecuteGet(UseClient,string.Format("hubs/documents/files/{0}", System.Net.WebUtility.UrlEncode(id)));
                Result = new FileContent(response, UseClient);
                Result.ContentStream = await response.Content.ReadAsStreamAsync();
                
            }
            finally
            {
                // note: cannot dispose the HttpClient until the stream is consumed, so will happen later
                //   APIClient.Timeout = WasTimeout;
                //  UseClient.Dispose();
            }
            return Result;
        
        }



        /// <summary>
        /// Uploads a file from a stream
        /// </summary>
        /// <param name="uploadSource"></param>
        /// <param name="contentType">image/jpeg or whatever is appropriate for the file type</param>
        /// <param name="path">full path and file name for cloud storage</param>
        /// <param name="description"></param>
        /// <param name="tags">Array of strings</param>
        /// <param name="overwrite"></param>
        /// <param name="sizeInBytes">Only required for Sharepoint; ignored if less than or = 0</param>
        /// <returns>metadata for the newly uploaded file</returns>
        public async Task<CloudFile> PostFile(System.IO.Stream uploadSource, string contentType, string path,
                                                string description,
                                                string[] tags,
                                                Boolean overwrite,
                                                long sizeInBytes)
        {
            string URL = string.Format("hubs/documents/files?path={0}&description={1}"
                                        , Tools.UrlEncode(path)
                                        , System.Net.WebUtility.UrlEncode(description)
                                        );
            // neither is RFC3986 compliant
            // System.Net.WebUtility.UrlEncode(path); or System.Uri.EscapeDataString(path); ??? see http://blog.nerdbank.net/2009/05/uriescapedatapath-and.html
            if (overwrite) URL += "&overwrite=true";
            if (sizeInBytes > 0) URL += "&size=" + sizeInBytes.ToString();
            if ((tags != null) && (tags.Length > 0))
            {
                if (EndpointOptions.SupportsTags)
                {
                    var tagCnt = 0;
                    foreach (string tag in tags)
                    {
                        if ((tag != null) && (tag.Trim().Length > 0))
                        {
                            if (tagCnt == 0) URL += "&tags%5B%5D=";
                            else URL += ",";
                            tagCnt++;
                            URL += System.Net.WebUtility.UrlEncode(tag.Trim());
                        }
                    }
                }
            }

            // ref http://stackoverflow.com/questions/16416601/c-sharp-httpclient-4-5-multipart-form-data-upload

            var content = new MultipartFormDataContent(String.Format("----------{0:N}", Guid.NewGuid()));
            var FileContentSection = new StreamContent(uploadSource, 32768);
            content.Add(FileContentSection);
            FileContentSection.Headers.ContentDisposition.Name = "file";
            FileContentSection.Headers.ContentDisposition.FileName = System.IO.Path.GetFileName(path);
            FileContentSection.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
            HttpClient UseClient = CloneAPIClient();
            CloudFile Result;
            try
            {
                DateTime SendStartedAt = DateTime.Now;
                UseClient.Timeout = CalculateTimeoutForBytesPerMS(sizeInBytes);
                HttpResponseMessage response = await APIExecutePost(UseClient, URL, content);
                Result = await response.Content.ReadAsAsync<CloudFile>();
                UpdateMSperKB( DateTime.Now.Subtract(SendStartedAt), sizeInBytes);
            }
            finally
            {
             //   APIClient.Timeout = WasTimeout;
                UseClient.Dispose();
            }
            return Result;

        }




        #endregion

        // ----------------------------

        #region "Request Support"

        public void OnDiagTrace(string info)
        {
            if (DiagTrace != null)
            {
                DiagTrace(this, info);
            }
            else
            {
                info = string.Format("{0}: {1}", Tools.TraceTimeNow(), info);
                System.Diagnostics.Trace.WriteLineIf(WriteDiagTrace, info);
            }
        }

        HttpContent CloudFileRequestContent(CloudFile filedata)
        {
            Newtonsoft.Json.JsonSerializerSettings jsonSettings = new Newtonsoft.Json.JsonSerializerSettings();
            jsonSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(filedata, jsonSettings);

            var content = new StringContent(jsonData);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            return content;
        }

        async Task<HttpResponseMessage> APIExecuteGet(string URI)
        {
            return await APIExecuteVerb(HttpVerb.Get, URI);
        }

        async Task<HttpResponseMessage> APIExecuteGet(HttpClient withClient, string URI)
        {
            return await APIExecuteVerb(withClient, HttpVerb.Get, URI,null);
        }

        async Task<HttpResponseMessage> APIExecuteDelete(HttpClient withClient,string URI)
        {
            return await APIExecuteVerb(withClient,HttpVerb.Delete, URI, null);
        }

        async Task<HttpResponseMessage> APIExecutePost(string URI, HttpContent content)
        {
            return await APIExecuteVerb(HttpVerb.Post, URI, content);
        }

        async Task<HttpResponseMessage> APIExecutePost(HttpClient withClient, string URI, HttpContent content)
        {
            return await APIExecuteVerb(withClient, HttpVerb.Post, URI, content);
        }

        async Task<HttpResponseMessage> APIExecutePatch(HttpClient withClient, string URI, HttpContent content)
        {
            return await APIExecuteVerb(withClient, HttpVerb.Patch, URI, content);
        }

        enum HttpVerb
        {
            Get,
            Delete,
            Post,
            Patch
        }

        public enum FileSpecificationType
        {
            ID,
            Path
        }

        public enum DirectoryEntryType
        {
            Folder,
            File
        }


        public enum TraceLevel
        {
            NonSuccess,
            All,
            Verbose
        }

        async Task<HttpResponseMessage> APIExecuteVerb(HttpVerb verb, string URI)
        {
            return await APIExecuteVerb(verb, URI, null);
        }

        readonly int minTimeOutMS = 99123;
         
        private double MSperKB = 2.5;
        private double OneMB = (1024.0 * 1024.0);
        private long HighWaterTimeout = 120000;
        private double SlowestKBRate = 8;
        private double FastestKBRate = 5;
        private TimeSpan CalculateTimeoutForBytesPerMS(long sizeInBytes)
        {
            int msTimeout =   (int)Math.Ceiling((sizeInBytes / 1024.0) * MSperKB);
            if (msTimeout < minTimeOutMS) msTimeout += minTimeOutMS; // internal default is 100 seconds
            if (msTimeout > MaxTimeOutMS) msTimeout = MaxTimeOutMS;
            if (msTimeout > HighWaterTimeout)
            {
                OnDiagTrace(string.Format("ce(?) New high request timeout {0:F1}s for {1:F1}MB" ,msTimeout / 1000, sizeInBytes / 1024.0 / 1024.0));
                HighWaterTimeout = msTimeout;
            }
            return new TimeSpan(0, 0, 0, 0, msTimeout); // allow 16ms per KB
        }

        private void UpdateMSperKB(TimeSpan timeUsed, long sizeInBytes)
        {
            if (sizeInBytes < (6.0*OneMB)) return;
            int msPerKBUsed = (int)Math.Ceiling(timeUsed.TotalMilliseconds / (sizeInBytes / 1024.0) );
            if (msPerKBUsed > 1000) return;
            if (msPerKBUsed < 2) return;
            if ((msPerKBUsed < FastestKBRate) || (msPerKBUsed > SlowestKBRate)) {
                OnDiagTrace(string.Format("ce(?) New throughput rate {0:F1}ms/KB", msPerKBUsed));
                if (msPerKBUsed < FastestKBRate)  FastestKBRate = msPerKBUsed;
                if (msPerKBUsed > SlowestKBRate) SlowestKBRate  = msPerKBUsed;
            }
            if (msPerKBUsed < MSperKB) return;
            MSperKB = msPerKBUsed;
        }



        /// <summary>
        /// Manages the requests per second, adding delays if/when needed
        /// </summary>
        private void ThrottleRequestsPerSecond(int forcedMS )
        {
            EndpointOptions options;
            if (EndpointSettings.ContainsKey(Endpoint))
            {
                options = EndpointSettings[Endpoint];
            }
            else
            {
                OnDiagTrace("ce(?) Settings missing for endpoint [" + Endpoint + "]");
                System.Threading.Thread.Sleep(128);
                return;
            }
            if ((options.MaxRqPerSecond <= 0) && (forcedMS <= 0)) return;
            int delayMS = 0;
            double RequestCountSinceBaseline;
            DateTime BaselineRequestAt;
            System.Collections.Generic.Queue<DateTime> RecentRqQ;
            lock (StaticLockObject)
            {
                RecentRqQ = EndpointRecentRequests[Endpoint];
                while ((RecentRqQ.Count > 0) && (DateTime.Now.Subtract(RecentRqQ.Peek()).TotalSeconds > options.RequestsPerSecondWindow))
                {
                    RecentRqQ.Dequeue();
                }
                if (RecentRqQ.Count > 1)
                {
                    BaselineRequestAt = RecentRqQ.Peek(); RequestCountSinceBaseline = RecentRqQ.Count + 1;
                    double TotalSecsSinceBaseline = DateTime.Now.Subtract(BaselineRequestAt).TotalSeconds;
                    if ((TotalSecsSinceBaseline < 0.1) && (!options.SupportsAsync)) TotalSecsSinceBaseline = 0.1;
                    double RecentReqPerSecond = (RequestCountSinceBaseline / TotalSecsSinceBaseline);
                    if ((RecentReqPerSecond > options.HighwaterGeneratedRequestsPerSecond) && (TotalSecsSinceBaseline > 0.5))
                    {
                        options.HighwaterGeneratedRequestsPerSecond = RecentReqPerSecond;
                        if (options.LogHighwaterThroughput) OnDiagTrace(string.Format("ce(throughput) [{0}] reached {1:F2}r/s", Endpoint, RecentReqPerSecond));
                    }

                    if (RecentReqPerSecond >= options.MaxRqPerSecond)
                    {
                        delayMS = (int)Math.Ceiling((1000.0 / options.MaxRqPerSecond) * (RecentReqPerSecond - options.MaxRqPerSecond));
                        if (delayMS > 3333) delayMS = 3333;
                    }
                    else if ((RecentReqPerSecond > 1) && (RecentReqPerSecond > (options.MaxRqPerSecond / 2)))
                    {
                        delayMS = (int)Math.Ceiling((1000.0 / options.MaxRqPerSecond) * (RecentReqPerSecond / options.MaxRqPerSecond));
                        if (delayMS > 2222) delayMS = 2222;
                    }
                }
                else
                {
                    BaselineRequestAt = DateTime.Now; RequestCountSinceBaseline = 1;
                    delayMS = 0;
                }

                if (delayMS > 0)
                {
                    InstanceThrottleDelayCnt++;
                    InstanceThrottleDelayMS += delayMS;
                    // note: actual delay happens outside lock()
                }

            }
            delayMS += forcedMS;
            if (delayMS > 0)
            {
                if (options.LogThrottleDelays) OnDiagTrace(string.Format("ce(throttled) [{0}] delayed {1}ms", Endpoint, delayMS));
                InstanceThrottleDelayCnt++;
                InstanceThrottleDelayMS += delayMS;
                System.Threading.Thread.Sleep(delayMS);
            }
            lock (StaticLockObject)
            {
                RecentRqQ.Enqueue(DateTime.Now);
            }
        }

        /// <summary>
        /// Executes a cloud elements request.  Every request comes through here
        /// </summary>
        /// <param name="verb"></param>
        /// <param name="URI"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        async Task<HttpResponseMessage> APIExecuteVerb(HttpVerb verb, string URI, HttpContent content)
        {
            return await APIExecuteVerb(APIClient, verb, URI, content);
        }




   


        /// <summary>
        /// Executes a cloud elements request.  Every request comes through here
        /// </summary>
        /// <param name="withClient">Instance of HttpClient</param>
        /// <param name="verb">Get, Delete, Post, Patch</param>
        /// <param name="URI"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        async Task<HttpResponseMessage> APIExecuteVerb(HttpClient withClient, HttpVerb verb, string URI, HttpContent content)
        {
            ThrottleRequestsPerSecond(0);
            DateTime startms = DateTime.Now;
            lock (StaticLockObject) RequestCounter++;
            InstanceRequestCounter++;
            HttpResponseMessage response;
            Task<HttpResponseMessage> HttpRequestTask;
            EndpointOptions options;
            EndpointOptions AsyncOptionsLockTarget = null;
            try
            {
                if (EndpointSettings.ContainsKey(Endpoint))
                {
                    options = EndpointSettings[Endpoint];
                    if (!options.SupportsAsync)
                    {
                        AsyncOptionsLockTarget = options;
                        bool myTurn = false;
                        DateTime myWait = DateTime.Now;
                        while (!myTurn)
                        {
                            lock (AsyncOptionsLockTarget)
                            {
                                myTurn = AsyncOptionsLockTarget.SetHttpClientBusy();
                            }  //lock (AsyncOptionsLockTarget)
                            if (!myTurn) System.Threading.Thread.Sleep(new TimeSpan(0, 0, 0, 0, 21));
                        }
                        TimeSpan myTotalWait = DateTime.Now.Subtract(myWait);
                        if ((options.LogThrottleDelays) && (myTotalWait.TotalMilliseconds > 50)) OnDiagTrace(string.Format("ce(single) [{0}] delayed {1}ms", Endpoint, myTotalWait));
                    }
                }

                HttpContent localCopyOfContent = null;
                if (content != null) localCopyOfContent = await Tools.CloneHttpContentAsync(content);
                lock (withClient) // not clear that this is required, but...
                {
                    switch (verb)
                    {
                        case HttpVerb.Get:
                            HttpRequestTask = withClient.GetAsync(URI, HttpCompletionOption.ResponseHeadersRead);
                            break;
                        case HttpVerb.Delete:
                            HttpRequestTask = withClient.DeleteAsync(URI);
                            break;
                        case HttpVerb.Post:
                            if (content == null) throw new ArgumentException("Post requests require content");

                            HttpRequestTask = withClient.PostAsync(URI, localCopyOfContent);
                            
                            break;
                        case HttpVerb.Patch:
                            if (content == null) throw new ArgumentException("Patch requests require content");
                            var request = new HttpRequestMessage(new HttpMethod("PATCH"), URI) { Content = localCopyOfContent };
                            HttpRequestTask = withClient.SendAsync(request);
                            break;
                        default:
                            throw new ApplicationException("Unsupported verb");
                    }
                }
                double msUsed;
                try
                {

                    response = await HttpRequestTask;
                }
                catch (Exception ex)
                {
                    msUsed = DateTime.Now.Subtract(startms).TotalMilliseconds;
                    string traceInfo = string.Format("<!> ce({0},{1}) s={3:F1}s; status={2}", verb, URIForLogging(URI), ex.Message, msUsed / 1000.0);
                    OnDiagTrace(traceInfo);
                    throw ex;
                }
                msUsed = DateTime.Now.Subtract(startms).TotalMilliseconds;
                lock (StaticLockObject) TotalRequestMS += msUsed;
                InstanceTotalRequestMS += msUsed;
                LastFailureInformation = "";
                if ((!response.IsSuccessStatusCode) || (DiagOutputLevel > TraceLevel.NonSuccess))
                {
                    string traceInfo = string.Format("ce({0},{1}) s={3:F1}s; status={2}", verb, URIForLogging(URI), response.StatusCode, msUsed / 1000.0);
                    if ((!response.IsSuccessStatusCode) && (response.Content.Headers.ContentLength > 0))
                    {
                        Newtonsoft.Json.Linq.JObject info = await response.Content.ReadAsAsync<Newtonsoft.Json.Linq.JObject>();
                        if (info != null)
                        {
                            Newtonsoft.Json.Linq.JToken msgtoken = info.GetValue("message");
                            Newtonsoft.Json.Linq.JToken providerMsgtoken = info.GetValue("providerMessage");
                            Newtonsoft.Json.Linq.JToken rqIDtoken = info.GetValue("requestId");
                            if (rqIDtoken != null) traceInfo = string.Concat(traceInfo, "; RequestId=", rqIDtoken.ToString());
                            if (msgtoken != null)
                            {
                                traceInfo = string.Concat(traceInfo, " - ", msgtoken.ToString());
                                LastFailureInformation = msgtoken.ToString();
                                if (providerMsgtoken != null)
                                {
                                    string providerMessage = providerMsgtoken.ToString();
                                    traceInfo = string.Concat(traceInfo, " - ", providerMessage);
                                    LastFailureInformation = string.Concat(traceInfo, " - ", providerMessage);
                                    if ( ((int)response.StatusCode == 429 ) || (ProviderMsgRemedyIsReIssue( providerMessage)))
                                    {
                                        if (EndpointSettings.ContainsKey(Endpoint))
                                        {
                                            DateTime lre = DateTime.Now;
                                            ThrottleRequestsPerSecond(2468);
                                            options = EndpointSettings[Endpoint];
                                            options.LastRateExceeded = lre;
                                            if ((options.MaxRqPerSecond <= 0) || (options.MaxRqPerSecond > options.HighwaterGeneratedRequestsPerSecond)) options.MaxRqPerSecond = (int)options.HighwaterGeneratedRequestsPerSecond;
                                            if ((options.MaxRqPerSecond > 2) && (DateTime.Now.Subtract(options.LastAutoLimit).TotalSeconds > 1))
                                            {
                                                options.MaxRqPerSecond--;
                                                options.LastAutoLimit = options.LastRateExceeded;
                                                OnDiagTrace(string.Format("ce(throughput) [{0}] rate limit exceeded: inferred new target of {1}r/s", Endpoint, options.MaxRqPerSecond));
                                            }
                                            if (DateTime.Now.Subtract(lre).TotalSeconds < 2.4) throw new ApplicationException("Rate throttle failed!");
                                            response = await APIExecuteVerb(withClient, verb, URI, content);
                                        }
                                    }
                                }
                                if (rqIDtoken != null) LastFailureInformation = string.Format("{0}; (Request #{1}, ID {2})", LastFailureInformation, RequestCounter, rqIDtoken.ToString());
                            }
                        }
                    }
                    OnDiagTrace(traceInfo);
                }
                response.EnsureSuccessStatusCode();
                return response;
            }
            finally
            {
                if (AsyncOptionsLockTarget != null) AsyncOptionsLockTarget.ClearHttpClientBusy();
            }
        }

        private bool ProviderMsgRemedyIsReIssue(string providerMessage)
        {
            bool reIssue = false ;
            reIssue = (providerMessage.IndexOf("rate limit exceeded", StringComparison.CurrentCultureIgnoreCase) > 0);
            reIssue = reIssue || (providerMessage.IndexOf("429", StringComparison.CurrentCultureIgnoreCase) > 0);
            reIssue = reIssue || (providerMessage.IndexOf("Please re-issue the request", StringComparison.CurrentCultureIgnoreCase) > 0);
            reIssue = reIssue || (providerMessage.IndexOf("too_many_", StringComparison.CurrentCultureIgnoreCase) > 0);
            reIssue = reIssue || (providerMessage.IndexOf("too many", StringComparison.CurrentCultureIgnoreCase) > 0);
            return reIssue;
        }

        string URIForLogging(string rawURI)
        {
            if (SimplifyLoggedURIs) rawURI = rawURI.Replace("%2F", "/");
            return rawURI;
        }

        HttpClient CloneAPIClient()
        {
            HttpClient client = NewHttpClient();
            client.DefaultRequestHeaders.Authorization = AuthorizationData.GetHeaderValue();
            return (client);
        }

        HttpClient NewHttpClient()
        {
            HttpClient client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip },false);
            lock (StaticLockObject) ++HttpClientInstanceCounter;
            client.BaseAddress = new Uri(ElementsPublicUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("deflate"));
            if ((EndpointOptions != null) && (EndpointOptions.HasExtraHeader))
            {
                EndpointOptions.GetExtraHeader(client);
            }
            return (client);
        }
        #endregion


    }

    
   

}
