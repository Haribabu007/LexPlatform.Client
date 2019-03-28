using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace LexPlatform.Client.Models
{
    public abstract class RequestBase
    {
        public abstract Dictionary<string, string> BuildProperties();

        public abstract string Url();

        public HttpMethod Method { get; set; }

        public TimeStamp Timestamp { get; set; }

        protected RequestBase()
        {
            Method = HttpMethod.Get;
        }

        public string BuildPayload(Dictionary<string, string> properties)
        {
            var payload = BuildQueryStringFromParams(properties);
            return string.Format("{0}|{1}|{2}", Method.ToString().ToUpper(), Url(), payload);
        }

        public virtual string BuildQueryStringFromParams(Dictionary<string, string> queryParams)
        {
            var arr = queryParams.OrderBy(pair => pair.Key).Select(kv => string.Format("{0}={1}", kv.Key, Uri.EscapeDataString(kv.Value))).ToArray();
            return string.Join("&", arr);
        }
    }
}
