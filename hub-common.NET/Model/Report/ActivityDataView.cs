using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Report
{
    public class ActivityDataView : HubResponse
    {
        [JsonProperty(PropertyName = "commitCount12Month")]
        public int CommitCount12Month { get; set; }

        [JsonProperty(PropertyName = "contributorCount12Month")]
        public int ContributorCount12Month { get; set; }

        [JsonProperty(PropertyName = "lastCommitDate")]
        public string LastCommitDate { get; set; }

        [JsonProperty(PropertyName = "trending")]
        [JsonConverter(typeof(HubEnumConverter<TrendingEnum>))]
        public TrendingEnum Trending { get; set; }
    }
}