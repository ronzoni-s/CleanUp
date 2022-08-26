using CleanUp.WebApi.Sdk.Models;
using CleanUp.Client.Models.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanUp.Client.Managers.Catalogs.Menus
{
    public interface IMenuManager : IManager
    {
        Task<ApiResult<List<Menu>>> GetAllAsync(int catalogId);

        Task<ApiResult<Menu>> GetByIdAsync(int catalogId, int menuId);

        Task<ApiResult<List<MenuCategory>>> GetCategorysAsync(int catalogId);
    }
}