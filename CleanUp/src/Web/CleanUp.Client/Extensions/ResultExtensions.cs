using CleanUp.WebApi.Sdk.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CleanUp.Client.Extensions
{
    internal static class ResultExtensions
    {
        internal static async Task<WebApi.Sdk.Models.ApiResult<T>> ToResult<T>(this HttpResponseMessage response)
            where T : class
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<ApiResult<T>>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                Converters = 
                {
                    new JsonStringEnumConverter()
                }
            });
            return responseObject;
        }

        internal static async Task<ApiPaginatedResult<T>> ToPaginatedResult<T>(this HttpResponseMessage response)
            where T : class
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<ApiPaginatedResult<T>>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            });
            return responseObject;
        }

        internal static async Task<ApiResult> ToResult(this HttpResponseMessage response)
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<ApiResult>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            });
            return responseObject;
        }


        internal static T Convert<T>(JsonElement json)
        {
            var responseObject = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            });
            return responseObject;
        }

        
    }
}