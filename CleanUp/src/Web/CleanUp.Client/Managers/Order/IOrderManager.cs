
using CleanUp.WebApi.Sdk.Models;
using CleanUp.Client.Models.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanUp.Client.Managers.Orders
{
    public interface IOrderManager : IManager
    {
        Task<ApiPaginatedResult<Order>> GetAllAsync(int pageSize, int pageNumber, DateTime? fromDate = null, DateTime? toDate = null, int? companyAddressId = null);
        Task<ApiResult<Order>> GetAsync(int id, int catalogId);
    }
}