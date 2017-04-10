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
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.ScanStatus;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using System.Collections.Generic;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Api.ResponseService
{
    public class ScanSummariesResponseService : HubResponseService
    {

        public ScanSummariesResponseService(RestConnection restConnection) : base(restConnection)
        {
        }

        public HubPagedResponse<ScanSummaryView> GetScanSummaries(CodeLocationView codeLocationView)
        {    
            if(codeLocationView == null)
            {
                return null;
            }
            string codeLocationUrl = MetadataResponseService.GetLink(codeLocationView, ApiLinks.SCANS_LINK);
            HubRequest request = new HubRequest(RestConnection);
            request.QueryParameters[HubRequest.Q_SORT] = "updated asc";
            request.SetUriFromString(codeLocationUrl);
            HubPagedResponse<ScanSummaryView> response = request.ExecuteGetForResponsePaged<ScanSummaryView>();
            return response;
        }

        public List<ScanSummaryView> GetAllScanSummaryItems(string scanSummaryUrl)
        {
            List<ScanSummaryView> allItems = GetAllItems<ScanSummaryView>(scanSummaryUrl);
            return allItems;
        }

        public ScanSummaryView GetMostRecentScanSummary(CodeLocationView codeLocationView)
        {
            HubPagedResponse<ScanSummaryView> response = GetScanSummaries(codeLocationView);
            ScanSummaryView scanSummary = null;
            if (response != null && response.Items != null)
            {
                scanSummary = response.Items[0];
            }
            return scanSummary;
        }
    }
}
