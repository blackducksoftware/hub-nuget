using System;
using System.Collections.Generic;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.CodeLocation;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;

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
            request.QueryParameters.Add(HubRequest.Q_LIMIT, limit.ToString());
            request.QueryParameters.Add(HubRequest.Q_QUERY, q);
            request.Path = $"api/{ApiLinks.CODE_LOCATION_LINK}";
            Console.WriteLine(request.BuildUri().ToString());
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

        public string GetCodeLocationId(CodeLocationView codeLocation)
        {
            return codeLocation.Metadata.GetFirstId(codeLocation.Metadata.Href);
        }
    }
}
