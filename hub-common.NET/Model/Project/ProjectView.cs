using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Project
{
    public class ProjectView : HubView
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "projectLevelAdjustments")]
        public bool ProjectLevelAdjustment { get; set; }

        [JsonProperty(PropertyName = "projectTier")]
        public string ProjectTier { get; set; }

        [JsonProperty(PropertyName = "source")]
        [JsonConverter(typeof(HubEnumConverter<SourceEnum>))]
        public SourceEnum Source { get; set; }
    }
}
