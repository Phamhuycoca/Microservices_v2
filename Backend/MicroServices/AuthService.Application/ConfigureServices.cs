using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AuthService.Infrastructure;
using AutoMapper;
using AuthService.Application.IServices;
using AuthService.Application.Services;

namespace AuthService.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(assembly);
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IAuthenService, AuthenService>();
            services.AddInfrastructureModule();
            return services;
        }
    }
}
