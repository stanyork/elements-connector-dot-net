using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Elements_API
{
    public class CloudAuthorization
    {
        private string Element;
        private string User;
        private string EndpointValue;
        public CloudAuthorization(string elementToken, string userToken)
        {
            Element = elementToken;
            User = userToken;

        }

        public CloudAuthorization(string jsonData)
        {
            Dictionary<string, object> deserializedAuthorization = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);
            if (deserializedAuthorization.ContainsKey("element")) Element = (string)deserializedAuthorization["element"];
            if (deserializedAuthorization.ContainsKey("user")) User = (string)deserializedAuthorization["user"];
            if (deserializedAuthorization.ContainsKey("endpointValue")) EndpointValue = (string)deserializedAuthorization["endpointValue"];
            if (deserializedAuthorization.ContainsKey("apiUrl")) ApiUrl = (string)deserializedAuthorization["apiUrl"];
        }

        public override string ToString()
        {
            return string.Format("Element {0}, User {1}", Element, User);
        }

        public   string ToJSonString(string extra)
        {
            Dictionary<string, object> info = new Dictionary<string, object>();
            info.Add("element", this.Element);
            info.Add("user", this.User);
            info.Add("apiUrl", this.ApiUrl);
            if (EndpointValue != null)  info.Add("endpointValue", this.EndpointValue);
            info.Add("_extra", extra);
            return Tools.ToJson(info);
        }

        public string ExtraValue { get { return EndpointValue; } set { EndpointValue = value; } }
        private string apiUrl;
        public string ApiUrl { get { return apiUrl; } set { apiUrl = value; } }


        public System.Net.Http.Headers.AuthenticationHeaderValue GetHeaderValue()
        {
            return new System.Net.Http.Headers.AuthenticationHeaderValue("Element",string.Format("{0}, User {1}", Element, User));
        }


    }
}
