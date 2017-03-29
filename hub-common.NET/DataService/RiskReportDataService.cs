using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api.ResponseService;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Project;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Report;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Resource;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.DataService
{
    public class RiskReportResponseService : HubResponseService
    {

        public const string HUB_REPORTING_VERSION = "1.0.1";
        public const string RISK_REPORT_DIRECTORY = "RiskReport";
        public const string RISK_REPORT_HTML_FILE = "riskreport.html";
        public const string REPLACEMENT_TOKEN = "TOKEN_RISK_REPORT_JSON_TOKEN";

        private AggregateBomResponseService AggregateBomDataService;

        public RiskReportResponseService(RestConnection restConnection) : base(restConnection)
        {
            AggregateBomDataService = new AggregateBomResponseService(restConnection);
        }

        public ReportData GetReportData(ProjectView projectView, ProjectVersionView projectVersionView)
        {
            ReportData reportData = new ReportData()
            {           
                ProjectName = projectView.Name,
                ProjectURL = GetReportProjectUrl(projectView.Metadata.Href),
                ProjectVersion = projectVersionView.VersionName,
                ProjectVersionURL = GetReportVersionUrl(projectVersionView.Metadata.Href, false),
                Phase = projectVersionView.Phase.ToString(),
                Distribution = projectVersionView.Distribution.ToString(),
            };
            List<BomComponent> components = new List<BomComponent>();

            List<VersionBomComponentView> bomEntries = AggregateBomDataService.GetBomEntries(projectVersionView);
            foreach (VersionBomComponentView bomEntry in bomEntries)
            {
                BomComponent component;
                try
                {
                    component = CreateBomComponentFromBomComponentView(bomEntry);
                    components.Add(component);
                }
                catch(Exception ex)
                {
                    throw new BlackDuckIntegrationException("Error getting BOM Component.", ex);
                }

                string componentPolicyStatusURL = null;
                if (!String.IsNullOrWhiteSpace(bomEntry.ComponentVersion))
                {
                    componentPolicyStatusURL = GetComponentPolicyUrl(projectVersionView.Metadata.Href, bomEntry.ComponentVersion);
                }
                else
                {
                    componentPolicyStatusURL = GetComponentPolicyUrl(projectVersionView.Metadata.Href, bomEntry.Component);
                }

                CheckPolicyStatusForComponent(componentPolicyStatusURL, component);
            }

            reportData.SetComponents(components);
            return reportData;
        }

        private void CheckPolicyStatusForComponent(string componentPolicyStatusURL, BomComponent component)
        {
            try 
            {
                HubRequest request = new HubRequest(RestConnection)
                {
                    Uri = new Uri(componentPolicyStatusURL)
                };
                BomComponentPolicyStatusView bomPolicyStatus = request.ExecuteGetForResponse<BomComponentPolicyStatusView>();
                component.PolicyStatus = bomPolicyStatus.ApprovalStatus.ToString();

            }
            catch (BlackDuckIntegrationException)
            {

            }
        }

        private string GetReportProjectUrl(string projectUrl)
        {
            string baseUrl = RestConnection.GetBaseUrl();
            string projectId = new UrlHelper().GetFirstId(projectUrl);
            string url = $"{baseUrl}/#projects/id:{projectId}";
            return url;
        }

        private string GetReportVersionUrl(string versionUrl, bool isComponent)
        {
            string baseUrl = RestConnection.GetBaseUrl();
            string versionId = new UrlHelper().GetLastId(versionUrl);
            string url = $"{baseUrl}/#versions/id:{versionId}";
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

            BomComponent component = new BomComponent()
            {
                ComponentName = bomEntry.ComponentName,
                ComponentURL = GetReportProjectUrl(bomEntry.Component),
                ComponentVersion = bomEntry.ComponentVersionName,
                ComponentVersionURL = GetReportVersionUrl(bomEntry.ComponentVersion, true)
            };

            string displayLicense = "";
            foreach (VersionBomLicenseView license in bomEntry.Licenses)
            {
                displayLicense = license.LicenseDisplay + " ";
            }
            component.License = displayLicense;

            if (bomEntry.SecurityRiskProfile != null && bomEntry.SecurityRiskProfile.Counts != null
                    && bomEntry.SecurityRiskProfile.Counts.Count != 0)
            {
                foreach (RiskCountView count in bomEntry.SecurityRiskProfile.Counts)
                {
                    if (count.CountType.Equals(RiskCountEnum.HIGH) && count.Count > 0)
                    {
                        component.SecurityRiskHighCount = count.Count;
                    }
                    else if (count.CountType.Equals(RiskCountEnum.MEDIUM) && count.Count > 0)
                    {
                        component.SecurityRiskMediumCount = count.Count;
                    }
                    else if (count.CountType.Equals(RiskCountEnum.LOW) && count.Count > 0)
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
                    if (count.CountType.Equals(RiskCountEnum.HIGH) && count.Count > 0)
                    {
                        component.LicenseRiskHighCount = count.Count;
                    }
                    else if (count.CountType.Equals(RiskCountEnum.MEDIUM) && count.Count > 0)
                    {
                        component.LicenseRiskMediumCount = count.Count;
                    }
                    else if (count.CountType.Equals(RiskCountEnum.LOW) && count.Count > 0)
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
                    if (count.CountType.Equals(RiskCountEnum.HIGH) && count.Count > 0)
                    {
                        component.OperationalRiskHighCount = count.Count;
                    }
                    else if (count.CountType.Equals(RiskCountEnum.MEDIUM) && count.Count > 0)
                    {
                        component.OperationalRiskMediumCount = count.Count;
                    }
                    else if (count.CountType.Equals(RiskCountEnum.LOW) && count.Count > 0)
                    {
                        component.OperationalRiskLowCount = count.Count;
                    }
                }
            }
            return component;
        }

        private void CopyRiskReport(string outputPath, string version = HUB_REPORTING_VERSION)
        {
            string remoteFileName = $"hub-common-reporting-{version}.jar";
            string filePath = $"{outputPath}/{remoteFileName}";
            string extractionDirectory = $"{outputPath}/Temp_RiskReport";
            string riskReportFiles = $"{extractionDirectory}/riskreport/web/";
            string url = "http://oss.sonatype.org/content/repositories/releases/com/blackducksoftware/integration/hub-common-reporting";
            url += $"/{version}/{remoteFileName}";

            // Cleanup any old stuff
            string riskReportDirectory = $"{outputPath}/{RISK_REPORT_DIRECTORY}";
            if (Directory.Exists(riskReportDirectory))
            {
                Directory.Delete(riskReportDirectory, true);
            }
            if(Directory.Exists(extractionDirectory))
            {
                Directory.Delete(extractionDirectory, true);
            }

            // Fetch the file
            ResourceCopier resourceCopier = new ResourceCopier();
            resourceCopier.CopyFromWeb(url, filePath);

            // Extract the resources   
            ZipFile.ExtractToDirectory(filePath, extractionDirectory);
            Directory.Move(riskReportFiles, riskReportDirectory);

            // Cleanup mess
            File.Delete(filePath);
            Directory.Delete(extractionDirectory, true);
        }

        public void WriteToRiskReport(ReportData reportData, string outputDirectory)
        {
            CopyRiskReport(outputDirectory);
            string htmlFilePath = $"{outputDirectory}/{RISK_REPORT_DIRECTORY}/{RISK_REPORT_HTML_FILE}";

            string htmlFile = File.ReadAllText(htmlFilePath);
            htmlFile =htmlFile.Replace(REPLACEMENT_TOKEN, JToken.FromObject(reportData).ToString());

            File.WriteAllText(htmlFilePath, htmlFile);
        }
    }
}