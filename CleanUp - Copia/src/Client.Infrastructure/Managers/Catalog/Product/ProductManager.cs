﻿using ErbertPranzi.Application.Features.Products.Commands.AddEdit;
using ErbertPranzi.Application.Features.Products.Commands.Update;
using ErbertPranzi.Application.Features.Products.Queries.GetAllPaged;
using ErbertPranzi.Application.Features.Products.Queries.GetOrderProductsPaged;
using ErbertPranzi.Application.Requests.Catalog;
using ErbertPranzi.Client.Infrastructure.Extensions;
using ErbertPranzi.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ErbertPranzi.Client.Infrastructure.Managers.Catalog.Product
{
    public class ProductManager : IProductManager
    {
        private readonly HttpClient _httpClient;

        public ProductManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ProductsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.ProductsEndpoints.Export
                : Routes.ProductsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<string>> GetProductImageAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetProductImage(id));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedProductsResponse>> GetProductsAsync(GetAllPagedProductsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby, request.HideNotActive));
            return await response.ToPaginatedResult<GetAllPagedProductsResponse>();
        }

        public async Task<PaginatedResult<GetAllPagedOrderProductsResponse>> GetOrderProductsAsync(GetAllPagedOrderProductsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetOrderProductsPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby, request.OrderId));
            return await response.ToPaginatedResult<GetAllPagedOrderProductsResponse>();
        }

        public async Task<IResult<int>> SaveAsync(UpdateProductCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ProductsEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        //public async Task<IResult<int>> SaveAsync(AddEditProductCommand request)
        //{
        //    var response = await _httpClient.PostAsJsonAsync(Routes.ProductsEndpoints.Save, request);
        //    return await response.ToResult<int>();
        //}
    }
}