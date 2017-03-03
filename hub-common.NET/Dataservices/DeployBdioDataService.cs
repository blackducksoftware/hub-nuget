using System;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Bdio.Simple;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using System.Net.Http;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices
{
    public class DeployBdioDataService : DataService
    {

        public DeployBdioDataService(RestConnection restConnection) : base(restConnection)
        {
            RestConnection = restConnection;
        }

        public HttpResponseMessage Deploy(BdioContent bdioContent)
        {
            HubRequest request = new HubRequest(RestConnection);
            request.Path = $"api/bom-import";
            Console.WriteLine(request.BuildUri().ToString());
            HttpResponseMessage response = request.ExecuteJsonLDPost(bdioContent.ToString());
            return response;
        }
    }
}
