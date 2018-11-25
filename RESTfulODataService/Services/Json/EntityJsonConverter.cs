using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace api.emoney.com.Core.Services.Json
{
    public abstract class EntityJsonConverter<T> : JsonConverter
    {
        public readonly JsonSerializer serializer;

        public EntityJsonConverter()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter(),
                }
            };
            
            serializer = JsonSerializer.Create(settings);
        }

        public T GetJObject(object obj, out JObject jObjectResult)
        {
            if (obj == null)
            {
                throw new NullReferenceException(nameof(obj) + " is null.");
            }

            var castResult = (T)obj;

            jObjectResult = JObject.FromObject(castResult, this.serializer);

            return castResult;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => null;

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType) => objectType.Equals(typeof(T));
    }
}