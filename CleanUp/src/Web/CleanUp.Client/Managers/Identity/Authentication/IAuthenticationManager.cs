using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Requests.User;
using CleanUp.Client.Models.Api;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanUp.Client.Managers.Identity.Authentication
{
    public interface IAuthenticationManager : IManager
    {
        Task<ApiResult<LoginResponse>> Login(TokenRequest model);

        Task Logout();

        Task<string> RefreshToken();

        Task<string> TryRefreshToken();

        Task<string> TryForceRefreshToken();

        Task<ClaimsPrincipal> CurrentUser();
    }
}