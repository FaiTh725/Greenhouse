using Application.Shared.Exceptions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Common.Interfaces;
using Notification.Application.Consumers.Email;
using Notification.Contracts.Email;
using Notification.Infastructure.Configurations;
using Notification.Infastructure.Implementations;

namespace Notification.Infastructure
{
    public static class Startup
    {
        public static IServiceCollection ConfigureInfastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.ConfigureMassTransit(configuration);

            services.AddScoped<INotificationService<SendEmailRequest>, SendEmailService>();

            return services;
        }

        private static IServiceCollection ConfigureMassTransit(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var rabbitMqConf = configuration.GetSection("RabbitMqSetting")
                .Get<RabbitMqConf>() ??
                throw new AppConfigurationException("");

            services.AddMassTransit(conf =>
            {
                conf.SetKebabCaseEndpointNameFormatter();

                conf.AddConsumer<SentEmailConsumer>();

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
    }
}
