using Com.Blackducksoftware.Integration.Hub.Bdio.Simple;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using System.Net.Http;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Api.ResponseService
{
    public class DeployBdioResponseService : HubResponseService
    {

        public DeployBdioResponseService(RestConnection restConnection) : base(restConnection)
        {
            RestConnection = restConnection;
        }

        public HttpResponseMessage Deploy(BdioContent bdioContent)
        {
            HubRequest request = new HubRequest(RestConnection);
            request.Path = $"api/{ApiLinks.BOM_IMPORTS_LINK}";
            HttpResponseMessage response = request.ExecuteJsonLDPost(bdioContent.ToString());
            return response;
        }
    }
}
