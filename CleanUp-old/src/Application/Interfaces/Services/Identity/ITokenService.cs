using CleanUp.Application.Interfaces.Common;
using CleanUp.Application.Requests.Identity;
using CleanUp.Application.Responses.Identity;
using CleanUp.Shared.Wrapper;
using System.Threading.Tasks;

namespace CleanUp.Application.Interfaces.Services.Identity
{
    public interface ITokenService : IService
    {
        Task<Result<TokenResponse>> LoginAsync(TokenRequest model);

        Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
    }
}