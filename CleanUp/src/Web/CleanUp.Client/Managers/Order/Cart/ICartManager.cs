using CleanUp.Client.Models.Api;
using CleanUp.Client.Models.Order;
using CleanUp.Client.Models.Pages;
using CleanUp.WebApi.Sdk.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanUp.Client.Managers.Orders
{
    public interface ICartManager
    {
        Task<ApiResult<PriceOrder>> EditOrders(int orderId, EditOrdersRequest request);
        Task<ApiResult<PriceOrder>> PlaceOrders(PlaceOrdersRequest request);
        Task<ApiResult<PriceOrder>> PriceOrders(PriceOrdersRequest request);
    }
}