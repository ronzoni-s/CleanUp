using CleanUp.Application.Features.Orders.Commands;
using CleanUp.Application.Features.Orders.Commands.AddEdit;
using CleanUp.Application.Features.Orders.Commands.Complete;
using CleanUp.Application.Features.Orders.Commands.Void;
using CleanUp.Application.Features.Orders.Queries.GetAllPaged;
using CleanUp.Application.Features.Orders.Queries.GetById;
using CleanUp.Application.Requests.Catalog;
using CleanUp.Client.Infrastructure.Extensions;
using CleanUp.Shared.Wrapper;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CleanUp.Client.Infrastructure.Managers.Catalog.Order
{
    public class OrderManager : IOrderManager
    {
        private readonly HttpClient _httpClient;

        public OrderManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.OrdersEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.OrdersEndpoints.Export
                : Routes.OrdersEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        //public async Task<IResult<string>> GetOrderImageAsync(int id)
        //{
        //    var response = await _httpClient.GetAsync(Routes.OrdersEndpoints.GetOrderImage(id));
        //    return await response.ToResult<string>();
        //}

        public async Task<PaginatedResult<GetAllPagedOrdersResponse>> GetOrdersAsync(GetAllPagedOrdersRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.OrdersEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby, request.HideCompleted, request.HideVoided));
            return await response.ToPaginatedResult<GetAllPagedOrdersResponse>();
        }

        public async Task<IResult<GetOrderByIdResponse>> GetOrderByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.OrdersEndpoints.GetById(id));
            var result = await response.ToResult<GetOrderByIdResponse>();
            return result;
        }

        public async Task<IResult<int>> GetNextOrderId(int id)
        {
            var response = await _httpClient.GetAsync(Routes.OrdersEndpoints.GetNextOrderId(id));
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> GetPreviousOrderId(int id)
        {
            var response = await _httpClient.GetAsync(Routes.OrdersEndpoints.GetPreviousOrderId(id));
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> SaveAsync(AddOrderCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.OrdersEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult> PrintPoliboxLabelAsync(PrintPoliboxLabelCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.OrdersEndpoints.PrintPoliboxLabel, request);
            return await response.ToResult();
        }

        public async Task<IResult> PrintProductsAsync()
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.OrdersEndpoints.PrintProducts, "");
            return await response.ToResult();
        }

        public async Task<IResult<int>> CompleteAsync(CompleteOrderCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.OrdersEndpoints.Complete, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> VoidAsync(VoidOrderCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.OrdersEndpoints.Void, request);
            return await response.ToResult<int>();
        }

    }
} 