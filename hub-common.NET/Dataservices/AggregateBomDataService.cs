using System.Collections.Generic;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Report;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Project;
using System;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices
{
    public class AggregateBomDataService : DataService
    {

        private const int BUFFER_LIMIT = 5;

        public AggregateBomDataService(RestConnection restConnection) : base(restConnection)
        {
        }

        public List<VersionBomComponentView> GetBomEntries(ProjectVersionView projectVersionView)
        {
            int totalComponents = GetPagedBomEntries(projectVersionView, 0, 1).TotalCount;
            HubPagedResponse<VersionBomComponentView> bomEntries = new HubPagedResponse<VersionBomComponentView>();

            for (int i = 0; i < totalComponents;)
            {
                HubPagedResponse<VersionBomComponentView> response = GetPagedBomEntries(projectVersionView, i, BUFFER_LIMIT);
                int responseCount = response.Items.Count;
                bomEntries.Items.AddRange(response.Items);
                bomEntries.TotalCount += responseCount;
                i += response.Items.Count;
            }

            return bomEntries.Items;
        }

        public HubPagedResponse<VersionBomComponentView> GetPagedBomEntries(ProjectVersionView projectVersionView, int offset, int limit)
        {
            string componentsUrl = MetadataDataService.GetLink(projectVersionView, ApiLinks.COMPONENTS_LINK);
            HubRequest request = new HubRequest(RestConnection);
            request.QueryParameters[HubRequest.Q_OFFSET] = offset.ToString();
            request.QueryParameters[HubRequest.Q_LIMIT] = limit.ToString();
            request.SetUriFromString(componentsUrl);
            HubPagedResponse<VersionBomComponentView> response = request.ExecuteGetForResponsePaged<VersionBomComponentView>();
            return response;
        }
    }
}