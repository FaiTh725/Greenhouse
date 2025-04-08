using Authorize.Dal.Repositories;
using Authorize.Dal.Services;
using Authorize.Domain.Repositories;
using Authorize.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Authorize.Dal
{
    public static class Startup
    {
        public static IServiceCollection ConfigureDalServices(
            this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IMigrationsService, MigrationsService>();

            return services;
        }

    }
}
