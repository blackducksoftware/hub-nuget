using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Project;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using System;
using System.Collections.Generic;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Api.ResponseService
{
    public class ProjectVersionResponseService : HubResponseService
    {
        public ProjectVersionResponseService(RestConnection restConnection) : base(restConnection)
        {
        }

        public ProjectVersionView GetProjectVersion(ProjectView project, string projectVersionName)
        {
            string versionsUrl = MetadataResponseService.GetLink(project, ApiLinks.VERSIONS_LINK);
            HubPagedRequest request = new HubPagedRequest(RestConnection);
            request.QueryParameters[HubRequest.Q_QUERY] = String.Format("versionName:{0}", projectVersionName);
            request.SetUriFromString(versionsUrl);
            List<ProjectVersionView> allProjectVersionMatchingItems = GetAllItems<ProjectVersionView>(request);
            foreach(ProjectVersionView projectVersion in allProjectVersionMatchingItems)
            {
                if(projectVersionName.Equals(projectVersion.VersionName))
                {
                    return projectVersion;
                }
            }

            throw new BlackDuckIntegrationException(String.Format("Could not find the version: {0} for project: {1}", projectVersionName, project.Name));
        }
    }
}
