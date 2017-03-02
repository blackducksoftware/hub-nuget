using Com.Blackducksoftware.Integration.HubCommon.NET.Model;
using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.HubCommon.NET.Api
{
    public class HubView : HubResponse
    {
        [JsonProperty(PropertyName = "_meta")]
        public Metadata Metadata { get; set; }
    }
}
