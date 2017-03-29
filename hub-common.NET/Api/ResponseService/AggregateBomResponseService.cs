using System.Collections.Generic;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Report;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Project;
using System;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Api.ResponseService
{
    public class AggregateBomResponseService : HubResponseService
    {

        private const int BUFFER_LIMIT = 5;

        public AggregateBomResponseService(RestConnection restConnection) : base(restConnection)
        {
        }

        public List<VersionBomComponentView> GetBomEntries(ProjectVersionView projectVersionView)
        {
            string componentsUrl = MetadataResponseService.GetLink(projectVersionView, ApiLinks.COMPONENTS_LINK);
            List<VersionBomComponentView> allItems = GetAllItems<VersionBomComponentView>(componentsUrl);

            return allItems;
        }
    }
}