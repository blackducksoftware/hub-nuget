using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Constants;
using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Report
{
    class BomComponentPolicyStatusView : HubView
    {
        [JsonProperty(PropertyName = "approvalStatus")]
        public VersionBomPolicyStatusOverallStatusEnum ApprovalStatus { get; set; }
    }
}
