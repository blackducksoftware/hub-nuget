namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Items
{
    public class BomComponent: Item
    {
        public string PolicyStatus { get; set; }
        public string ComponentName { get; set; }
        public string ComponentURL { get; set; }
        public string ComponentVersion { get; set; }
        public string ComponentVersionURL { get; set; }
        public string License { get; set; }
        public int SecurityRiskHighCount { get; set; }
        public int SecurityRiskMediumCount { get; set; }
        public int SecurityRiskLowCount { get; set; }
        public int LicenseRiskHighCount { get; set; }
        public int LicenseRiskMediumCount { get; set; }
        public int LicenseRiskLowCount { get; set; }
        public int OperationalRiskHighCount { get; set; }
        public int OperationalRiskMediumCount { get; set; }
        public int OperationalRiskLowCount { get; set; }
    }
}
