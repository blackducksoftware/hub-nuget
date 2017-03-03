using Com.Blackducksoftware.Integration.Hub.Common.Net.Global;
using Mono.Web;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Rest
{
    public class RestConnection : HttpClient
    {
        private HubServerConfig HubServerConfig;

        public RestConnection(HubServerConfig hubServerConfig)
        {
            HubServerConfig = hubServerConfig;
            Timeout = new TimeSpan(0, 0, HubServerConfig.Timeout);
        }

        #region Utility
        public string GetBaseUrl()
        {
            return HubServerConfig.Url;
        }

        public Uri CreateURI(string baseUrl,string path, Dictionary<string, string> queryParameters)
        {
            UriBuilder uriBuilder = new UriBuilder(baseUrl);
            uriBuilder.Path = path;
            if (queryParameters != null)
            {
                NameValueCollection parameters = HttpUtility.ParseQueryString(string.Empty);
                foreach (KeyValuePair<string, string> queryParameter in queryParameters)
                {
                    string key = Uri.EscapeDataString(queryParameter.Key);
                    string value = Uri.EscapeDataString(queryParameter.Value);
                    parameters[key] = value;
                }
                uriBuilder.Query = parameters.ToString();
            }
            return uriBuilder.Uri;
        }
        #endregion

        public HttpResponseMessage CreateGetRequest(Uri uri)
        {
            return GetAsync(uri).Result;
        }

        public HttpResponseMessage CreatePostRequest(Uri uri, HttpContent content)
        {
            HttpResponseMessage response = PostAsync(uri, content).Result;
            return response;
        }

        #region HttpContent
        public HttpContent CreateHttpContentJsonLD(string content)
        {
            return CreateHttpContent(content, "application/ld+json");
        }

        public HttpContent CreateHttpContentJson(string content)
        {
            return CreateHttpContent(content, "application/json");
        }

        public HttpContent CreateHttpContent(string content, string contentType)
        {
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, contentType);
            return httpContent;
        }
        #endregion
    }
}
