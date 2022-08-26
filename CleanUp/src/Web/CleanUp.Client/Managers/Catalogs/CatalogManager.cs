using CleanUp.Client.Extensions;
using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Endpoints;
using CleanUp.Client.Models.Api;

namespace CleanUp.Client.Managers.Catalogs
{
    public class CatalogManager : ICatalogManager
    {
        private readonly HttpClient _httpClient;

        private Catalog catalog;

        public CatalogManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Catalog> GetCatalog(bool reload = false)
        {
            if (!reload && catalog != null)
            {
                return catalog;
            }
            catalog = null;
            var response = await _httpClient.GetAsync(CatalogEndpoints.Get);
            var obj = await response.ToResult<IList<Catalog>>();
            if (!obj.IsSuccess || obj.Response.Count != 1)
            {
                throw new Exception();
            }
            catalog = obj.Response.First();
            return catalog;
        }


        public async Task<ApiResult<List<OrderDeliveryDate>>> GetAvailableOrderSlots()
        {
            if (catalog == null)
            {
                await GetCatalog(true);
            }
            var response = await _httpClient.GetAsync(CatalogEndpoints.GetOrderSlots(catalog.Id));
            return await response.ToResult<List<OrderDeliveryDate>>();
        }
    }
}