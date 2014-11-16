using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Elements_API
{
    public class CloudAuthorization
    {
        private string element;
        private string user;
        public CloudAuthorization(string elementToken, string userToken)
        {
            element = elementToken;
            user = userToken;

        }

        public CloudAuthorization(string jsonData)
        {
            Dictionary<string, object> deserializedAuthorization = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);
            if (deserializedAuthorization.ContainsKey("element")) element = (string)deserializedAuthorization["element"];
            if (deserializedAuthorization.ContainsKey("user")) user = (string)deserializedAuthorization["user"];

        }

        public override string ToString()
        {
            return string.Format("Element {0}, User {1}", element, user);
        }

        public   string ToJSonString(string extra)
        {
            Dictionary<string, object> info = new Dictionary<string, object>();
            info.Add("element", this.element);
            info.Add("user", this.user);
            info.Add("_extra", extra);
            return Tools.ToJson(info);
        }



        public System.Net.Http.Headers.AuthenticationHeaderValue GetHeaderValue()
        {
            return new System.Net.Http.Headers.AuthenticationHeaderValue("Element",string.Format("{0}, User {1}", element, user));
        }


    }
}
