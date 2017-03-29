using Com.Blackducksoftware.Integration.Hub.Common.Net.Model;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Api.ResponseService
{
    class MetadataResponseService : HubResponseService
    {
        public MetadataResponseService(RestConnection restConnection) : base(restConnection)
        {

        }

        public static string GetLink(HubView hubView, string rel)
        {
            return GetLink(hubView.Metadata, rel);
        }

        public static string GetLink(Metadata metadata, string rel)
        {
            foreach(Link link in metadata.Links)
            {
                if(link.Rel == rel)
                {
                    return link.Href;
                }
            }
            return null;
        }
    }
}
