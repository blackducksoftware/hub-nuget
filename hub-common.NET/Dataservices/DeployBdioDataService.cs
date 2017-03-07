using System;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Bdio.Simple;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using System.Net.Http;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Constants;

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
            request.Path = $"api/{ApiLinks.BOM_IMPORTS_LINK}";
            Console.WriteLine(request.BuildUri().ToString());
            HttpResponseMessage response = request.ExecuteJsonLDPost(bdioContent.ToString());
            return response;
        }
    }
}
