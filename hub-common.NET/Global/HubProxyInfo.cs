using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Global
{
    public class HubProxyInfo
    {
        public string ProxyHost { get; set; }
        public string ProxyPort { get; set; }
        public HubCredentials ProxyCredentials { get; set; }

        public HubProxyInfo()
        {

        }

        public HubProxyInfo(string proxyHost, string proxyPort, HubCredentials proxyCredentials)
        {
            ProxyHost = proxyHost;
            ProxyPort = proxyPort;
            ProxyCredentials = proxyCredentials;
        }
    }
}
