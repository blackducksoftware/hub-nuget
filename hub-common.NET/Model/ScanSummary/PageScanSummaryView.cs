using Newtonsoft.Json;
using System.Collections.Generic;

namespace Com.Blackducksoftware.Integration.HubCommon.NET.Model.ScanSummary
{
    public class PageScanSummaryView : HubView
    {
        [JsonProperty(PropertyName = "totalCount")]
        public int TotalCount { get; set; }

        [JsonProperty(PropertyName = "items")]
        public List<ScanSummaryView> Items { get; set; }
    }
}
