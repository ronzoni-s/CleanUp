using CleanUp.Client.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CleanUp.WebApi.Sdk.Models;
using Blazored.LocalStorage;
using static CleanUp.Client.Constants.Storage.StorageConstants;
using System.Text.Json;
using CleanUp.WebApi.Sdk.Endpoints;
using System.Linq;
using CleanUp.WebApi.Sdk.Models.AdditionalData;
using CleanUp.Client.Services.User;
using CurrieTechnologies.Razor.SweetAlert2;
using CleanUp.Client.Models.Order;
using CleanUp.Client.Models.Pages;
using CleanUp.Client.Models.Api;

namespace CleanUp.Client.Managers.Orders
{
    public static class CartConstants
    {
        public static int Version = 1;
    }

    public class CartManager : ICartManager
    {

        private readonly HttpClient httpClient;

        public CartManager(
            HttpClient httpClient
            )
        {
            this.httpClient = httpClient;
        }

        public async Task<ApiResult<PriceOrder>> PriceOrders(PriceOrdersRequest request)
        {
            var response = await httpClient.PostAsJsonAsync(OrderEndpoints.PriceMulti, request);
            return await response.ToResult<PriceOrder>();
        }

        public async Task<ApiResult<PriceOrder>> PlaceOrders(PlaceOrdersRequest request)
        {
            var response = await httpClient.PostAsJsonAsync(OrderEndpoints.PlaceMulti, request);
            return await response.ToResult<PriceOrder>();
        }

        public async Task<ApiResult<PriceOrder>> EditOrders(int orderId, EditOrdersRequest request)
        {
            var response = await httpClient.PostAsJsonAsync(OrderEndpoints.EditMulti(orderId), request);
            return await response.ToResult<PriceOrder>();
        }
    }
}