using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            JValue obj = (JValue) JToken.Load(reader);
            //T riskCountEnum = new T((string)obj);
            string[] args = { (string)obj };
            T hubEnum = (T)Activator.CreateInstance(typeof(T), args);
            return hubEnum;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
