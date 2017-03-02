using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Blackducksoftware.Integration.HubCommon.NET.Global
{
    public class HubCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public HubCredentials()
        {

        }

        public HubCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
