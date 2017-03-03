using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Api
{
    public class HubRequest
    {
        public static string Q_LIMIT = "limit";

        private RestConnection RestConnection;
        private Uri Uri;
        public List<string> UrlSegments = new List<string>();
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
            foreach(T item in response.Items)
            {
                item.Json = jobject.ToString();
            }
            return response;
        }

        public T ExecuteGetForResponse<T>() where T : HubView
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
            HubResponse hubResponse = new HubResponse();
            hubResponse.Json = responseMessage;
            return hubResponse;
        }

        public string ExecutePost(string content)
        {
            HttpContent httpContent = RestConnection.CreateHttpContentJson(content);
            return ExecutePost(httpContent);
        }

        public string ExecutePost(HttpContent httpContent)
        {
            BuildUri();
            HttpResponseMessage response = RestConnection.CreatePostRequest(Uri, httpContent);
            string responseMessage = response.Content.ReadAsStringAsync().Result;
            return responseMessage;
        }

        public Uri BuildUri()
        {
            if (!string.IsNullOrWhiteSpace(Q))
            {
                QueryParameters.Add("q", Q);
            }
            if (Uri == null)
            {
                Uri = RestConnection.CreateURI(RestConnection.GetBaseUrl(), UrlSegments, QueryParameters);
            }
            return Uri;
        }
    }
}
