using System.Collections.Generic;
using System.Threading.Tasks;
using ErbertPranzi.Application.Interfaces.Common;
using ErbertPranzi.Application.Requests.Identity;
using ErbertPranzi.Application.Responses.Identity;
using ErbertPranzi.Shared.Wrapper;

namespace ErbertPranzi.Application.Interfaces.Services.Identity
{
    public interface IRoleClaimService : IService
    {
        Task<Result<List<RoleClaimResponse>>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<Result<RoleClaimResponse>> GetByIdAsync(int id);

        Task<Result<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId);

        Task<Result<string>> SaveAsync(RoleClaimRequest request);

        Task<Result<string>> DeleteAsync(int id);
    }
}