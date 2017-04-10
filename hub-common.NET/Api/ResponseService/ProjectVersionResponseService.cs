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
