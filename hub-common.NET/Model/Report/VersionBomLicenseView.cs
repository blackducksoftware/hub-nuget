using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Report
{
    public class VersionBomLicenseView: HubResponse
    {
        [JsonProperty(PropertyName = "license")]
        public string License { get; set; }

        [JsonProperty(PropertyName = "licenseDisplay")]
        public string LicenseDisplay { get; set; }

        [JsonProperty(PropertyName = "licenseType")]
        [JsonConverter(typeof(HubEnumConverter<LicenseTypeEnum>))]
        public LicenseTypeEnum LicenseType { get; set; }

        [JsonProperty(PropertyName = "licenses")]
        public List<VersionBomLicenseView> Licenses { get; set; }
    }
}
