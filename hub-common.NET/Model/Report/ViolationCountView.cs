using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model
{
    public class ViolationCountView : HubResponse
    {
        [JsonProperty(PropertyName = "name")]
        [JsonConverter(typeof(HubEnumConverter<PolicyStatusEnum>))]
        public PolicyStatusEnum Name { get; set; }

        [JsonProperty(PropertyName = "value")]
        public int Count { get; set; }
    }
}
