using CleanUp.Client.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CleanUp.Client.Models.Api;
using CleanUp.WebApi.Sdk.Models;
using CleanUp.Client.Services.User;
using CleanUp.WebApi.Sdk.Endpoints;

namespace CleanUp.Client.Managers.Catalogs.Menus
{
    public class MenuManager : IMenuManager
    {
        private readonly HttpClient _httpClient;
        private readonly IUserService userService;

        public MenuManager(HttpClient httpClient, IUserService userService)
        {
            _httpClient = httpClient;
            this.userService = userService;
        }

        public async Task<ApiResult<List<Menu>>> GetAllAsync(int catalogId)
        {
            var response = await _httpClient.GetAsync(MenuEndpoints.GetAll(catalogId, await userService.GetAddressId()));
            return await response.ToResult<List<Menu>>();
        }

        public async Task<ApiResult<Menu>> GetByIdAsync(int catalogId, int menuId)
        {
            var response = await _httpClient.GetAsync(MenuEndpoints.GetById(catalogId, menuId, await userService.GetAddressId()));
            return await response.ToResult<Menu>();
        }

        public async Task<ApiResult<List<MenuCategory>>> GetCategorysAsync(int catalogId)
        {
            var response = await _httpClient.GetAsync(MenuEndpoints.GetCategorys(catalogId));
            return await response.ToResult<List<MenuCategory>>();
        }
    }
}