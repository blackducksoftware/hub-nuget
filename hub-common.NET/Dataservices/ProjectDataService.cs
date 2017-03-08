using System.Collections.Generic;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Project;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;

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

        public HubPagedResponse<ProjectVersionView> GetPagedProjectVersionView(string projectId)
        {
            HubRequest hubRequest = new HubRequest(RestConnection);
            hubRequest.QueryParameters.Add(HubRequest.Q_SORT, "updatedAt asc"); // Sort it by most recent
            hubRequest.Path = $"api/{ApiLinks.PROJECTS_LINK}/{projectId}/{ApiLinks.VERSIONS_LINK}";
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


        public ProjectVersionView GetMostRecentVersion(string projectId)
        {
            List<ProjectVersionView> versions = GetPagedProjectVersionView(projectId).Items;
            ProjectVersionView recent = null;
            if (versions != null && versions.Count > 0)
            {
                recent = versions[0]; // Assuming sorted by updatedAt asc
            }
            return recent;
        }

        public Project GetMostRecentProjectItem(string projectName)
        {
            Project projectItem = new Project();
            ProjectView projectView = GetProjectView(projectName);
            projectItem.ProjectView = projectView; // Sets the project Id
            ProjectVersionView versionView = GetMostRecentVersion(projectItem.ProjectId);
            projectItem.ProjectVersionView = versionView; // Sets the version Id
            return projectItem;
        }
    }
}
