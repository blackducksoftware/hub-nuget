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
