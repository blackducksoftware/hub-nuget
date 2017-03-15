using System.Collections.Generic;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Project;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;
using System;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices
{
    public class ProjectDataService : DataService
    {
        public ProjectDataService(RestConnection restConnection) : base(restConnection)
        {
        }

        public HubPagedResponse<ProjectView> GetPagedProjectView(string projectName)
        {
            HubRequest hubRequest = new HubRequest(RestConnection);
            hubRequest.Path = $"api/{ApiLinks.PROJECTS_LINK}";
            hubRequest.QueryParameters.Add(HubRequest.Q_QUERY, $"name:{projectName}");
            HubPagedResponse<ProjectView> response = hubRequest.ExecuteGetForResponsePaged<ProjectView>();
            return response;
        }

        public HubPagedResponse<ProjectVersionView> GetPagedProjectVersionView(ProjectView projectView)
        {
            string projectVersionsUrl = MetadataDataService.GetLink(projectView, ApiLinks.VERSIONS_LINK);
            HubRequest hubRequest = new HubRequest(RestConnection);
            hubRequest.QueryParameters.Add(HubRequest.Q_SORT, "updatedAt asc"); // Sort it by most recent
            hubRequest.Uri = new Uri(projectVersionsUrl, UriKind.Absolute);
            HubPagedResponse<ProjectVersionView> response = hubRequest.ExecuteGetForResponsePaged<ProjectVersionView>();
            return response;
        }

        public ProjectView GetProjectView(string projectName)
        {
            List<ProjectView> projectViews = GetPagedProjectView(projectName).Items;
            ProjectView first = null;
            if (projectViews != null && projectViews.Count > 0)
            {
                first = projectViews[0];
            }
            return first;
        }


        public ProjectVersionView GetMostRecentVersion(ProjectView projectView)
        {
            List<ProjectVersionView> versions = GetPagedProjectVersionView(projectView).Items;
            ProjectVersionView recent = null;
            if (versions != null && versions.Count > 0)
            {
                recent = versions[0]; // Assuming sorted by updatedAt asc
            }
            return recent;
        }
    }
}
