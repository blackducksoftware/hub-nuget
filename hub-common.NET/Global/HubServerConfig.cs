using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Com.Blackducksoftware.Integration.HubCommon.NET.Global
{
    public class HubServerConfig
    {
        public string Url { get; set; }
        public int Timeout { get; set; }
        public HubCredentials HubCredentials { get; set; }
        public HubProxyInfo HubProxyInfo { get; set; }

        public HubServerConfig()
        {

        }

        public HubServerConfig(string url, int timeout, string username, string password) : this(url, timeout, new HubCredentials(username, password), null)
        {
            // Use username and password as string. No proxy
        }

        public HubServerConfig(string url, int timeout, HubCredentials hubCredentials, HubProxyInfo hubProxyInfo)
        {
            Url = url;
            Timeout = timeout;
            HubCredentials = hubCredentials;
            HubProxyInfo = HubProxyInfo;
        }


    }
}
