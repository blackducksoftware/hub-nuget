using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.HubCommon.NET.Model.ScanSummary
{
    public class ScanSummaryView : HubView
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public string UpdatedAt { get; set; }

    }
}
