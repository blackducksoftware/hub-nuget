using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model
{
    public class ComplexLicenseView : HubView
    {
        [JsonProperty(PropertyName = "present")]
        public bool Present { get; set; }
    }
}
