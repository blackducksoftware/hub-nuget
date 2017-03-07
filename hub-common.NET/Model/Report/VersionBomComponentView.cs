using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Report
{
    public class VersionBomComponentView : HubView
    {
        [JsonProperty(PropertyName = "activityData")]
        public ActivityDataView ActivityData { get; set; }

        [JsonProperty(PropertyName = "activityRiskProfile")]
        public RiskProfileView RiskProfile { get; set; }

        [JsonProperty(PropertyName = "component")]
        public string Component { get; set; }

        [JsonProperty(PropertyName = "componentName")]
        public string ComponentName { get; set; }

        [JsonProperty(PropertyName = "componentVersion")]
        public string ComponentVersion { get; set; }

        [JsonProperty(PropertyName = "componentVersionName")]
        public string ComponentVersionName { get; set; }

        [JsonProperty(PropertyName = "licenseRiskProfile")]
        public RiskProfileView LicenseRiskProfile { get; set; }

        [JsonProperty(PropertyName = "licenses")]
        public List<VersionBomLicenseView> Licenses { get; set; }

        [JsonProperty(PropertyName = "operationalRiskProfile")]
        public RiskProfileView OperationalRiskProfile { get; set; }

        [JsonProperty(PropertyName = "releasedOn")]
        public string ReleasedOn { get; set; }

        [JsonProperty(PropertyName = "securityRiskProfile")]
        public RiskProfileView SecurityRiskProfile { get; set; }

        [JsonProperty(PropertyName = "versionRiskProfile")]
        public RiskProfileView VersionRiskProfile { get; set; }
    }
}
