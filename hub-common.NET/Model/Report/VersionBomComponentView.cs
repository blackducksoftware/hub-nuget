using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Report
{
    public class VersionBomComponentView : HubView
    {
        [JsonProperty(PropertyName = "componentName")]
        public string ComponentName { get; set; }

        [JsonProperty(PropertyName = "componentVersionName")]
        public string ComponentVersionName { get; set; }

        [JsonProperty(PropertyName = "component")]
        public string ComponentUrl { get; set; }

        [JsonProperty(PropertyName = "componentVersion")]
        public string ComponentVersionUrl { get; set; }

        [JsonProperty(PropertyName = "licenses")]
        public List<VersionBomLicenseView> Licenses { get; set; }
    }
}
