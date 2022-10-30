using CleanUp.Client.Extensions;
using CleanUp.WebApi.Sdk.Endpoints;
using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Requests;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CleanUp.Client.Managers
{
    public class ClassroomManager : IManager
    {
        private readonly HttpClient _httpClient;

        public ClassroomManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResult<List<Classroom>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(ClassroomEndpoints.GetAll);
            return await response.ToResult<List<Classroom>>();
        }
    }
}