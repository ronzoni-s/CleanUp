using CleanUp.Application.Requests;
using CleanUp.Application.Models;
using CleanUp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CleanUp.Application.Interfaces
{
    public interface IUserService
    {
        Task<CleanUpUser> DeleteById(string userId);
        Task<List<CleanUpUser>> GetAll();
        Task<CleanUpUser> GetById(string userId);
        Task<List<CleanUpRole>> GetRolesAsync(string userId);
        Task<bool> IsInRole(string userId, string role);
    }
}
