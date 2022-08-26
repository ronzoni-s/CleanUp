using CleanUp.WebApi.Sdk.Models;
using CleanUp.Client.Models.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanUp.Client.Managers.Catalogs
{
    public interface ICatalogManager : IManager
    {
        Task<ApiResult<List<OrderDeliveryDate>>> GetAvailableOrderSlots();
        Task<Catalog> GetCatalog(bool reload = false);
    }
}