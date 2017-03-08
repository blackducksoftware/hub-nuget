using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.PolicyStatus;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices
{
    public class PolicyDataService : DataService
    {
        public PolicyDataService(RestConnection restConnection) : base(restConnection)
        {
        }

        public VersionBomPolicyStatusView GetVersionBomPolicyStatusView(string projectId, string versionId)
        {
            HubRequest request = new HubRequest(RestConnection);
            request.Path = $"api/{ApiLinks.PROJECTS_LINK}/{projectId}/{ApiLinks.VERSIONS_LINK}/{versionId}/{ApiLinks.POLICY_STATUS_LINK}";
            VersionBomPolicyStatusView response = request.ExecuteGetForResponse<VersionBomPolicyStatusView>();
            return response;
        }

        public PolicyStatus GetPolicyStatus(string projectId, string versionId)
        {
            VersionBomPolicyStatusView policyView = GetVersionBomPolicyStatusView(projectId, versionId);
            PolicyStatus policyStatus = new PolicyStatus(policyView);
            return policyStatus;
        }
    }
}
