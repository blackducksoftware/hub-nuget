using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices
{
    public class DataService
    {
        public RestConnection RestConnection { get; set; }

        public DataService(RestConnection restConnection)
        {
            RestConnection = restConnection;
        }
    }
}
