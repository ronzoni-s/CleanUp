using System.Text.Json;
using ErbertPranzi.Application.Interfaces.Serialization.Options;

namespace ErbertPranzi.Application.Serialization.Options
{
    public class SystemTextJsonOptions : IJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = new();
    }
}