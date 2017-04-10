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
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Report
{
    public class ActivityDataView : HubResponse
    {
        [JsonProperty(PropertyName = "commitCount12Month")]
        public int CommitCount12Month { get; set; }

        [JsonProperty(PropertyName = "contributorCount12Month")]
        public int ContributorCount12Month { get; set; }

        [JsonProperty(PropertyName = "lastCommitDate")]
        public string LastCommitDate { get; set; }

        [JsonProperty(PropertyName = "trending")]
        [JsonConverter(typeof(HubEnumConverter<TrendingEnum>))]
        public TrendingEnum Trending { get; set; }
    }
}