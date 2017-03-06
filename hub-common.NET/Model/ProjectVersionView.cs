using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model
{
    public class ProjectVersionView: HubView
    {
        [JsonProperty(PropertyName = "distribution")]
        public string Distribution { get; set; }

        [JsonProperty(PropertyName = "license")]
        public ComplexLicenseView License { get; set; }

        [JsonProperty(PropertyName = "nickname")]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "phase")]
        public string Phase { get; set; }

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
