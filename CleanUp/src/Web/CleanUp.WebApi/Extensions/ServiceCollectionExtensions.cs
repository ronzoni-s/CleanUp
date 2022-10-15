using CleanUp.Application.Models;
using CleanUp.Application.WebApi;
using CleanUp.Infrastructure.Extensions;
using CleanUp.Domain.Entities;
using CleanUp.Infrastructure.Persistance;
using CleanUp.WebApi.Authorization;
using CleanUp.WebApi.Services;
using fbognini.Core.Interfaces;
using fbognini.WebFramework.OpenApi;
using fbognini.WebFramework.Transformers;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using CleanUp.Application;
using CleanUp.Application.Authorization;

namespace CleanUp.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddInfrastructureWebApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructure(configuration);

            services
                .AddApiVersioning()
                .AddAuth(configuration)
                .AddOpenApiDocumentation(configuration)
                .AddCorsPolicy();

            
            services.AddControllers(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .AddFluentValidation();
            ;


            return services;
        }

        private static IServiceCollection AddApiVersioning(this IServiceCollection services) =>
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });

        private static IServiceCollection AddCorsPolicy(this IServiceCollection services) =>
            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

        private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity();
            services.AddJwtAuthentication(configuration);
            services.AddCurrentUserService();

            return services;
        }

        private static IServiceCollection AddCurrentUserService(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }

        private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection("AuthenticationSettings");
            services.Configure<AuthenticationSettings>(config);
            AuthenticationSettings settings = new AuthenticationSettings();
            config.Bind(settings);

            var key = Encoding.ASCII.GetBytes(settings.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer("Bearer", async bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RoleClaimType = ClaimTypes.Role,
                        ClockSkew = TimeSpan.Zero
                    };

                    bearer.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            if (!context.Response.HasStarted)
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                context.Response.ContentType = "application/json";
                                var result = JsonConvert.SerializeObject("You are not Authorized.");
                                return context.Response.WriteAsync(result);
                            }

                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            if (!context.Response.HasStarted)
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                                context.Response.ContentType = "application/json";
                                var result = JsonConvert.SerializeObject("You are not authorized to access this resource.");
                                return context.Response.WriteAsync(result);
                            }
                            return Task.CompletedTask;

                        },
                    };
                });
            services.AddAuthorization(options =>
            {
                    // Here I stored necessary permissions/roles in a constant
                    foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
                {
                    var propertyValue = prop.GetValue(null);
                    if (propertyValue is not null)
                    {
                        options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(CustomClaimTypes.Permission, propertyValue.ToString()));
                    }
                }
            });

            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }


        internal static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                //.AddScoped<IClaimsTransformation, ClaimsTransformer>()
                .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>()
                .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
                .AddIdentity<CleanUpUser, CleanUpRole>(options =>
                {
                    options.User.AllowedUserNameCharacters += "\\àèìòù";
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedEmail = true;
                    options.SignIn.RequireConfirmedPhoneNumber = false;

                    // Password settings
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<CleanUpDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
