using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Nuget.Model
{
    public class HubView
    {
        [JsonProperty(PropertyName = "_meta")]
        public Metadata Metadata { get; set; }
    }
}
