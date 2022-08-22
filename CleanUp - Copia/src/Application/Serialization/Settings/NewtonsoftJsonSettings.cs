
using ErbertPranzi.Application.Interfaces.Serialization.Settings;
using Newtonsoft.Json;

namespace ErbertPranzi.Application.Serialization.Settings
{
    public class NewtonsoftJsonSettings : IJsonSerializerSettings
    {
        public JsonSerializerSettings JsonSerializerSettings { get; } = new();
    }
}