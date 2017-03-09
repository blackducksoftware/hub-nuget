using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Api
{
    public class HubRequest
    {
        public static string Q_OFFSET = "offset";
        public static string Q_LIMIT = "limit";
        public static string Q_SORT = "sort";
        public static string Q_QUERY = "q";

        private RestConnection RestConnection;
        public Uri Uri;
        public string Path = "";
        public Dictionary<string, string> QueryParameters = new Dictionary<string, string>();
        public string Q;

        public HubRequest(RestConnection restConnection)
        {
            RestConnection = restConnection;
        }

        public HubPagedResponse<T> ExecuteGetForResponsePaged<T>() where T : HubView
        {
            JObject jobject = ExecuteGetForResponseJson();
            HubPagedResponse<T> response = jobject.ToObject<HubPagedResponse<T>>();
            response.Json = jobject.ToString();
            if (response.Items != null)
            {
                foreach (T item in response.Items)
                {
                    item.Json = jobject.ToString();
                }
            }
            return response;
        }

        public T ExecuteGetForResponse<T>() where T : HubResponse
        {
            JObject jobject = ExecuteGetForResponseJson();
            T result = jobject.ToObject<T>();
            result.Json = jobject.ToString();
            return result;
        }

        public JObject ExecuteGetForResponseJson()
        {
            string responseMessage = ExecuteGetForResponseString();
            JObject jsonObject = JObject.Parse(responseMessage);
            return jsonObject;
        }

        public string ExecuteGetForResponseString()
        {
            return ExecuteGetForHubResponse().Json;
        }

        public HubResponse ExecuteGetForHubResponse()
        {
            BuildUri();
            HttpResponseMessage response = RestConnection.CreateGetRequest(Uri);
            string responseMessage = response.Content.ReadAsStringAsync().Result;
            HubResponse hubResponse = new HubResponse
            {
                Json = responseMessage
            };
            return hubResponse;
        }

        public HttpResponseMessage ExecuteJsonPost(string content)
        {
            HttpContent httpContent = RestConnection.CreateHttpContentJson(content);
            return ExecutePost(httpContent);
        }

        public HttpResponseMessage ExecuteJsonLDPost(string content)
        {
            HttpContent httpContent = RestConnection.CreateHttpContentJsonLD(content);
            return ExecutePost(httpContent);
        }

        public HttpResponseMessage ExecutePost(HttpContent httpContent)
        {
            BuildUri();
            HttpResponseMessage response = RestConnection.CreatePostRequest(Uri, httpContent);
            return response;
        }

        public Uri BuildUri()
        {
            if (!string.IsNullOrWhiteSpace(Q))
            {
                QueryParameters.Add("q", Q);
            }
            if (Uri == null)
            {
                Uri = RestConnection.CreateURI(RestConnection.GetBaseUrl(), Path, QueryParameters);
            }
            return Uri;
        }
    }
}
