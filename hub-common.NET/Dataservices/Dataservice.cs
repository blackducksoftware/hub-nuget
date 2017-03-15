using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices
{
    public class DataService
    {
        internal RestConnection RestConnection { get; set; }

        public DataService(RestConnection restConnection)
        {
            RestConnection = restConnection;
        }
    }
}
