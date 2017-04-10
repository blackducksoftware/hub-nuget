/*******************************************************************************
 * Copyright (C) 2017 Black Duck Software, Inc.
 * http://www.blackducksoftware.com/
 *
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements. See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership. The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied. See the License for the
 * specific language governing permissions and limitations
 * under the License.
 *******************************************************************************/
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
        internal HubServerConfig HubServerConfig;

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

        public Uri CreateURI(Uri uri, Dictionary<string, string> queryParameters)
        {
            UriBuilder uriBuilder = new UriBuilder(uri);
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

        public Uri CreateURI(string baseUrl, string path, Dictionary<string, string> queryParameters)
        {
            Uri uri = new Uri($"{baseUrl}/{path}", UriKind.Absolute);
            return CreateURI(uri, queryParameters);
        }
        #endregion

        public virtual HttpResponseMessage CreateGetRequest(Uri uri)
        {
            HttpResponseMessage response = GetAsync(uri).Result;
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            throw new BlackDuckIntegrationException($"The hub returned status code: {response.StatusCode} @ {uri.AbsoluteUri}");
        }

        public virtual HttpResponseMessage CreatePostRequest(Uri uri, HttpContent content)
        {
            HttpResponseMessage response = PostAsync(uri, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            throw new BlackDuckIntegrationException($"The hub returned status code: {response.StatusCode} @ {uri.AbsolutePath}");
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
