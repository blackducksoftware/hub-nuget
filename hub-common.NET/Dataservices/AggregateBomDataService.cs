using System.Collections.Generic;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Report;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices
{
    public class AggregateBomDataService : DataService
    {
        public AggregateBomDataService(RestConnection restConnection) : base(restConnection)
        {

        }

        public List<VersionBomComponentView> GetBomEntries(Project projectItem)
        {
            return GetBomEntries(projectItem.ProjectId, projectItem.VersionId);
        }

        public List<VersionBomComponentView> GetBomEntries(string projectId, string versionId)
        {
            HubPagedResponse<VersionBomComponentView> response = GetPagedBomEntries(projectId, versionId);
            return response.Items;
        }

        public HubPagedResponse<VersionBomComponentView> GetPagedBomEntries(string projectId, string versionId)
        {
            HubRequest request = new HubRequest(RestConnection);
            request.Path = $"/api/projects/{projectId}/versions/{versionId}/components";
            HubPagedResponse<VersionBomComponentView> response = request.ExecuteGetForResponsePaged<VersionBomComponentView>();
            return response;
        }
    }
}