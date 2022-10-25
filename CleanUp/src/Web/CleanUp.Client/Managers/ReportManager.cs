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
    public class ReportManager : IManager
    {
        private readonly HttpClient _httpClient;

        public ReportManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<byte[]> GetCleaningOperations(DateTime fromDate, DateTime toDate)
        {
            return await _httpClient.GetByteArrayAsync(ReportEndpoints.GetCleaningOperations(fromDate, toDate));
        }
    }
}