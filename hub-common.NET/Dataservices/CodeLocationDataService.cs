using System.Collections.Generic;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.CodeLocation;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices
{
    public class CodeLocationDataService : DataService
    {
        public CodeLocationDataService(RestConnection restConnection) : base(restConnection)
        {

        }

        public List<CodeLocationView> FetchCodeLocations(string q, int limit)
        {
            HubRequest request = new HubRequest(RestConnection);
            request.QueryParameters[HubRequest.Q_LIMIT] = limit.ToString();
            request.QueryParameters[HubRequest.Q_QUERY] = q;
            request.Path = $"api/{ApiLinks.CODE_LOCATION_LINK}";
            HubPagedResponse<CodeLocationView> response = request.ExecuteGetForResponsePaged<CodeLocationView>();
            List<CodeLocationView> codeLocations = response.Items;
            return codeLocations;
        }

        public CodeLocationView GetCodeLocationView(string bdioId)
        {
            string q = $"url:{bdioId}";
            List<CodeLocationView> codeLocations = FetchCodeLocations(q, 1);
            if (codeLocations != null && codeLocations.Count > 0)
            {
                return codeLocations[0];
            }
            else
            {
                return null;
            }
        }

        public List<CodeLocationView> GetAllCodeLocationsForCodeLocationType(CodeLocationTypeEnum codeLocationType)
        {
            HubPagedRequest request = new HubPagedRequest(RestConnection);
            request.QueryParameters[HubRequest.Q_CODE_LOCATION_TYPE] = codeLocationType.ToString();
            request.Path = $"api/{ApiLinks.CODE_LOCATION_LINK}";
            List<CodeLocationView> allLocations = GetAllItems<CodeLocationView>(request);
            return allLocations;
        }
    }
}
