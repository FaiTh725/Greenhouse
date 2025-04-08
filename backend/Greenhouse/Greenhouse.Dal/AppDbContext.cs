using Application.Shared.Exceptions;
using Greenhouse.Dal.Configurations;
using Greenhouse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Greenhouse.Dal
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IConfiguration configuration) :
            base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<GreenhouseEntity> Greenhouses { get; set; }

        public DbSet<CropType> CropTypes { get; set; }

        public DbSet<AgricultiralEventResource> EventResources { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Employee> Employees { get; set; }  

        public DbSet<AgricultiralEvent> Events { get; set; }

        public DbSet<HarvestRecord> HarvestRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AgricultiralEventConfiguration());
            modelBuilder.ApplyConfiguration(new AgricultiralEventResourceConfiguration());
            modelBuilder.ApplyConfiguration(new CropTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new GreenhouseConfiguration());
            modelBuilder.ApplyConfiguration(new ResourceConfiguration());
            modelBuilder.ApplyConfiguration(new HarvestRecordConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = configuration
                .GetConnectionString("NpgConnection") ??
                throw new AppConfigurationException("Posgres connection string");

            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
