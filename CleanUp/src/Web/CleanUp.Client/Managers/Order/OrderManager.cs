using CleanUp.Client.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CleanUp.WebApi.Sdk.Models;
using CleanUp.Client.Models.Order;
using Blazored.LocalStorage;
using static CleanUp.Client.Constants.Storage.StorageConstants;
using System.Text.Json;
using CleanUp.WebApi.Sdk.Endpoints;
using System.Linq;
using CleanUp.WebApi.Sdk.Models.AdditionalData;
using CurrieTechnologies.Razor.SweetAlert2;
using CleanUp.Client.Models.Exceptions;
using CleanUp.Client.Models.Pages;
using CleanUp.Client.Models.Api;

namespace CleanUp.Client.Managers.Orders
{
    public class OrderManager : IOrderManager
    {
        private readonly HttpClient httpClient;

        public OrderManager(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ApiPaginatedResult<Order>> GetAllAsync(int pageSize, int pageNumber, DateTime? fromDate = null, DateTime? toDate = null, int? companyAddressId = null)
        {
            var response = await httpClient.GetAsync(OrderEndpoints.GetAllByCompany(pageSize, pageNumber, fromDate, toDate, companyAddressId));
            return await response.ToPaginatedResult<Order>();
        }

        public async Task<ApiResult<Order>> GetAsync(int id, int catalogId)
        {
            var response = await httpClient.GetAsync(OrderEndpoints.Get(id, catalogId));
            var result = await response.ToResult<Order>();

            if (result.IsSuccess)
            { 
                foreach (var childOrder in result.Response.ChildrenOrders)
                {
                    foreach (var op in childOrder.OrderProducts)
                    {
                        op.Product = result.Response.OrderProducts.First(x => x.ProductId == op.ProductId).Product;
                    }
                }
            }

            return result;
        }
    }
}