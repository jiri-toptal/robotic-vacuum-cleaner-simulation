using MyQ.Shared.Services.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyQ.Shared.Services
{
    public class JsonService : IJsonService
    {
        public JsonSerializerSettings JsonSettings => new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        public string SerializeObject(object obj) => JsonConvert.SerializeObject(obj, JsonSettings);

        public T Deserialize<T>(string value) => JsonConvert.DeserializeObject<T>(value);
    }
}
