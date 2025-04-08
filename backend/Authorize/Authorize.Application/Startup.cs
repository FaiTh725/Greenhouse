using Authorize.Application.Common.Implementations;
using Authorize.Application.Common.Intefaces;
using Microsoft.Extensions.DependencyInjection;

namespace Authorize.Application
{
    public static class Startup
    {
        public static IServiceCollection ConfigureAppServices(
            this IServiceCollection services)
        {
            services.ConfigureMediatR();

            services.AddSingleton<IHashService, HashService>();

            return services;
        }

        private static IServiceCollection ConfigureMediatR(
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
