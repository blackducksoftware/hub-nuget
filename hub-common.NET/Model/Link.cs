using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.HubCommon.NET.Model
{
    public class Link
    {
        [JsonProperty(PropertyName = "rel")]
        public string Rel { get; set; }

        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
    }
}
