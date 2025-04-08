using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Report.Application
{
    public static class Startup
    {
        public static IServiceCollection ConfigureAppServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMediatorProvider();

            return services;
        }

        private static IServiceCollection AddMediatorProvider(
            this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly);
            });

            return services;
        }
    }
}
