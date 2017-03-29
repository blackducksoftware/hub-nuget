﻿using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.CodeLocation;
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