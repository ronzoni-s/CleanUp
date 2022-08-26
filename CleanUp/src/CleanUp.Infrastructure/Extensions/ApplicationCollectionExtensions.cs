
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanUp.Infrastructure.Extensions
{
    public static class ApplicationCollectionExtensions
    {
        public static IApplicationBuilder InitializeInfrastructureDatabase(this IApplicationBuilder app, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var initializers = serviceScope.ServiceProvider.GetServices<IDatabaseSeeder>();

            foreach (var initializer in initializers)
            {
                initializer.Initialize();
            }

            return app;
        }
    }
}
