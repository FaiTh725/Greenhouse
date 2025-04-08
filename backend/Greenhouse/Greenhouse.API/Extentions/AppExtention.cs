using Application.Shared.Exceptions;

namespace Greenhouse.API.Extentions
{
    public static class AppExtention
    {
        public static IServiceCollection ConfigureAppServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.ConfigureCors(configuration);

            return services;
        }

        private static IServiceCollection ConfigureCors(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var clientUrl = configuration
                .GetValue<string>("ClientUrl") ??
                throw new AppConfigurationException("Client url");

            services.AddCors(options =>
            {
                options.AddPolicy("Client", policy =>
                {
                    policy
                        .WithOrigins(clientUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            return services;
        }
    }
}
