using CleanUp.Application.Interfaces;
using CleanUp.Application.Interfaces.Repositorys;
using CleanUp.Infrastructure.Persistance;
using CleanUp.Infrastructure.Repositorys;
using CleanUp.Infrastructure.Services;
using fbognini.Core.Interfaces;
using fbognini.i18n;
using fbognini.Infrastructure.Multitenancy;
using fbognini.Infrastructure.Persistence;
using fbognini.Infrastructure.Services;
using fbognini.Notifications;
using fbognini.Notifications.Interfaces;
using fbognini.Notifications.MTarget;
using fbognini.Notifications.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanUp.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfiguirations(configuration);

            services.AddI18N(configuration);

            services.AddMultitenancy(configuration)
                .WithDelegateStrategy(async context =>
                {
                    return await Task.FromResult<string>("root");
                });

            services
               .AddDistributedMemoryCache()
               .AddPersistence(configuration);

            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISerializerService, NewtonSoftService>();
            services.AddTransient<ISchedulerService, SortBasedSchedulerService>();
            services.AddTransient<IPushNotificationService, FirebasePushNotificationService>();

            services.AddNotifications(configuration);

            return services;
        }

        private static IServiceCollection AddConfiguirations(this IServiceCollection services, IConfiguration configuration)
        {
            // ...
            return services;
        }


        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(ICleanUpRepositoryAsync), typeof(CleanUpRepositoryAsync))
                ;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddPersistence<CleanUpDbContext, CleanUpTenant>(configuration)
                .AddTransient<IDatabaseSeeder, InfrastructureDatabaseSeeder>()
                .AddRepositories();
            
            services.AddDataProtection()
                .PersistKeysToDbContext<CleanUpDbContext>()
                .SetApplicationName("CleanUp.Identity");
            
            return services;
        }

        private static IServiceCollection AddNotifications(this IServiceCollection services, IConfiguration configuration)
        {
            var dbSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
            services.AddNotifications(options =>
                {
                    options.ConnectionString = dbSettings.ConnectionString;
                })
                .AddEmailService("EXCHANGE")
                .AddMTargetService("PROD");

            return services;
        }

        private static FinbuckleMultiTenantBuilder<CleanUpTenant> AddMultitenancy(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddMultitenancy<CleanUpTenant>(configuration);
        }
    }
}
