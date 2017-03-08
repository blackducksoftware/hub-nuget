using System;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.CodeLocation;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.ScanStatus;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices
{
    public class ScanSummariesDataService : DataService
    { 
        public ScanSummariesDataService(RestConnection restConnection) : base(restConnection)
        {
        }

        public HubPagedResponse<ScanSummaryView> GetScanSummaries(CodeLocationView codeLocationView)
        {
            return GetScanSummaries(GetCodeLocationId(codeLocationView));
        }

        public HubPagedResponse<ScanSummaryView> GetScanSummaries(string codeLocationId)
        {
            HubRequest request = new HubRequest(RestConnection);
            request.QueryParameters.Add(HubRequest.Q_SORT, "updated asc");
            request.Path = $"api/{ApiLinks.CODE_LOCATION_LINK}/{codeLocationId}/{ApiLinks.SCAN_SUMMARIES_LINK}";
            Console.WriteLine(request.BuildUri().ToString());
            HubPagedResponse<ScanSummaryView> response = request.ExecuteGetForResponsePaged<ScanSummaryView>();
            return response;
        }

        public ScanSummaryView GetMostRecentScanSummary(CodeLocationView codeLocationView)
        {
            return GetMostRecentScanSummary(GetCodeLocationId(codeLocationView));
        }

        public ScanSummaryView GetMostRecentScanSummary(string codeLocationId)
        {
            HubPagedResponse<ScanSummaryView> response = GetScanSummaries(codeLocationId);
            ScanSummaryView scanSummary = null;
            if (response.Items != null)
            {
                scanSummary = response.Items[0];
            }
            return scanSummary;
        }

        public string GetCodeLocationId(CodeLocationView codeLocation)
        {
            if (codeLocation == null)
            {
                return null;
            }
            return codeLocation.Metadata.GetFirstId(codeLocation.Metadata.Href);
        }
    }
}
