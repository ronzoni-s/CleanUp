using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using CleanUp.Application.Interfaces.Repositories;
using CleanUp.Application.Interfaces.Services.Storage;
using CleanUp.Application.Interfaces.Services.Storage.Provider;
using CleanUp.Application.Interfaces.Serialization.Serializers;
using CleanUp.Application.Serialization.JsonConverters;
using CleanUp.Infrastructure.Repositories;
using CleanUp.Infrastructure.Services.Storage;
using CleanUp.Application.Serialization.Options;
using CleanUp.Infrastructure.Services.Storage.Provider;
using CleanUp.Application.Serialization.Serializers;

namespace CleanUp.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>))
                .AddTransient<IProductRepository, ProductRepository>()
                .AddTransient<IOrderRepository, OrderRepository>()
                .AddTransient<IParameterRepository, ParameterRepository>()
                .AddTransient<IReceiptEodRepository, ReceiptEodRepository>()
                .AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        }

        public static IServiceCollection AddExtendedAttributesUnitOfWork(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IExtendedAttributeUnitOfWork<,,>), typeof(ExtendedAttributeUnitOfWork<,,>));
        }

        public static IServiceCollection AddServerStorage(this IServiceCollection services)
            => AddServerStorage(services, null);

        public static IServiceCollection AddServerStorage(this IServiceCollection services, Action<SystemTextJsonOptions> configure)
        {
            return services
                .AddScoped<IJsonSerializer, SystemTextJsonSerializer>()
                .AddScoped<IStorageProvider, ServerStorageProvider>()
                .AddScoped<IServerStorageService, ServerStorageService>()
                .AddScoped<ISyncServerStorageService, ServerStorageService>()
                .Configure<SystemTextJsonOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    if (!configureOptions.JsonSerializerOptions.Converters.Any(c => c.GetType() == typeof(TimespanJsonConverter)))
                        configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
        }
    }
}