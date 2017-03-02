using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Blackducksoftware.Integration.HubCommon.NET.Api
{
    public class HubPagedResponse<HubView>
    {
        [JsonProperty(PropertyName = "totalCount")]
        public int TotalCount { get; set;}

        [JsonProperty(PropertyName = "items")]
        public List<HubView> Items { get; set; }
    }
}
