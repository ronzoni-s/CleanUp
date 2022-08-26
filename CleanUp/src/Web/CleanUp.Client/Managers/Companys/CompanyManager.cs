using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Endpoints;
using CleanUp.Client.Models.Api;
using CleanUp.Client.Extensions;

namespace CleanUp.Client.Managers.Companys
{
    public class CompanyManager : ICompanyManager
    {
        private readonly HttpClient _httpClient;

        public CompanyManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResult<Company>> GetByCatalog(int catalogId)
        {
            var response = await _httpClient.GetAsync(CompanyEndpoints.GetByCatalog(catalogId));
            return await response.ToResult<Company>();
        }

        public async Task<ApiResult<List<CompanyAddress>>> GetAddresss(int catalogId, bool onlyAvailable)
        {
            var response = await _httpClient.GetAsync(CompanyEndpoints.GetAddresss(catalogId, onlyAvailable));
            return await response.ToResult<List<CompanyAddress>>();
        }
    }
}