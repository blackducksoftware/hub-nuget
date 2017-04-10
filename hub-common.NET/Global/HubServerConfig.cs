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
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Global
{
    public class HubServerConfig
    {
        public string Url { get; set; }
        public int Timeout { get; set; }
        public HubCredentials HubCredentials { get; set; }
        public HubProxyInfo HubProxyInfo { get; set; }

        public HubServerConfig()
        {

        }

        public HubServerConfig(string url, int timeout, string username, string password) : this(url, timeout, new HubCredentials(username, password), null)
        {
            // Use username and password as string. No proxy
        }

        public HubServerConfig(string url, int timeout, HubCredentials hubCredentials, HubProxyInfo hubProxyInfo)
        {
            Url = url;
            Timeout = timeout;
            HubCredentials = hubCredentials;
            HubProxyInfo = HubProxyInfo;
        }


    }
}
