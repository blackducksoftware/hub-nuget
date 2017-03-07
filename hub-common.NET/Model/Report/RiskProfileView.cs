using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Report
{
    public class RiskProfileView : HubResponse
    {
        [JsonProperty(PropertyName = "counts")]
        public List<RiskCountView> Counts { get; set; }
    }
}