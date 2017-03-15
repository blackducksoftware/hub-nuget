using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.PolicyStatus;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Project;
using System;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices
{
    public class PolicyDataService : DataService
    {
        public PolicyDataService(RestConnection restConnection) : base(restConnection)
        {
        }

        public VersionBomPolicyStatusView GetVersionBomPolicyStatusView(ProjectVersionView projectVersionView)
        {
            string policyStatusUrl = MetadataDataService.GetLink(projectVersionView, ApiLinks.POLICY_STATUS_LINK);
            HubRequest request = new HubRequest(RestConnection);
            request.Uri = new Uri(policyStatusUrl, UriKind.Absolute);
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
