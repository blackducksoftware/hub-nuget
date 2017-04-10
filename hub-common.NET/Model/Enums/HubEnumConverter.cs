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
using Newtonsoft.Json.Linq;
using System;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums
{
    class HubEnumConverter<T> : JsonConverter where T : HubEnum, new()
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(RiskCountEnum);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JValue obj = (JValue)JToken.Load(reader);
            string[] args = { (string)obj };
            T hubEnum = (T)Activator.CreateInstance(typeof(T), args);
            return hubEnum;
        }

        public override bool CanWrite => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            HubEnum hubEnum = value as HubEnum;
            string enumValue = hubEnum.ToString();
            serializer.Serialize(writer, enumValue, typeof(string));
        }
    }
}
