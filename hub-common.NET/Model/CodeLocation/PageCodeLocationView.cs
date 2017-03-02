using Newtonsoft.Json;
using System.Collections.Generic;

namespace Com.Blackducksoftware.Integration.HubCommon.NET.Model.CodeLocation
{
    public class PageCodeLocationView
    {
        [JsonProperty(PropertyName = "totalCount")]
        public int TotalCount { get; set; }

        [JsonProperty(PropertyName = "items")]
        public List<CodeLocationView> Items { get; set; }
    }
}
