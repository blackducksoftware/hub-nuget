using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.PolicyStatus;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Project;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Api.ResponseService
{
    public class PolicyResponseService : HubResponseService
    {
        public PolicyResponseService(RestConnection restConnection) : base(restConnection)
        {
        }

        public VersionBomPolicyStatusView GetVersionBomPolicyStatusView(ProjectVersionView projectVersionView)
        {
            string policyStatusUrl = MetadataResponseService.GetLink(projectVersionView, ApiLinks.POLICY_STATUS_LINK);
            HubRequest request = new HubRequest(RestConnection);
            request.SetUriFromString(policyStatusUrl);
            VersionBomPolicyStatusView response = request.ExecuteGetForResponse<VersionBomPolicyStatusView>();
            return response;
        }

        public PolicyStatus GetPolicyStatus(ProjectVersionView projectVersionView)
        {
            VersionBomPolicyStatusView policyView = GetVersionBomPolicyStatusView(projectVersionView);
            PolicyStatus policyStatus = new PolicyStatus(policyView);
            return policyStatus;
        }
    }
}
