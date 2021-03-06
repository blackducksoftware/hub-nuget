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
