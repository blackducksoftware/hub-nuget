using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.ScanStatus
{
    public class ScanSummaryView : HubView
    {
        [JsonProperty(PropertyName = "status")]
        [JsonConverter(typeof(HubEnumConverter<ScanStatusEnum>))]
        public ScanStatusEnum Status { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public string UpdatedAt { get; set; }

    }
}
