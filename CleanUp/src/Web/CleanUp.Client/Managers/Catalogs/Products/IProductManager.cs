using CleanUp.WebApi.Sdk.Models;
using CleanUp.Client.Models.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanUp.Client.Managers.Catalogs.Products
{
    public interface IProductManager : IManager
    {
        Task<ApiResult<List<Product>>> GetAllAsync(int catalogId , DateTime couponsDate);
        Task<ApiResult<Product>> GetByIdAsync(int productId, int catalogId);
        Task<ApiResult<List<Group>>> GetCategorysAsync();
    }
}