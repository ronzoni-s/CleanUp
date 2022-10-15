using CleanUp.Application.Models;
using CleanUp.Application.Requests;
using CleanUp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CleanUp.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> GetRefreshTokenAsync(RefreshLoginRequest model);
        Task<LoginResponse> LoginAsync(LoginRequest model);
    }
}
