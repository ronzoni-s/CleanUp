using CleanUp.Application.Common.Authorization;
using CleanUp.Domain.Entities;
using CleanUp.Infrastructure.Helpers;
using CleanUp.Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Infrastructure
{
    public interface IDatabaseSeeder
    {
        void Initialize();
    }

    public class InfrastructureDatabaseSeeder: IDatabaseSeeder
    {
        private readonly ILogger<InfrastructureDatabaseSeeder> logger;        
        private readonly CleanUpDbContext context;
        private readonly UserManager<CleanUpUser> userManager;
        private readonly RoleManager<CleanUpRole> roleManager;

        public InfrastructureDatabaseSeeder(
            UserManager<CleanUpUser> userManager
            , RoleManager<CleanUpRole> roleManager
            , ILogger<InfrastructureDatabaseSeeder> logger
            , CleanUpDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
            this.context = context;
        }

        public void Initialize()
        {
            AddAdministrator();
            AddBasicUser();

            context.SaveChanges();
        }

        private void AddAdministrator()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var adminRole = new CleanUpRole(RoleConstants.AdministratorRole);
                var adminRoleInDb = await roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                if (adminRoleInDb == null)
                {
                    await roleManager.CreateAsync(adminRole);
                    adminRoleInDb = await roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                    logger.LogInformation("Seeded Administrator Role.");
                }
                //Check if User Exists
                var superUser = new CleanUpUser
                {
                    FirstName = "Simone",
                    LastName = "Ronzoni",
                    Email = "sronzoni99@gmail.com",
                    UserName = "sronzoni99@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Created = DateTime.Now,
                };
                var superUserInDb = await userManager.FindByEmailAsync(superUser.Email);
                if (superUserInDb == null)
                {
                    await userManager.CreateAsync(superUser, "Password1");
                    var result = await userManager.AddToRoleAsync(superUser, RoleConstants.AdministratorRole);
                    if (result.Succeeded)
                    {
                        logger.LogInformation("Seeded Default SuperAdmin User.");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            logger.LogError(error.Description);
                        }
                    }
                }
                foreach (var permission in Permissions.GetRegisteredPermissions())
                {
                    await roleManager.AddPermissionClaim(adminRoleInDb, permission);
                }
            }).GetAwaiter().GetResult();
        }

        private void AddBasicUser()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var basicRole = new CleanUpRole(RoleConstants.BasicRole);
                var basicRoleInDb = await roleManager.FindByNameAsync(RoleConstants.BasicRole);
                if (basicRoleInDb == null)
                {
                    await roleManager.CreateAsync(basicRole);
                    logger.LogInformation("Seeded Basic Role.");
                }
                //Check if User Exists
                var basicUser = new CleanUpUser
                {
                    FirstName = "Mario",
                    LastName = "Rossi",
                    Email = "mariorossi@gmail.com",
                    UserName = "mariorossi@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Created = DateTime.Now,
                };
                var basicUserInDb = await userManager.FindByEmailAsync(basicUser.Email);
                if (basicUserInDb == null)
                {
                    await userManager.CreateAsync(basicUser, "Password1");
                    await userManager.AddToRoleAsync(basicUser, RoleConstants.BasicRole);
                    logger.LogInformation("Seeded User with Basic Role.");
                }
            }).GetAwaiter().GetResult();
        }

        //private void AddOrderStatuss()
        //{
        //    var orderStatuss = Enum.GetValues(typeof(Domain.Enums.OrderStatus)).Cast<Domain.Enums.OrderStatus>();
        //    foreach (var orderStatus in orderStatuss)
        //    {
        //        var orderStatusInDb = context.OrderStatuss.Find((int)orderStatus);
        //        if (orderStatusInDb == null)
        //        {
        //            context.OrderStatuss.Add(new Domain.Entities.OrderStatus()
        //            {
        //                Id = (int)orderStatus,
        //                Name = orderStatus.ToString()
        //            });
        //        }
        //    }
        //}

        //private void AddOrderSources()
        //{
        //    var orderSources = Enum.GetValues(typeof(Domain.Enums.OrderSource)).Cast<Domain.Enums.OrderSource>();
        //    foreach (var orderSource in orderSources)
        //    {
        //        var orderSourceInDb = context.OrderSources.Find((int)orderSource);
        //        if (orderSourceInDb == null)
        //        {
        //            context.OrderSources.Add(new Domain.Entities.OrderSource()
        //            {
        //                Id = (int)orderSource,
        //                Name = orderSource.ToString()
        //            });
        //        }
        //    }
        //}
    }

}
