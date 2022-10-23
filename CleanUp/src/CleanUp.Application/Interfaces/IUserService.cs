using CleanUp.Application.Requests;
using CleanUp.Application.Models;
using CleanUp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using CleanUp.Application.Common.Requests;

namespace CleanUp.Application.Interfaces
{
    public interface  IUserService
    {
        Task<CleanUpUser> DeleteById(string userId);
        Task<List<CleanUpUser>> GetAll(string role = null);
        Task<CleanUpUser> GetById(string userId);
        Task<List<CleanUpRole>> GetRolesAsync(string userId);
        Task<CleanUpUser> Register(RegisterRequest request);
        Task<CleanUpUser> Update(UpdateUserRequest request);
        Task<bool> IsInRole(string userId, string role);
    }
}
