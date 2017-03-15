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
        MetadataDataService MetadataDataService;

        public ScanSummariesDataService(RestConnection restConnection) : base(restConnection)
        {
            MetadataDataService = new MetadataDataService(RestConnection);
        }

        public HubPagedResponse<ScanSummaryView> GetScanSummaries(CodeLocationView codeLocationView)
        {    
            if(codeLocationView == null)
            {
                return null;
            }
            string codeLocation = MetadataDataService.GetLink(codeLocationView, ApiLinks.SCANS_LINK);
            HubRequest request = new HubRequest(RestConnection);
            request.QueryParameters.Add(HubRequest.Q_SORT, "updated asc");
            request.Uri = new Uri(codeLocation, UriKind.Absolute);
            HubPagedResponse<ScanSummaryView> response = request.ExecuteGetForResponsePaged<ScanSummaryView>();
            return response;
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
