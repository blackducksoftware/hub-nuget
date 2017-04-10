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
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums
{
    public sealed class ScanStatusEnum : HubEnum
    {
        public static readonly ScanStatusEnum UNSTARTED = new ScanStatusEnum("UNSTARTED");
        public static readonly ScanStatusEnum SCANNING = new ScanStatusEnum("SCANNING");
        public static readonly ScanStatusEnum SAVING_SCAN_DATA = new ScanStatusEnum("SAVING_SCAN_DATA");
        public static readonly ScanStatusEnum SCAN_DATA_SAVE_COMPLETE = new ScanStatusEnum("SCAN_DATA_SAVE_COMPLETE");
        public static readonly ScanStatusEnum REQUESTED_MATCH_JOB = new ScanStatusEnum("REQUESTED_MATCH_JOB");
        public static readonly ScanStatusEnum MATCHING = new ScanStatusEnum("MATCHING");
        public static readonly ScanStatusEnum BOM_VERSION_CHECK = new ScanStatusEnum("BOM_VERSION_CHECK");
        public static readonly ScanStatusEnum BUILDING_BOM = new ScanStatusEnum("BUILDING_BOM");
        public static readonly ScanStatusEnum COMPLETE = new ScanStatusEnum("COMPLETE");
        public static readonly ScanStatusEnum CANCELLED = new ScanStatusEnum("CANCELLED");
        public static readonly ScanStatusEnum CLONED = new ScanStatusEnum("CLONED");
        public static readonly ScanStatusEnum ERROR_SCANNING = new ScanStatusEnum("ERROR_SCANNING");
        public static readonly ScanStatusEnum ERROR_SAVING_SCAN_DATA = new ScanStatusEnum("ERROR_SAVING_SCAN_DATA");
        public static readonly ScanStatusEnum ERROR_MATCHING = new ScanStatusEnum("ERROR_MATCHING");
        public static readonly ScanStatusEnum ERROR_BUILDING_BOM = new ScanStatusEnum("ERROR_BUILDING_BOM");
        public static readonly ScanStatusEnum ERROR = new ScanStatusEnum("ERROR");

        public ScanStatusEnum()
        {

        }

        public ScanStatusEnum(string value) : base(value)
        {
        }
    }
}
