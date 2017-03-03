using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices
{
    public class CodeLocationDataService : DataService
    {
        public CodeLocationDataService(RestConnection restConnection) : base(restConnection)
        {

        }

        public List<CodeLocationView> FetchCodeLocations()
        {
            HubRequest request = new HubRequest(RestConnection);
            request.QueryParameters.Add(HubRequest.Q_LIMIT, "1");
            request.UrlSegments.Add($"api/codelocations");
            HubPagedResponse<CodeLocationView> response = request.ExecuteGetForResponsePaged<CodeLocationView>();
            List<CodeLocationView> codeLocations = response.Items;
            return codeLocations;
        }
    }
}
