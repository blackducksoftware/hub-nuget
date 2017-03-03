using Com.Blackducksoftware.Integration.Hub.Common.Net.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Rest
{
    public class RestConnection : HttpClient
    {
        private HubServerConfig HubServerConfig;

        public RestConnection(HubServerConfig hubServerConfig) 
        {
            HubServerConfig = hubServerConfig;
            Timeout = new TimeSpan(0, 0, HubServerConfig.Timeout);
        }
    }
}
