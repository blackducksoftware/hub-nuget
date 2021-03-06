﻿/*******************************************************************************
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
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Api
{
    public class HubRequest
    {
        public const string Q_OFFSET = "offset";
        public const string Q_LIMIT = "limit";
        public const string Q_SORT = "sort";
        public const string Q_QUERY = "q";
        public const string Q_CODE_LOCATION_TYPE = "codeLocationType";

        private RestConnection RestConnection;

        public Uri Uri { get; set; }

        public string Path = "";
        public Dictionary<string, string> QueryParameters = new Dictionary<string, string>();

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

        public void SetUriFromString(string absoluteUri)
        {
            Uri = new Uri(absoluteUri, UriKind.Absolute);
        }

        public Uri BuildUri()
        {
            if (Uri == null)
            {
                Uri = RestConnection.CreateURI(RestConnection.GetBaseUrl(), Path, QueryParameters);
            } else
            {
                Uri = RestConnection.CreateURI(Uri, QueryParameters);
            }
            return Uri;
        }
    }
}
