using System.Collections.Generic;
using System.Threading.Tasks;
using CleanUp.Application.Interfaces.Common;
using CleanUp.Application.Requests.Identity;
using CleanUp.Application.Responses.Identity;
using CleanUp.Shared.Wrapper;

namespace CleanUp.Application.Interfaces.Services.Identity
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