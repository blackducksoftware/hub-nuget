using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model
{
    public class ProjectView : HubView
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "projectLevelAdjustments")]
        public bool ProjectLevelAdjustment { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }
    }
}
