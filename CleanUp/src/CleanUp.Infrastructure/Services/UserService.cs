﻿using CleanUp.Application.Common.Interfaces;
using CleanUp.Application.Common.Interfaces.Repositorys;
using CleanUp.Application.Common.Models;
using CleanUp.Application.Common.Requests;
using CleanUp.Domain.Entities;
using fbognini.Core.Exceptions;
using fbognini.Notifications.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;
using RegisterRequest = CleanUp.Application.Common.Requests.RegisterRequest;

namespace CleanUp.Infrastructure.Services
{
    public class UserService : IUserService
    {

        private readonly UserManager<CleanUpUser> userManager;
        private readonly RoleManager<CleanUpRole> roleManager;
        private readonly ICleanUpRepositoryAsync repository;
        private readonly IEmailService emailService;
        private readonly ILogger<UserService> logger;

        public UserService(
            UserManager<CleanUpUser> userManager
            , RoleManager<CleanUpRole> roleManager
            , IEmailService emailService
            , ILogger<UserService> logger
            , ICleanUpRepositoryAsync repository
            )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.emailService = emailService;
            this.logger = logger;
            this.repository = repository;
        }

        public async Task<CleanUpUser> GetById(string userId)
        {
            return await userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
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
    }
}