using Com.Blackducksoftware.Integration.Hub.Common.Net.Global;
using System;
using System.Net.Http;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Rest
{
    public class MockRestConnection : CredentialsResetConnection
    {
        public MockRestConnection(HubServerConfig hubServerConfig) : base(hubServerConfig)
        {
            
        }

        public override HttpResponseMessage CreateGetRequest(Uri uri)
        {
            // Instead of making a call. Lets do some mocking
            return GetAsync(uri).Result;
        }

        public override HttpResponseMessage CreatePostRequest(Uri uri, HttpContent content)
        {
            // Instead of making a call. Lets do some mocking
            HttpResponseMessage response = PostAsync(uri, content).Result;
            return response;
        }
    }
}
