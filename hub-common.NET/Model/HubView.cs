using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.HubCommon.NET.Model
{
    public class HubView
    {
        [JsonProperty(PropertyName = "_meta")]
        public Metadata Metadata { get; set; }
    }
}
