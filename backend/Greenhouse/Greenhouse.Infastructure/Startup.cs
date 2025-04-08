using Application.Shared.Exceptions;
using Greenhouse.Application.Common.Intrefaces;
using Greenhouse.Application.Consumers.EmploeeConsumers;
using Greenhouse.Application.Consumers.EventConsumers;
using Greenhouse.Application.Consumers.EventResource;
using Greenhouse.Application.Consumers.HarvestResource;
using Greenhouse.Application.Contracts.Employe;
using Greenhouse.Infastructure.Configurations;
using Greenhouse.Infastructure.Implementations;
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text;

namespace Greenhouse.Infastructure
{
    public static class Startup
    {
        public static IServiceCollection ConfigureInfastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .ConfigureMasstansit(configuration)
                .ConfigureJwtAuth(configuration)
                .AddHangfireProvider(configuration)
                .AddRedisCacheProvider(configuration);

            services.AddScoped<IBackgroundService, BackgorundService>();
            services.AddSingleton<IJwtTokenService<EmployeToken>, JwtTokenService>();
            services.AddSingleton<ICacheService, CacheService>();

            return services;
        }

        private static IServiceCollection ConfigureMasstansit(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var rabbitMqConf = configuration.GetSection("Rabbitmq")
                .Get<RabbitMqConf>() ?? 
                throw new AppConfigurationException("RabbitMq configuration");

            services.AddMassTransit(conf =>
            {
                conf.SetKebabCaseEndpointNameFormatter();

                conf.AddConsumer<CreateEmploeeConsumer>();
                conf.AddConsumer<AddJobNotificationByEventConsumer>();
                conf.AddConsumer<CreateHarvestResourceConsumer>();
                conf.AddConsumer<SpendEventResourcesConsumer>();

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
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings")
                .Get<JwtConf>() ??
                throw new AppConfigurationException("Jwt token section");

            services.AddAuthentication()
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

            services.AddAuthorization();

            return services;
        }

        private static IServiceCollection AddHangfireProvider(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var bdConnectionString = configuration
                .GetConnectionString("NpgConnection") ?? 
                throw new AppConfigurationException("Connection string to database");

            var jsonSerializeSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            services.AddHangfire(x =>
            {
                x.UseSimpleAssemblyNameTypeSerializer()
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UsePostgreSqlStorage(x => x.UseNpgsqlConnection(bdConnectionString))
                .UseSerializerSettings(jsonSerializeSettings);
            });

            services.AddHangfireServer();

            return services;
        }

        private static IServiceCollection AddRedisCacheProvider(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var redisConnection = configuration
                .GetConnectionString("RedisConnection") ??
                throw new AppConfigurationException("Redis Connection");

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnection;
                options.InstanceName = "Greenhouse";
            });

            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(redisConnection));

            return services;
        }
    }
}
