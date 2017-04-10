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
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Api.ResponseService
{
    class MetadataResponseService : HubResponseService
    {
        public MetadataResponseService(RestConnection restConnection) : base(restConnection)
        {

        }

        public static string GetLink(HubView hubView, string rel)
        {
            return GetLink(hubView.Metadata, rel);
        }

        public static string GetLink(Metadata metadata, string rel)
        {
            foreach(Link link in metadata.Links)
            {
                if(link.Rel == rel)
                {
                    return link.Href;
                }
            }
            return null;
        }
    }
}
