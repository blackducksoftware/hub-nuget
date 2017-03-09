using System.Collections.Generic;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Report;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices
{
    public class AggregateBomDataService : DataService
    {

        private const int BUFFER_LIMIT = 5;

        public AggregateBomDataService(RestConnection restConnection) : base(restConnection)
        {

        }

        public List<VersionBomComponentView> GetBomEntries(Project projectItem)
        {
            return GetBomEntries(projectItem.ProjectId, projectItem.VersionId);
        }

        public List<VersionBomComponentView> GetBomEntries(string projectId, string versionId)
        {
            int totalComponents = GetPagedBomEntries(projectId, versionId, 0, 1).TotalCount;
            HubPagedResponse<VersionBomComponentView> bomEntries = new HubPagedResponse<VersionBomComponentView>();

            for (int i = 0; i < totalComponents;)
            {
                HubPagedResponse<VersionBomComponentView> response = GetPagedBomEntries(projectId, versionId, i, BUFFER_LIMIT);
                int responseCount = response.Items.Count;
                bomEntries.Items.AddRange(response.Items);
                bomEntries.TotalCount += responseCount;
                i += response.Items.Count;
            }

            return bomEntries.Items;
        }

        public HubPagedResponse<VersionBomComponentView> GetPagedBomEntries(string projectId, string versionId, int offset, int limit)
        {
            HubRequest request = new HubRequest(RestConnection);
            request.QueryParameters.Add(HubRequest.Q_OFFSET, offset.ToString());
            request.QueryParameters.Add(HubRequest.Q_LIMIT, limit.ToString());
            request.Path = $"/api/{ApiLinks.PROJECTS_LINK}/{projectId}/{ApiLinks.VERSIONS_LINK}/{versionId}/{ApiLinks.COMPONENTS_LINK}";
            HubPagedResponse<VersionBomComponentView> response = request.ExecuteGetForResponsePaged<VersionBomComponentView>();
            return response;
        }
    }
}