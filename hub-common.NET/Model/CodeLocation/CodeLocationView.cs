using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.CodeLocation
{
    public class CodeLocationView : HubView
    {
        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(HubEnumConverter<CodeLocationTypeEnum>))]
        public CodeLocationTypeEnum Type { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "mappedProjectVersion")]
        public string MappedProjectVersion { get; set; }
    }
}
