using Com.Blackducksoftware.Integration.Hub.Common.Net.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Rest
{
    public class CredentialsResetConnection : RestConnection
    {
        public CredentialsResetConnection(HubServerConfig hubServerConfig) : base(hubServerConfig)
        {
            string hubUrl = hubServerConfig.Url;

            Dictionary<string, string> credentials = new Dictionary<string, string>
            {
                {"j_username", hubServerConfig.HubCredentials.Username },
                {"j_password", hubServerConfig.HubCredentials.Password }
            };

            HttpContent content = new FormUrlEncodedContent(credentials);
            HttpResponseMessage response = PostAsync($"{hubUrl}/j_spring_security_check", content).Result;
        }
    }
}
