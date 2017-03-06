using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model
{
    public class Metadata
    {
        [JsonProperty(PropertyName = "allow")]
        public string[] AllowedRequestTypes { get; set; }

        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }

        [JsonProperty(PropertyName = "links")]
        public Link[] Links { get; set; }

        public List<string> ExtractIds(string url)
        {
            List<string> ids = new List<string>();
            Regex regex = new Regex("(\\w*-){2,}\\w*");
            MatchCollection matches = regex.Matches(url);
            foreach(Match match in matches)
            {
                ids.Add(match.Value);
            }
            return ids;
        }

        public string GetFirstId(string url)
        {
            return ExtractIds(url).First();
        }
        
        public string GetId(string url, int index)
        {
            List<string> ids = ExtractIds(url);
            return ids[index];
        }
    }
}
