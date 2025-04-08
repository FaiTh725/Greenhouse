
using Application.Shared.Exceptions;
using Authorize.Application.Common.Intefaces;
using Authorize.Application.Contracts.JwtToken;
using Authorize.Application.Contracts.User;
using Authorize.Infastructure.BackgroundServices;
using Authorize.Infastructure.Configurations;
using Authorize.Infastructure.Implementations;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace Authorize.Infastructure
{
    public static class Startup
    {
        public static IServiceCollection ConfigureInfastructureServices(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services
                .AddCacheProvider(configuration)
                .ConfigureMassTransit(configuration)
                .ConfigureJwtAuth(configuration);

            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IJwtService<UserResponse, TokenResponse>, JwtService>();

            services.AddHostedService<InitializeRolesBackgroundService>();

            return services;
        }

        private static IServiceCollection AddCacheProvider(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var cacheConnectionString = configuration
                .GetConnectionString("RedisCacheConnection") ??
                throw new AppConfigurationException("Redis connection string");

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cacheConnectionString;
                options.InstanceName = "GreenhouseAuth";
            });

            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(cacheConnectionString));

            return services;
        }

        private static IServiceCollection ConfigureMassTransit(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var rabbitMqConf = configuration.GetSection("RabbitMq")
                .Get<RabbitMqConf>() ??
                throw new AppConfigurationException("RabbitMq configuration");

            services.AddMassTransit(conf =>
            {
                conf.SetKebabCaseEndpointNameFormatter();

                conf.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(rabbitMqConf.Host, h =>
                    {
                        h.Username(rabbitMqConf.UserLogin);
                        h.Password(rabbitMqConf.UserPassword);
                    });

                    configurator.ConfigureEndpoints(context);
                });
            });

            return services;
        }

        private static IServiceCollection ConfigureJwtAuth(
            this IServiceCollection service,
            IConfiguration configuration)

        {
            var jwtSettings = configuration.GetSection("JwtSettings")
                .Get<JwtConf>() ??
                throw new AppConfigurationException("Jwt token section");

            service.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, 
                jwtOptions =>
                {
                    jwtOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidIssuer = jwtSettings.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(jwtSettings.SecretKey))
                    };

                    jwtOptions.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            var token = ctx.Request.Cookies["token"];

                            if (!string.IsNullOrEmpty(token))
                            {
                                ctx.Token = token;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            service.AddAuthorization();

            return service;
        }
    }
}
