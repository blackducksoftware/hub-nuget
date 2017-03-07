using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model
{
    public class ViolationCountView : HubResponse
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "value")]
        public int Count { get; set; }
    }
}
