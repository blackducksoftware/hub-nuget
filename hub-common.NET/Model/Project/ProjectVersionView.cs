using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.ComplexLicense;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Project
{
    public class ProjectVersionView: HubView
    {
        [JsonProperty(PropertyName = "distribution")]
        [JsonConverter(typeof(HubEnumConverter<DistributionEnum>))]
        public DistributionEnum Distribution { get; set; }

        [JsonProperty(PropertyName = "license")]
        public ComplexLicenseView License { get; set; }

        [JsonProperty(PropertyName = "nickname")]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "phase")]
        [JsonConverter(typeof(HubEnumConverter<PhaseEnum>))]
        public PhaseEnum Phase { get; set; }

        [JsonProperty(PropertyName = "releaseComments")]
        public string ReleaseComments { get; set; }

        [JsonProperty(PropertyName = "releasedOn")]
        public string ReleasedOn { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        [JsonProperty(PropertyName = "versionName")]
        public string VersionName { get; set; }
    }
}
