namespace Authorize.API.Extentions
{
    public static class AppExtention
    {
        public static IServiceCollection ConfigureApi(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.ConfigureCorses(configuration);

            return services;
        }

        private static IServiceCollection ConfigureCorses(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var clientUrl = configuration.GetValue<string>("ClientUrl") ?? "";
            
            services.AddCors(options =>
            {
                options.AddPolicy("Client", policy =>
                {
                    policy
                        .WithOrigins(clientUrl)
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
