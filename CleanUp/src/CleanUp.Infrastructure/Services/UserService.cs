using CleanUp.Application.Authorization;
using CleanUp.Application.Common.Requests;
using CleanUp.Application.Interfaces;
using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Application.Models;
using CleanUp.Application.Requests;
using CleanUp.Domain.Entities;
using fbognini.Core.Exceptions;
using fbognini.Notifications.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;
using RegisterRequest = CleanUp.Application.Requests.RegisterRequest;

namespace CleanUp.Infrastructure.Services
{
    public class UserService : IUserService
    {

        private readonly UserManager<CleanUpUser> userManager;
        private readonly RoleManager<CleanUpRole> roleManager;
        private readonly ILogger<UserService> logger;

        public UserService(
            UserManager<CleanUpUser> userManager
            , RoleManager<CleanUpRole> roleManager
            , ILogger<UserService> logger
            )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        public async Task<CleanUpUser> GetById(string userId)
        {
            return await userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
        }

        public async Task<List<CleanUpUser>> GetAll()
        {
            return await userManager.Users.ToListAsync();
        }

        public async Task<List<CleanUpRole>> GetRolesAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var roles = await roleManager.Roles.ToListAsync();

            var userRoles = new List<CleanUpRole>();

            foreach (var role in roles)
            {
                if (!await userManager.IsInRoleAsync(user, role.Name))
                {
                    continue;
                }

                userRoles.Add(role);
            }

            return userRoles;
        }

        public async Task<bool> IsInRole(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId);
            return await userManager.IsInRoleAsync(user, role);
        }

        public async Task<CleanUpUser> DeleteById(string userId)
        {
            var user = await GetById(userId);
            if (user == null)
                throw new NotFoundException($"Utente {userId} non trovato");

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new Exception($"Impossibile eliminare l'utente {userId}");

            return user;
        }

        public async Task<CleanUpUser> Register(RegisterRequest request)
        {
            //var user = await GetById(newUser.Id);
            //if (user == null)
            //    throw new NotFoundException($"Utente {userId} non trovato");

            var user = new CleanUpUser
            {
                Email = request.Email,
                EmailConfirmed = true,
                EmailConfirmationDate = DateTime.Now,
                FirstName = request.Name,
                LastName = request.Surname,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email,
            };
            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
                throw new Exception($"Impossibile creare un nuovo utente");

            await userManager.AddPasswordAsync(user, request.Password);

            return user;
        }

        public async Task<CleanUpUser> Update(UpdateUserRequest request)
        {
            var user = await GetById(request.Id);
            if (user == null)
                throw new NotFoundException($"Utente {request.Id} non trovato");

            user.FirstName = request.Name;
            user.LastName = request.Surname;
            user.PhoneNumber = request.PhoneNumber;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception($"Impossibile modificare l'utente {request.Id}");

            return user;
        }
    }
}
