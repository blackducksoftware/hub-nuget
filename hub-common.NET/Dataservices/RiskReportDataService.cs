using System.Collections.Generic;
using System.Linq;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Reporting;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Project;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Report;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Constants;
using System;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices
{
    public class RiskReportDataService : DataService
    {
        private AggregateBomDataService AggregateBomDataService;

        public RiskReportDataService(RestConnection restConnection) : base(restConnection)
        {
            AggregateBomDataService = new AggregateBomDataService(restConnection);
        }

        public ReportData GetReportData(Project projectItem)
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

            List<VersionBomComponentView> bomEntries = AggregateBomDataService.GetBomEntries(projectItem);
            foreach (VersionBomComponentView bomEntry in bomEntries)
            {
                BomComponent component = CreateBomComponentFromBomComponentView(bomEntry);
                string componentPolicyStatusURL = null;
                if (string.IsNullOrWhiteSpace(bomEntry.ComponentVersion))
                {
                    componentPolicyStatusURL = GetComponentPolicyUrl(versionView.Metadata.Href, component.ComponentVersion);
                }
            }

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

        private string GetComponentPolicyUrl(string versionUrl, string componentUrl)
        {
            string componentVersionSegments = componentUrl.Substring(componentUrl.IndexOf(ApiLinks.COMPONENTS_LINK));
            return $"{versionUrl}/{componentVersionSegments}/{ApiLinks.POLICY_STATUS_LINK}";
        }

        private BomComponent CreateBomComponentFromBomComponentView(VersionBomComponentView bomEntry)
        {

            BomComponent component = new BomComponent();
            component.ComponentName = bomEntry.ComponentName;
            component.ComponentURL = GetReportProjectUrl(bomEntry.Component);
            component.ComponentVersion = bomEntry.ComponentVersionName;
            component.ComponentVersionURL = GetReportVersionUrl(bomEntry.ComponentVersion, true);

            if (bomEntry.SecurityRiskProfile != null && bomEntry.SecurityRiskProfile.Counts != null
                    && bomEntry.SecurityRiskProfile.Counts.Count != 0)
            {
                foreach (RiskCountView count in bomEntry.SecurityRiskProfile.Counts)
                {
                    if (count.CountType == RiskCountEnum.HIGH && count.Count > 0)
                    {
                        component.SecurityRiskHighCount = count.Count;
                    }
                    else if (count.CountType == RiskCountEnum.MEDIUM && count.Count > 0)
                    {
                        component.SecurityRiskMediumCount = count.Count;
                    }
                    else if (count.CountType == RiskCountEnum.LOW && count.Count > 0)
                    {
                        component.SecurityRiskLowCount = count.Count;
                    }
                }
            }
            if (bomEntry.LicenseRiskProfile != null && bomEntry.LicenseRiskProfile.Counts != null
                    && bomEntry.LicenseRiskProfile.Counts.Count != 0)
            {
                foreach (RiskCountView count in bomEntry.LicenseRiskProfile.Counts)
                {
                    if (count.CountType == RiskCountEnum.HIGH && count.Count > 0)
                    {
                        component.LicenseRiskHighCount = count.Count;
                    }
                    else if (count.CountType == RiskCountEnum.MEDIUM && count.Count > 0)
                    {
                        component.LicenseRiskMediumCount = count.Count;
                    }
                    else if (count.CountType == RiskCountEnum.LOW && count.Count > 0)
                    {
                        component.LicenseRiskLowCount = count.Count;
                    }
                }
            }
            if (bomEntry.OperationalRiskProfile != null && bomEntry.OperationalRiskProfile.Counts != null
                    && bomEntry.OperationalRiskProfile.Counts.Count != 0)
            {
                foreach (RiskCountView count in bomEntry.OperationalRiskProfile.Counts)
                {
                    if (count.CountType == RiskCountEnum.HIGH && count.Count > 0)
                    {
                        component.OperationalRiskHighCount = count.Count;
                    }
                    else if (count.CountType == RiskCountEnum.MEDIUM && count.Count > 0)
                    {
                        component.OperationalRiskMediumCount = count.Count;
                    }
                    else if (count.CountType == RiskCountEnum.LOW && count.Count > 0)
                    {
                        component.OperationalRiskLowCount = count.Count;
                    }
                }
            }
            return component;

        }
    }
}