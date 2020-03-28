using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Cloud_Elements_API
{
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

    public class CloudStorage
    {
        public Int64 shared; //  optional),
        public Int64 total; //  optional),
        public Int64 used; //  optional),
    }

    public class FileContent : IDisposable
    {
        public readonly long ContentLength;
        public readonly string Disposition;
        public System.IO.Stream ContentStream;
        private HttpClient ViaClient;
        public FileContent(HttpResponseMessage response, HttpClient viaClient)
        {
            ContentLength = 0;
            if (response.Content.Headers.ContentLength != null) ContentLength = (long)response.Content.Headers.ContentLength;
            Disposition = "";
            ViaClient = viaClient;
            if (response.Content.Headers.ContentDisposition != null) Disposition = (string)response.Content.Headers.ContentDisposition.FileName;
        }
        public override string ToString()
        {
            return string.Format("{0}", Disposition);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (ViaClient != null)
                {
                    ViaClient.Dispose();
                    ViaClient = null;
                }
            }
            // free native resources if there are any.
        }

    }
}
