using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.PolicyStatus
{
    public class VersionBomPolicyStatusView : HubResponse
    { 
        [JsonProperty(PropertyName = "componentVersionStatusCounts")]
        public List<ViolationCountView> ComponentVersionStatusCounts { get; set; }

        [JsonProperty(PropertyName = "overallStatus")]
        [JsonConverter(typeof(HubEnumConverter<PolicyStatusEnum>))]
        public PolicyStatusEnum OverallStatus { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public string UpdatedAt { get; set; }
    }
}
