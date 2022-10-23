using CleanUp.Client.Extensions;
using CleanUp.WebApi.Sdk.Endpoints;
using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Models.Events;
using CleanUp.WebApi.Sdk.Requests;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CleanUp.Client.Managers
{
    public class EventManager : IManager
    {
        private readonly HttpClient _httpClient;

        public EventManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResult<Event>> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync(EventEndpoints.Get(id));
            return await response.ToResult<Event>();
        }

        public async Task<ApiResult<List<Event>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(EventEndpoints.GetAll);
            return await response.ToResult<List<Event>>();
        }

        public async Task<ApiResult> UploadAsync(MultipartFormDataContent content)
        {
            var response = await _httpClient.PostAsync(EventEndpoints.Upload, content);
            return await response.ToResult();
        }

        public async Task<ApiResult<Event>> UpdateAsync(int id, UpdateEventRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync(EventEndpoints.Update(id), request);
            return await response.ToResult<Event>();
        }

        public async Task<ApiResult<Event>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(EventEndpoints.Delete(id));
            return await response.ToResult<Event>();
        }
    }
}