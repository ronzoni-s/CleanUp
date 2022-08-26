
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using MediatR;
using AutoMapper;
using fbognini.Application.DependencyInjection;

namespace CleanUp.Application.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationWebApi(this IServiceCollection services)
        {
            services.AddApplication();
            services.AddApplication(Assembly.GetExecutingAssembly());

            return services;
        }

    }
}
