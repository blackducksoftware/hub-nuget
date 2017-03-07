using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.ComplexLicense
{
    public class ComplexLicenseView : HubView
    {
        [JsonProperty(PropertyName = "present")]
        public bool Present { get; set; }
    }
}
