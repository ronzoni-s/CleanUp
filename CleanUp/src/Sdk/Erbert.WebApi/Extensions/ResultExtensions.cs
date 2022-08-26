using CleanUp.WebApi.Sdk.Models;
using Newtonsoft.Json;

namespace CleanUp.WebApi.Sdk.Extensions
{
    internal static class ResultExtensions
    {
        internal static async Task<T> ToResult<T>(this HttpResponseMessage response, JsonSerializerSettings serializerSettings)
            //where T : class
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<T>(responseAsString, serializerSettings);
            return responseObject;
        }

        internal static async Task<ApiResult> ToResult(this HttpResponseMessage response, JsonSerializerSettings serializerSettings)
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ApiResult>(responseAsString, serializerSettings);
            return responseObject;
        }
    }
}