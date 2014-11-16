using System;
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
        static readonly string DefaultElementsPublicURL = "https://api.cloud-elements.com/elements/api-v2/";
        private static int ConnectorInstanceCounter = 0;
        private static int HttpClientInstanceCounter = 0;
        private static int RequestCounter = 0;
        private static double TotalRequestMS = 0d;
        // ...hubs/documents/folders/contents?path=%2FSQL&fetchTags=true"
        // ...hubs/documents/files/21794645297/metadata
        private Cloud_Elements_API.CloudAuthorization AuthorizationData;
        private HttpClient APIClient;
        #endregion

        public string ElementsPublicUrl { get; set; }
        public Cloud_Elements_API.CloudAuthorization APIAuthorization
        {
            set
            {
                AuthorizationData = value;
                APIClient.DefaultRequestHeaders.Authorization = AuthorizationData.GetHeaderValue();
            }
        }


        #region "Constructors"
        public CloudElementsConnector(string elementsURL)
        {
            if (elementsURL != null) ElementsPublicUrl = DefaultElementsPublicURL;
        }


        public CloudElementsConnector()
        {
            ElementsPublicUrl = DefaultElementsPublicURL;
            APIClient = NewHttpClient();
            ++ConnectorInstanceCounter;
        }
        #endregion

        public async Task<Pong> Ping()
        {
            HttpResponseMessage response = await APIExecuteGet("hubs/documents/ping");
            Pong ResultPong = await response.Content.ReadAsAsync<Pong>();
            return ResultPong;
        }

        #region "documents/folders/..."
        public async Task<List<CloudFile>> ListFolderContents(string path, Boolean withTags)
        {
            HttpResponseMessage response = await APIExecuteGet(string.Format("hubs/documents/folders/contents?path={0}&fetchTags={1}", System.Net.WebUtility.UrlEncode(path), withTags));
            List<CloudFile> ResultList = await response.Content.ReadAsAsync<List<CloudFile>>();
            return ResultList;
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

            if (!newFolder.path.StartsWith("/")) throw new ArgumentException("Path must begin with a slash");

            var content = CloudFileRequestContent(newFolder);
            HttpResponseMessage response = await APIExecutePost(URL, content);
            CloudFile Result = await response.Content.ReadAsAsync<CloudFile>();
            return Result;

        }

        public async Task<CloudFile> DeleteFolder(string path, Boolean withTrash)
        {
            HttpResponseMessage response = await APIExecuteDelete(string.Format("hubs/documents/folders?path={0}&emptyTrash={1}", System.Net.WebUtility.UrlEncode(path), withTrash));
            CloudFile ResultList = await response.Content.ReadAsAsync<CloudFile>();
            return ResultList;
        }

        #endregion

        #region "documents/files/..."
        public async Task<CloudLink> FileLinks(string id)
        {
            HttpResponseMessage response = await APIExecuteGet(string.Format("hubs/documents/files/{0}/links", System.Net.WebUtility.UrlEncode(id)));
            CloudLink Result = await response.Content.ReadAsAsync<CloudLink>();
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
            CloudFile Result;
            HttpResponseMessage response;
            string RequestURL;
            switch (fileSpecType)
            {
                case FileSpecificationType.ID:
                    RequestURL = string.Format("hubs/documents/files/{0}/metadata", System.Net.WebUtility.UrlEncode(identifier));
                    break;
                case FileSpecificationType.Path:
                    RequestURL = string.Format("hubs/documents/files/metadata?path={0}", System.Net.WebUtility.UrlEncode(identifier));
                    break;
                default:
                    throw new ArgumentException("unsupported File Specification Type - " + fileSpecType.ToString());
            }
            response = await APIExecuteGet(RequestURL);
            Result = await response.Content.ReadAsAsync<CloudFile>();
            return Result;
        }

        /// <summary>
        /// Update a file's metadata associated with an ID or PATH
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
            string RequestURL;
            switch (fileSpecType)
            {
                case FileSpecificationType.ID:
                    RequestURL = string.Format("hubs/documents/files/{0}/metadata", System.Net.WebUtility.UrlEncode(identifier));
                    break;
                case FileSpecificationType.Path:
                    RequestURL = string.Format("hubs/documents/files/metadata?path={0}", System.Net.WebUtility.UrlEncode(identifier));
                    break;
                default:
                    throw new ArgumentException("unsupported File Specification Type - " + fileSpecType.ToString());
            }

            var content = CloudFileRequestContent(fileData);
            response = await APIExecutePatch(RequestURL, content);
            fileData = await response.Content.ReadAsAsync<CloudFile>();
            return fileData;
        }

        public async Task<FileContent> GetFile(string id)
        {
            HttpResponseMessage response = await APIExecuteGet(string.Format("hubs/documents/files/{0}", System.Net.WebUtility.UrlEncode(id)));
            FileContent Result = new FileContent(response);
            Result.ContentStream = await response.Content.ReadAsStreamAsync();
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
                                        , System.Net.WebUtility.UrlEncode(path)
                                        , System.Net.WebUtility.UrlEncode(description)
                                        );
            // neither is RFC3986 compliant
            // System.Net.WebUtility.UrlEncode(path); or System.Uri.EscapeDataString(path); ??? see http://blog.nerdbank.net/2009/05/uriescapedatapath-and.html
            if (overwrite) URL += "&overwrite=true";
            if (sizeInBytes > 0) URL += "&size=" + sizeInBytes.ToString();
            if ((tags != null) && (tags.Length > 0))
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

            // ref http://stackoverflow.com/questions/16416601/c-sharp-httpclient-4-5-multipart-form-data-upload

            var content = new MultipartFormDataContent(String.Format("----------{0:N}", Guid.NewGuid()));
            var FileContentSection = new StreamContent(uploadSource, 16384);
            content.Add(FileContentSection);
            FileContentSection.Headers.ContentDisposition.Name = "file";
            FileContentSection.Headers.ContentDisposition.FileName = System.IO.Path.GetFileName(path);
            FileContentSection.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);


            HttpResponseMessage response = await APIExecutePost(URL, content);
            CloudFile Result = await response.Content.ReadAsAsync<CloudFile>();
            return Result;

        }




        #endregion

        // ----------------------------

        #region "Request Support"

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

        async Task<HttpResponseMessage> APIExecuteDelete(string URI)
        {
            return await APIExecuteVerb(HttpVerb.Delete, URI, null);
        }

        async Task<HttpResponseMessage> APIExecutePost(string URI, HttpContent content)
        {
            return await APIExecuteVerb(HttpVerb.Post, URI, content);
        }

        async Task<HttpResponseMessage> APIExecutePatch(string URI, HttpContent content)
        {
            return await APIExecuteVerb(HttpVerb.Patch, URI, content);
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

        async Task<HttpResponseMessage> APIExecuteVerb(HttpVerb verb, string URI)
        {
            return await APIExecuteVerb(verb, URI, null);
        }

        async Task<HttpResponseMessage> APIExecuteVerb(HttpVerb verb, string URI, HttpContent content)
        {
            DateTime startms = DateTime.Now;
            RequestCounter++;
            HttpResponseMessage response;
            Task<HttpResponseMessage> HttpRequestTask;
            switch (verb)
            {
                case HttpVerb.Get:
                    HttpRequestTask = APIClient.GetAsync(URI, HttpCompletionOption.ResponseHeadersRead);
                    break;
                case HttpVerb.Delete:
                    HttpRequestTask = APIClient.DeleteAsync(URI);
                    break;
                case HttpVerb.Post:
                    if (content == null) throw new ArgumentException("Post requests require content");
                    HttpRequestTask = APIClient.PostAsync(URI, content);
                    break;
                case HttpVerb.Patch:
                    if (content == null) throw new ArgumentException("Patch requests require content");
                    var request = new HttpRequestMessage(new HttpMethod("PATCH"), URI) { Content = content };
                    HttpRequestTask = APIClient.SendAsync(request);
                    break;
                default:
                    throw new ApplicationException("Unsupported verb");
            }

            response = await HttpRequestTask;
            TotalRequestMS += DateTime.Now.Subtract(startms).TotalMilliseconds;
            response.EnsureSuccessStatusCode();
            return response;
        }

        HttpClient NewHttpClient()
        {
            HttpClient client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip });
            ++HttpClientInstanceCounter;
            client.BaseAddress = new Uri(ElementsPublicUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("deflate"));

            return (client);
        }
        #endregion





    }

    public class Pong
    {
        public string dateTime;
        public string endpoint;
        public override string ToString()
        {
            return string.Format("Pong[{0},{1}]", dateTime, endpoint);
        }
    }

    public class CloudLink
    {
        public string expires;  //  optional
        public string cloudElementsLink; //  optional
        public string providerViewLink; //  optional
        public string providerLink; //  optional
    }

    public class FileContent
    {
        public readonly long ContentLength;
        public readonly string Disposition;
        public System.IO.Stream ContentStream;
        public FileContent(HttpResponseMessage response)
        {
            ContentLength = 0;
            if (response.Content.Headers.ContentLength != null) ContentLength = (long)response.Content.Headers.ContentLength;
            Disposition = "";
            if (response.Content.Headers.ContentDisposition != null) Disposition = (string)response.Content.Headers.ContentDisposition.FileName;
        }
    }

}
