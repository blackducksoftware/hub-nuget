using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model
{
    public class VersionBomPolicyStatusView : HubResponse
    {
        [JsonProperty(PropertyName = "componentVersionStatusCounts")]
        public List<NameValuePairView> ComponentVersionStatusCounts { get; set; }

        [JsonProperty(PropertyName = "overallStatus")]
        public string OverallStatus { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public string UpdatedAt { get; set; }
    }
}
