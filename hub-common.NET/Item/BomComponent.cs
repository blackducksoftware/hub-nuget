using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Items
{
    public class BomComponent
    {
        [JsonProperty(PropertyName = "policyStatus")]
        public string PolicyStatus { get; set; }

        [JsonProperty(PropertyName = "componentName")]
        public string ComponentName { get; set; }

        [JsonProperty(PropertyName = "componentURL")]
        public string ComponentURL { get; set; }

        [JsonProperty(PropertyName = "componentVersion")]
        public string ComponentVersion { get; set; }

        [JsonProperty(PropertyName = "componentVersionURL")]
        public string ComponentVersionURL { get; set; }

        [JsonProperty(PropertyName = "license")]
        public string License { get; set; }

        [JsonProperty(PropertyName = "securityRiskHighCount")]
        public int SecurityRiskHighCount { get; set; }

        [JsonProperty(PropertyName = "securityRiskMediumCount")]
        public int SecurityRiskMediumCount { get; set; }

        [JsonProperty(PropertyName = "securityRiskLowCount")]
        public int SecurityRiskLowCount { get; set; }

        [JsonProperty(PropertyName = "licenseRiskHighCount")]
        public int LicenseRiskHighCount { get; set; }

        [JsonProperty(PropertyName = "licenseRiskMediumCount")]
        public int LicenseRiskMediumCount { get; set; }

        [JsonProperty(PropertyName = "licenseRiskLowCount")]
        public int LicenseRiskLowCount { get; set; }

        [JsonProperty(PropertyName = "operationalRiskHighCount")]
        public int OperationalRiskHighCount { get; set; }

        [JsonProperty(PropertyName = "operationalRiskMediumCount")]
        public int OperationalRiskMediumCount { get; set; }

        [JsonProperty(PropertyName = "operationalRiskLowCount")]
        public int OperationalRiskLowCount { get; set; }
    }
}
