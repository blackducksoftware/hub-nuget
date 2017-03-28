using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Api
{
    public class HubPagedRequest : HubRequest
    {
        public HubPagedRequest(RestConnection restConnection) : base(restConnection)
        {
            QueryParameters[Q_OFFSET] = "0";
            QueryParameters[Q_LIMIT] = "10";
        }
    }
}
