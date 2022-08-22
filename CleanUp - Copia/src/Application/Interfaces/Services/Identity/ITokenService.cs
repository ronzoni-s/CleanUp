using ErbertPranzi.Application.Interfaces.Common;
using ErbertPranzi.Application.Requests.Identity;
using ErbertPranzi.Application.Responses.Identity;
using ErbertPranzi.Shared.Wrapper;
using System.Threading.Tasks;

namespace ErbertPranzi.Application.Interfaces.Services.Identity
{
    public interface ITokenService : IService
    {
        Task<Result<TokenResponse>> LoginAsync(TokenRequest model);

        Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
    }
}