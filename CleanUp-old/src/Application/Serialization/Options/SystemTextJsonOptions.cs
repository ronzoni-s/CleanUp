using System.Text.Json;
using CleanUp.Application.Interfaces.Serialization.Options;

namespace CleanUp.Application.Serialization.Options
{
    public class SystemTextJsonOptions : IJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = new();
    }
}