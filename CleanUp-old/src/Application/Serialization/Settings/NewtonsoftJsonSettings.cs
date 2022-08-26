
using CleanUp.Application.Interfaces.Serialization.Settings;
using Newtonsoft.Json;

namespace CleanUp.Application.Serialization.Settings
{
    public class NewtonsoftJsonSettings : IJsonSerializerSettings
    {
        public JsonSerializerSettings JsonSerializerSettings { get; } = new();
    }
}