using Greenhouse.Dal.Repositories;
using Greenhouse.Domain.Repositorires;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Greenhouse.Dal
{
    public static class Startup
    {
        public static IServiceCollection ConfigureDalServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmployeRepository, EmployeRepository>();
            services.AddScoped<IGreenhouseRepository, GreenhouseRepository>();
            services.AddScoped<IAgricultiralEventRepository, AgricultiralEventRepository>();
            services.AddScoped<IAgricultiralEventResourceRepository, AgricultiralEventResourceRepository>();
            services.AddScoped<IHarvestRecordRepository, HarvestRecordRepository>();

            return services;
        }
    }
}
