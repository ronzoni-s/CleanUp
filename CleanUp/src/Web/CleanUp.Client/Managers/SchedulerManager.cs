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
    public class SchedulerManager : IManager
    {
        private readonly HttpClient _httpClient;

        public SchedulerManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResult<List<CleaningOperation>>> GetAllAsync(DateTime fromDate, DateTime toDate)
        {
            var response = await _httpClient.GetAsync(SchedulerEndpoints.GetAll(fromDate, toDate));
            return await response.ToResult<List<CleaningOperation>>();
        }

        public async Task<ApiResult> Schedule(DateTime date)
        {
            var response = await _httpClient.PostAsync(SchedulerEndpoints.Schedule(date), null);
            return await response.ToResult();
        }
    }
}