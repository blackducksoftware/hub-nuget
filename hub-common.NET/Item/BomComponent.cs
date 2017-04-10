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
using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Items
{
    public class BomComponent
    {
        [JsonProperty(PropertyName = "policyStatus")]
        public string PolicyStatus { get; set; }

        [JsonProperty(PropertyName = "componentName")]
        public string ComponentName { get; set; }

        [JsonProperty(PropertyName = "componentURL")]
        public string ComponentURL { get; set; }

        [JsonProperty(PropertyName = "componentVersion")]
        public string ComponentVersion { get; set; }

        [JsonProperty(PropertyName = "componentVersionURL")]
        public string ComponentVersionURL { get; set; }

        [JsonProperty(PropertyName = "license")]
        public string License { get; set; }

        [JsonProperty(PropertyName = "securityRiskHighCount")]
        public int SecurityRiskHighCount { get; set; }

        [JsonProperty(PropertyName = "securityRiskMediumCount")]
        public int SecurityRiskMediumCount { get; set; }

        [JsonProperty(PropertyName = "securityRiskLowCount")]
        public int SecurityRiskLowCount { get; set; }

        [JsonProperty(PropertyName = "licenseRiskHighCount")]
        public int LicenseRiskHighCount { get; set; }

        [JsonProperty(PropertyName = "licenseRiskMediumCount")]
        public int LicenseRiskMediumCount { get; set; }

        [JsonProperty(PropertyName = "licenseRiskLowCount")]
        public int LicenseRiskLowCount { get; set; }

        [JsonProperty(PropertyName = "operationalRiskHighCount")]
        public int OperationalRiskHighCount { get; set; }

        [JsonProperty(PropertyName = "operationalRiskMediumCount")]
        public int OperationalRiskMediumCount { get; set; }

        [JsonProperty(PropertyName = "operationalRiskLowCount")]
        public int OperationalRiskLowCount { get; set; }
    }
}
