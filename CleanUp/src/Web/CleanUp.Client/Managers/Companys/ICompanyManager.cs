using CleanUp.Client.Models.Api;
using CleanUp.WebApi.Sdk.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanUp.Client.Managers.Companys
{
    public interface ICompanyManager : IManager
    {
        Task<ApiResult<List<CompanyAddress>>> GetAddresss(int catalogId, bool onlyAvailable);
        Task<ApiResult<Company>> GetByCatalog(int catalogId);
    }
}