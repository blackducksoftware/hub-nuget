using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices
{
    class MetadataDataService : DataService
    {
        public MetadataDataService(RestConnection restConnection) : base(restConnection)
        {

        }

        public string GetLink(HubView hubView, string rel)
        {
            return GetLink(hubView.Metadata, rel);
        }

        public string GetLink(Metadata metadata, string rel)
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
