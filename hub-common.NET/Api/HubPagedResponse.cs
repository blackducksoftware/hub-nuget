using Com.Blackducksoftware.Integration.Hub.Common.Net.Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Api
{
    public class HubPagedResponse<T> : HubResponse, IEnumerable<T> where T : HubView
    {
        [JsonProperty(PropertyName = "totalCount")]
        public int TotalCount { get; set; }

        [JsonProperty(PropertyName = "items")]
        public List<T> Items { get; set; }

        [JsonProperty(PropertyName = "_meta")]
        public Metadata Metadata { get; set; }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)Items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)Items).GetEnumerator();
        }
    }
}
