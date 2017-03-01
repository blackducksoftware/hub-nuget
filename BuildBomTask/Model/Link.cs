using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Nuget.Model
{
    public class Link
    {
        [JsonProperty(PropertyName = "rel")]
        public string Rel { get; set; }

        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
    }
}
