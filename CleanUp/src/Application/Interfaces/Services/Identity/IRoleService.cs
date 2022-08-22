using CleanUp.Application.Interfaces.Common;
using CleanUp.Application.Requests.Identity;
using CleanUp.Application.Responses.Identity;
using CleanUp.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanUp.Application.Interfaces.Services.Identity
{
    public interface IRoleService : IService
    {
        Task<Result<List<RoleResponse>>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<Result<RoleResponse>> GetByIdAsync(string id);

        Task<Result<string>> SaveAsync(RoleRequest request);

        Task<Result<string>> DeleteAsync(string id);

        Task<Result<PermissionResponse>> GetAllPermissionsAsync(string roleId);

        Task<Result<string>> UpdatePermissionsAsync(PermissionRequest request);
    }
}