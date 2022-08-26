using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using CleanUp.Client.Authentication;
using CleanUp.Client.Managers;
using CleanUp.Client.Managers.Orders;
using CleanUp.Client.Managers.Preferences;
using CleanUp.Client.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;
using System.Net.Http;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace CleanUp.Client.Extensions
{
    public static class WebAssemblyHostBuilderExtensions
    {
        private const string ClientName = "CleanUp.WebApi";

        public static WebAssemblyHostBuilder AddRootComponents(this WebAssemblyHostBuilder builder)
        {
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            return builder;
        }

        public static WebAssemblyHostBuilder AddClientServices(this WebAssemblyHostBuilder builder)
        {
            builder
                .Services
                //.AddLocalization(options =>
                //{
                //    options.ResourcesPath = "Resources";
                //})
                .AddAuthorizationCore(options =>
                {
                    options.AddPolicy("PortaleUser", policy => policy.RequireRole("B2B.Portale"));
                })
                .AddBlazoredLocalStorage()
                .AddScoped<ClientPreferenceManager>()
                .AddScoped<ClientStateProvider>()
                .AddScoped<AuthenticationStateProvider, ClientStateProvider>()
                .AddSweetAlert2()
                //.AddSingleton<B2CConfiguration>(builder.Configuration.GetSection("B2CConfiguration").Get<B2CConfiguration>())
                .AddManagers()
                .AddServices()
                //.AddExtendedAttributeManagers()
                .AddTransient<AuthenticationHeaderHandler>()
                .AddScoped(sp => sp
                    .GetRequiredService<IHttpClientFactory>()
                    .CreateClient(ClientName).EnableIntercept(sp))
                .AddHttpClient(ClientName, client =>
                {                  
                    client.DefaultRequestHeaders.AcceptLanguage.Clear();
                    client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
                    //client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                    client.BaseAddress = new Uri(builder.Configuration["ApiUrl"]);
                })
                .AddHttpMessageHandler<AuthenticationHeaderHandler>();

            builder.Services.AddHttpClientInterceptor();
            return builder;
        }

        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            var managers = typeof(IManager);

            var types = managers
                .Assembly
                .GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .Where(t => t.Service != null);

            foreach (var type in types)
            {
                if (managers.IsAssignableFrom(type.Service))
                {
                    services.AddTransient(type.Service, type.Implementation);
                }
            }

            services.AddScoped<ICartManager, CartManager>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICartService, CartService>();
            return services;
        }

        //private static void RegisterPermissionClaims(AuthorizationOptions options)
        //{
        //    foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
        //    {
        //        var propertyValue = prop.GetValue(null);
        //        if (propertyValue is not null)
        //        {
        //            options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.ToString()));
        //        }
        //    }
        //}
    }
}
