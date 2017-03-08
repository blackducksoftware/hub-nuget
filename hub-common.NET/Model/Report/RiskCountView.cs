using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Report
{
    public class RiskCountView : HubResponse
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        [JsonProperty(PropertyName = "countType")]
        [JsonConverter(typeof(HubEnumConverter<RiskCountEnum>))]
        public RiskCountEnum CountType { get; set; }
    }
}
