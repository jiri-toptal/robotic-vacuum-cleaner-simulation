using Newtonsoft.Json;

namespace MyQ.Shared.Services.Abstractions
{
    public interface IJsonService
    {
        JsonSerializerSettings JsonSettings { get; }

        string SerializeObject(object obj);

        T Deserialize<T>(string value);
    }
}
