using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model
{
    class VersionBomLicenseView: HubResponse
    {
        [JsonProperty(PropertyName = "license")]
        public string License { get; set; }

        [JsonProperty(PropertyName = "licenseDisplay")]
        public string LicenseDisplay { get; set; }

        [JsonProperty(PropertyName = "licenseType")]
        public string LicenseType { get; set; }

        [JsonProperty(PropertyName = "licenses")]
        public List<VersionBomLicenseView> Licenses { get; set; }
    }
}
