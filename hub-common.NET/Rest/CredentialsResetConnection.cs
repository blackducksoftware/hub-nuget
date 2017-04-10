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
using System.Collections.Generic;
using System.Net.Http;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Rest
{
    public class CredentialsResetConnection : RestConnection
    {
        public CredentialsResetConnection(HubServerConfig hubServerConfig) : base(hubServerConfig)
        {
            string hubUrl = hubServerConfig.Url;

            Dictionary<string, string> credentials = new Dictionary<string, string>
            {
                {"j_username", hubServerConfig.HubCredentials.Username },
                {"j_password", hubServerConfig.HubCredentials.Password }
            };

            HttpContent content = new FormUrlEncodedContent(credentials);
            HttpResponseMessage response = PostAsync($"{hubUrl}/j_spring_security_check", content).Result;
        }
    }
}
