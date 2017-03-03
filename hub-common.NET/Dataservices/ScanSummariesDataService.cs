using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;

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
            request.Path = $"api/codelocations/{codeLocationId}/scan-summaries";
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
            HubRequest request = new HubRequest(RestConnection);
            request.QueryParameters.Add(HubRequest.Q_SORT, "updated asc");
            request.Path = $"api/codelocations/{codeLocationId}/scan-summaries";
            Console.WriteLine(request.BuildUri().ToString());
            HubPagedResponse<ScanSummaryView> response = request.ExecuteGetForResponsePaged<ScanSummaryView>();
            ScanSummaryView scanSummary = null;
            if (response.Items != null)
            {
                scanSummary = response.Items[0];
            }
            return scanSummary;
        }

        public string GetCodeLocationId(CodeLocationView codeLocation)
        {
            return codeLocation.Metadata.GetFirstId(codeLocation.Metadata.Href);
        }
    }
}
