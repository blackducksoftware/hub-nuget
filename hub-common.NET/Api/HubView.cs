using Com.Blackducksoftware.Integration.Hub.Common.Net.Model;
using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Api
{
    public class HubView : HubResponse
    {
        [JsonProperty(PropertyName = "_meta")]
        public Metadata Metadata { get; set; }
    }
}
