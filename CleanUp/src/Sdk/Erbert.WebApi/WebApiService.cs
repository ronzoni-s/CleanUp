using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Json;
using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Endpoints;
using CleanUp.WebApi.Sdk.Extensions;
using CleanUp.WebApi.Sdk.Requests;

namespace CleanUp.WebApi.Sdk
{
    public static class JsonSerializerUtils
    {
        public static JsonSerializerSettings WebApiJsonSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };
    }

    public class WebApiService
    {
        public HttpClient Client { get; }

        public WebApiService(HttpClient client)
        {
            //client.BaseAddress = new Uri(settings.PaymentBaseUrl);
            //client.DefaultRequestHeaders.Add("X-Client-Id", settings.PaymentClientId);
            //client.DefaultRequestHeaders.Add("X-Client-Secret", settings.PaymentClientSecret);

            Client = client;
        }

        
    }
    
}
