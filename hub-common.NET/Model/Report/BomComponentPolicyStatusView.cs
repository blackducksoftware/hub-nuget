using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Report
{
    class BomComponentPolicyStatusView : HubView
    {
        [JsonProperty(PropertyName = "approvalStatus")]
        [JsonConverter(typeof(HubEnumConverter<VersionBomPolicyStatusOverallStatusEnum>))]
        public VersionBomPolicyStatusOverallStatusEnum ApprovalStatus { get; set; } = VersionBomPolicyStatusOverallStatusEnum.NOT_IN_VIOLATION;
    }
}
