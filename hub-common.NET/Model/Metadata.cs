using Com.Blackducksoftware.Integration.Hub.Common.Net.Global;
using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model
{
    public class Metadata : UrlHelper
    {
        [JsonProperty(PropertyName = "allow")]
        public string[] AllowedRequestTypes { get; set; }

        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }

        [JsonProperty(PropertyName = "links")]
        public Link[] Links { get; set; }
    }
}
