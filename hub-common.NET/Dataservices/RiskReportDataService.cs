using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Reporting;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices
{
    public class RiskReportDataService : DataService
    {
        private BomRequestDataService BomRequestDataService;

        public RiskReportDataService(RestConnection restConnection) : base(restConnection)
        {
            BomRequestDataService = new BomRequestDataService(restConnection);
        }

        public ReportData GetReportData(ProjectItem projectItem)
        {
            ProjectView projectView = projectItem.ProjectView;
            ProjectVersionView versionView = projectItem.ProjectVersionView;
            ReportData reportData = new ReportData();
            reportData.ProjectName = projectView.Name;
            reportData.ProjectURL = GetReportProjectUrl(projectItem.ProjectId);
            reportData.ProjectVersion = versionView.VersionName;
            reportData.ProjectVersionURL = GetReportVersionUrl(projectItem.VersionId, false);
            reportData.Phase = versionView.Phase;
            reportData.Distribution = versionView.Distribution;

            List<BomComponent> components = new List<BomComponent>();

            /*List<VersionBomComponentView> bomEntries = BomRequestDataService.GetBomEntries(projectItem);
            foreach(VersionBomComponentView bomEntry in bomEntries)
            {
                BomComponent component = CreateBomComponentFromBomComponentView(bomEntry);
            }*/

            return reportData;
        }

        private string GetReportProjectUrl(string projectId)
        {
            string baseUrl = RestConnection.GetBaseUrl();
            string url = $"{baseUrl}/#projects/id:{projectId}";
            return url;
        }

        private string GetReportVersionUrl(string versionId, bool isComponent)
        {
            string baseUrl = RestConnection.GetBaseUrl();
            string url = $"{baseUrl}/#version/id:{versionId}";
            if (!isComponent)
            {
                url += "/view:bom";
            }
            return url;
        }
    }
}
