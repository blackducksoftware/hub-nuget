using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using System.Collections.Generic;
using System.Net;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Reporting
{
    public class ReportData
    {
        public string ProjectName { get; set; }
        public string ProjectURL { get; set; }
        public string ProjectVersion { get; set; }
        public string ProjectVersionURL { get; set; }
        public string Phase { get; set; }
        public string Distribution { get; set; }

        public List<BomComponent> Components { get; private set; }
        public int TotalComponents { get; private set; }

        public int VulnerabilityRiskHighCount { get; private set; }
        public int VulnerabilityRiskMediumCount { get; private set; }
        public int VulnerabilityRiskLowCount { get; private set; }
        public int VulnerabilityRiskNoneCount { get; private set; }

        public int LicenseRiskHighCount { get; private set; }
        public int LicenseRiskMediumCount { get; private set; }
        public int LicenseRiskLowCount { get; private set; }
        public int LicenseRiskNoneCount { get; private set; }

        public int OperationalRiskHighCount { get; private set; }
        public int OperationalRiskMediumCount { get; private set; }
        public int OperationalRiskLowCount { get; private set; }
        public int OperationalRiskNoneCount { get; private set; }

        public void SetComponents(List<BomComponent> components)
        {
            Components = components;

            VulnerabilityRiskHighCount = 0;
            VulnerabilityRiskMediumCount = 0;
            VulnerabilityRiskLowCount = 0;

            LicenseRiskHighCount = 0;
            LicenseRiskMediumCount = 0;
            LicenseRiskLowCount = 0;

            OperationalRiskHighCount = 0;
            OperationalRiskMediumCount = 0;
            OperationalRiskLowCount = 0;

            foreach (BomComponent component in components)
            {
                AddComponentValues(component);
            }
            TotalComponents = components.Count;

            VulnerabilityRiskNoneCount = TotalComponents - VulnerabilityRiskHighCount - VulnerabilityRiskMediumCount - VulnerabilityRiskLowCount;
            LicenseRiskNoneCount = TotalComponents - LicenseRiskHighCount - LicenseRiskMediumCount - LicenseRiskLowCount;
            OperationalRiskNoneCount = TotalComponents - OperationalRiskHighCount - OperationalRiskMediumCount - OperationalRiskLowCount;
        }

        public void AddComponentValues(BomComponent component)
        {
            if (component != null)
            {
                // Vulnerabilities
                if (component.SecurityRiskHighCount > 0)
                {
                    VulnerabilityRiskHighCount++;
                }
                else if (component.SecurityRiskMediumCount > 0)
                {
                    VulnerabilityRiskMediumCount++;
                }
                else if (component.SecurityRiskLowCount > 0)
                {
                    VulnerabilityRiskLowCount++;
                }

                // License
                if (component.LicenseRiskHighCount > 0)
                {
                    LicenseRiskHighCount++;
                }
                else if (component.LicenseRiskMediumCount > 0)
                {
                    LicenseRiskMediumCount++;
                }
                else if (component.LicenseRiskLowCount > 0)
                {
                    LicenseRiskLowCount++;
                }

                // Operational
                if (component.OperationalRiskHighCount > 0)
                {
                    OperationalRiskHighCount++;
                }
                else if (component.OperationalRiskMediumCount > 0)
                {
                    OperationalRiskMediumCount++;
                }
                else if (component.OperationalRiskLowCount > 0)
                {
                    OperationalRiskLowCount++;
                }
            }
        }

        public string HtmlEscape(string valueToEscape)
        {
            if(string.IsNullOrWhiteSpace(valueToEscape))
            {
                return null;
            }
            return WebUtility.HtmlEncode(valueToEscape);
        }
    }
}
