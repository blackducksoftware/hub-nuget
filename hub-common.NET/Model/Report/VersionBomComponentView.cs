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
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Report
{
    public class VersionBomComponentView : HubView
    {
        [JsonProperty(PropertyName = "activityData")]
        public ActivityDataView ActivityData { get; set; }

        [JsonProperty(PropertyName = "activityRiskProfile")]
        public RiskProfileView RiskProfile { get; set; }

        [JsonProperty(PropertyName = "component")]
        public string Component { get; set; }

        [JsonProperty(PropertyName = "componentName")]
        public string ComponentName { get; set; }

        [JsonProperty(PropertyName = "componentVersion")]
        public string ComponentVersion { get; set; }

        [JsonProperty(PropertyName = "componentVersionName")]
        public string ComponentVersionName { get; set; }

        [JsonProperty(PropertyName = "licenseRiskProfile")]
        public RiskProfileView LicenseRiskProfile { get; set; }

        [JsonProperty(PropertyName = "licenses")]
        public List<VersionBomLicenseView> Licenses { get; set; }

        [JsonProperty(PropertyName = "operationalRiskProfile")]
        public RiskProfileView OperationalRiskProfile { get; set; }

        [JsonProperty(PropertyName = "releasedOn")]
        public string ReleasedOn { get; set; }

        [JsonProperty(PropertyName = "securityRiskProfile")]
        public RiskProfileView SecurityRiskProfile { get; set; }

        [JsonProperty(PropertyName = "versionRiskProfile")]
        public RiskProfileView VersionRiskProfile { get; set; }
    }
}
