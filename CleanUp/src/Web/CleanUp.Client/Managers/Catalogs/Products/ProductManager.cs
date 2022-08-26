using CleanUp.Client.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CleanUp.Client.Models.Api;
using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Endpoints;
using CleanUp.Client.Services.User;

namespace CleanUp.Client.Managers.Catalogs.Products
{
    public class ProductManager : IProductManager
    {
        private readonly HttpClient _httpClient;
        private readonly IUserService userService;

        public ProductManager(HttpClient httpClient, IUserService userService)
        {
            _httpClient = httpClient;
            this.userService = userService;
        }

        public async Task<ApiResult<List<Product>>> GetAllAsync(int catalogId, DateTime couponsDate)
        {
            var response = await _httpClient.GetAsync(ProductsEndpoints.GetAll(catalogId, await userService.GetAddressId(), couponsDate));
            return await response.ToResult<List<Product>>();
        }
        
        public async Task<ApiResult<Product>> GetByIdAsync(int productId, int catalogId)
        {
            var response = await _httpClient.GetAsync(ProductsEndpoints.GetById(productId, catalogId, await userService.GetAddressId()));
            return await response.ToResult<Product>();
        }

        public async Task<ApiResult<List<Group>>> GetCategorysAsync()
        {
            var response = await _httpClient.GetAsync(ProductsEndpoints.GetCategorys());
            return await response.ToResult<List<Group>>();
        }
    }
}