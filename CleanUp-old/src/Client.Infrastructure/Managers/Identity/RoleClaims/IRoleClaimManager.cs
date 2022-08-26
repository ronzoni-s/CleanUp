using System.Collections.Generic;
using System.Threading.Tasks;
using CleanUp.Application.Requests.Identity;
using CleanUp.Application.Responses.Identity;
using CleanUp.Shared.Wrapper;

namespace CleanUp.Client.Infrastructure.Managers.Identity.RoleClaims
{
    public interface IRoleClaimManager : IManager
    {
        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsAsync();

        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsByRoleIdAsync(string roleId);

        Task<IResult<string>> SaveAsync(RoleClaimRequest role);

        Task<IResult<string>> DeleteAsync(string id);
    }
}