using Application.Shared.Exceptions;

namespace Report.API.Extention
{
    public static class AppExtention
    {
        public static IServiceCollection ConfigureAPIServices(
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
