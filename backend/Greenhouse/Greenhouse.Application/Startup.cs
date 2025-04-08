using Greenhouse.Application.Behaviors.Cache;
using Greenhouse.Application.Behaviors.Event;
using Microsoft.Extensions.DependencyInjection;

namespace Greenhouse.Application
{
    public static class Startup
    {
        public static IServiceCollection ConfigureApplicationServices(
            this IServiceCollection services)
        {
            services
                .ConfigureMediatr();

            return services;
        }

        public static IServiceCollection ConfigureMediatr(
            this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly);
                cfg.AddOpenBehavior(typeof(CheckEmployAccessBehavior<,>));
                cfg.AddOpenBehavior(typeof(CacheBehavior<,>));
                cfg.AddOpenBehavior(typeof(ClearCacheBehavior<,>));
            });

            return services;
        }
    }
}
