using CleanUp.Application.Common.Models;
using CleanUp.Application.Common.Requests;
using CleanUp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CleanUp.Application.Common.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> GetRefreshTokenAsync(RefreshLoginRequest model);
        Task<LoginResponse> LoginAsync(LoginRequest model);
    }
}
