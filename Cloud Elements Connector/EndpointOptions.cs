using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Cloud_Elements_API
{
    /// <summary>
    /// Connector Options that apply to a specific Endpoint Provider (BOX, Google Drive, OneDrive, etc)
    /// </summary>
    public class EndpointOptions
    {
        /// <summary>
        /// The maximum requests per second.  The engine will introduce delays if request rate exceeds this.
        /// </summary>
        public int MaxRqPerSecond;
        /// <summary>
        /// The number of seconds over which the average is calculated.  Too high a number will allow bursts that exceed the providers limit
        /// </summary>
        public int RequestsPerSecondWindow;
        /// <summary>
        /// Set by the engine when a new high for requests per second is reached.  The value may be higher than expected due to sub-second bursts. 
        /// </summary>
        public double HighwaterGeneratedRequestsPerSecond
        {
            get { return _HighwaterGeneratedRequestsPerSecond; }
            internal set { _HighwaterGeneratedRequestsPerSecond = value; }
        }
        public DateTime BackoffUntil;          // future:  
        /// <summary>
        /// Set to the time at which the connector last detected a "rate exceeded" condition and adjust MaxRqPerSecond
        /// </summary>
        public DateTime LastAutoLimit
        {
            get { return _LastAutoLimit; }
            internal set { _LastAutoLimit = value; }
        }
        public DateTime LastRateExceeded
        {
            get { return _LastRateExceeded; }
            internal set { _LastRateExceeded = value; }
        }

        public bool HasFileHashAlgorithm
        {
            get { return ((_FileHashCyproName != null) && (_FileHashCyproName.Length > 0)); }

        }

        public bool HasModifiedBy
        {
            get { return ((_ModifiedByRawPath != null) && (_ModifiedByRawPath.Length > 0)); }

        }

        public string EndpointType
        {
            get { return _EndpointType; }
            internal set { _EndpointType = value; }
        }

        public string FileHashAlgorithmName
        {
            get { return _FileHashCyproName; }
            internal set { _FileHashCyproName = value; }
        }

        protected internal string FileHashRawIDPath
        {
            get { return _FileHashRawIDPath; }
            set { _FileHashRawIDPath = value; }
        }

        protected internal string ModifiedByRawIDPath
        {
            get { return _ModifiedByRawPath; }
            set { _ModifiedByRawPath = value; }
        }

        public void GetExtraHeader(HttpClient client)
        {
            if (_ExtraHeaderName == null) return;
            client.DefaultRequestHeaders.Add(_ExtraHeaderName, _ExtraHeaderValue);
        }

        public bool HasExtraHeader
        {
            get
            {
                bool result = false;
                if (_ExtraHeaderName != null) result = true;
                return result;
            }
        }

        internal void SetExtraHeaderID(string ID)
        {
            _ExtraHeaderName = ID;
        }
        public void SetExtraHeaderValue(string value)
        {
            if (_ExtraHeaderName == null) throw new ApplicationException("This endpoint does not use an extra header!");
            // _ExtraHeaderNVP.Value = value;
            //_ExtraHeaderNVP = new System.Net.Http.Headers.NameValueHeaderValue(_ExtraHeaderNVP.Name, value);  
            _ExtraHeaderValue = value;
        }

        /// <summary>
        /// Call when locked
        /// </summary>
        /// <returns>True if the semiphore was off and has been set on</returns>
        public bool SetHttpClientBusy()
        {
            bool result;
            result = !_HttpClientBusySemiphore;
            if (result)
            {
                _HttpClientBusyThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
                _HttpClientBusySemiphore = true;
            }
            else if (_HttpClientBusyThreadID == System.Threading.Thread.CurrentThread.ManagedThreadId)
            {
                result = true;
            }
            return (result);
        }
        public void ClearHttpClientBusy()
        {
            _HttpClientBusySemiphore = false;
        }

        public bool SupportsAsync = true;
        public bool SupportsCopy = true;
        public bool SupportsGetStorage = true;
        public bool SupportsTags = true;
        public bool LogThrottleDelays;
        public bool LogHighwaterThroughput;
        private bool _HttpClientBusySemiphore;
        private int _HttpClientBusyThreadID;

        private double _HighwaterGeneratedRequestsPerSecond;
        private DateTime _LastAutoLimit;
        private DateTime _LastRateExceeded;
        private string _FileHashCyproName;
        private string _ModifiedByRawPath;
        private string _FileHashRawIDPath;
        private string _EndpointType;
        //private System.Net.Http.Headers.NameValueHeaderValue _ExtraHeaderNVP;
        private string _ExtraHeaderName;
        private string _ExtraHeaderValue;



    }

}
