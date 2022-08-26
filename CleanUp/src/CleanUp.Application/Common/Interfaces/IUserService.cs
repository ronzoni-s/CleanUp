using CleanUp.Application.Common.Models;
using CleanUp.Application.Common.Requests;
using CleanUp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CleanUp.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<CleanUpUser> GetById(string userId);
        Task<List<CleanUpRole>> GetRolesAsync(string userId);
    }
}
