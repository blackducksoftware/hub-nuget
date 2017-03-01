using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Nuget.Model
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
