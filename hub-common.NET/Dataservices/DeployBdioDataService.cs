using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices
{
    public class DeployBdioDataService : DataService
    {
        public DeployBdioDataService(RestConnection restConnection) : base(restConnection)
        {
        }

    }
}
