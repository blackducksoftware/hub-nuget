using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices
{
    public class PolicyDataService : DataService
    {

        public PolicyDataService(RestConnection restConnection) : base(restConnection)
        {
        }

        public VersionBomPolicyStatusView GetPolicyStatus(string projectId, string versionId)
        {
            HubRequest request = new HubRequest(RestConnection);
            request.Path = $"api/projects/{projectId}/versions/{versionId}/policy-status";
            VersionBomPolicyStatusView response = request.ExecuteGetForResponse<VersionBomPolicyStatusView>();
            return response;
        }
    }
}
