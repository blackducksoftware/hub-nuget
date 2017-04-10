/*******************************************************************************
 * Copyright (C) 2017 Black Duck Software, Inc.
 * http://www.blackducksoftware.com/
 *
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements. See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership. The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied. See the License for the
 * specific language governing permissions and limitations
 * under the License.
 *******************************************************************************/
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Project;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using System.Collections.Generic;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Api.ResponseService
{
    public class ProjectResponseService : HubResponseService
    {
        public ProjectResponseService(RestConnection restConnection) : base(restConnection)
        {
        }

        public HubPagedResponse<ProjectView> GetPagedProjectView(string projectName)
        {
            HubRequest hubRequest = new HubRequest(RestConnection);
            hubRequest.Path = $"api/{ApiLinks.PROJECTS_LINK}";
            hubRequest.QueryParameters[HubRequest.Q_QUERY] = $"name:{projectName}";
            HubPagedResponse<ProjectView> response = hubRequest.ExecuteGetForResponsePaged<ProjectView>();
            return response;
        }

        public HubPagedResponse<ProjectVersionView> GetPagedProjectVersionView(ProjectView projectView)
        {
            string projectVersionsUrl = MetadataResponseService.GetLink(projectView, ApiLinks.VERSIONS_LINK);
            HubRequest hubRequest = new HubRequest(RestConnection);
            hubRequest.QueryParameters[HubRequest.Q_SORT] = "updatedAt asc"; // Sort it by most recent
            hubRequest.SetUriFromString(projectVersionsUrl);
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
