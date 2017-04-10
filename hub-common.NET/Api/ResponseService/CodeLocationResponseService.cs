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
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.CodeLocation;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using System.Collections.Generic;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Api.ResponseService
{
    public class CodeLocationResponseService : HubResponseService
    {
        public CodeLocationResponseService(RestConnection restConnection) : base(restConnection)
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
