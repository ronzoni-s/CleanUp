using CleanUp.WebApi.Sdk;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CleanUp.Hangfire.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration configuration, List<string> queues)
        {
            services.Configure<HostOptions>(option =>
            {
                option.ShutdownTimeout = TimeSpan.FromSeconds(60);
            });

            services.TryAddSingleton<SqlServerStorageOptions>(new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.FromTicks(1),
                UseRecommendedIsolationLevel = true,
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(1)
            });

            services.AddHangfire((provider, hangfireConfiguration) => hangfireConfiguration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(
                    configuration.GetConnectionString("DefaultConnection"),
                    provider.GetRequiredService<SqlServerStorageOptions>()
                ));

            services.AddSingleton(configuration);

            services.AddHangfireServer(options =>
            {
                options.Queues = queues.ToArray();
            });

            return services;
        }

        public static IServiceCollection AddWebApiService(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("WebApiSettings").Get<WebApiSettings>();
            services.AddHttpClient<WebApiService>(c =>
            {
                c.BaseAddress = new Uri(settings.BaseUrl);
            });

            return services;
        }
    }
}
