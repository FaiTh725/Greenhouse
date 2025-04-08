
using Application.Shared.Exceptions;
using Authorize.Dal.Configurations;
using Authorize.Domain.Entities;
using Authorize.Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Authorize.Dal
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IConfiguration configuration,
            IMediator mediator)
        {
            this.configuration = configuration;
            this.mediator = mediator;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = configuration
                .GetConnectionString("NpgConnection") ?? 
                throw new AppConfigurationException("Posgres connection string");

            optionsBuilder.UseNpgsql(connectionString);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            await PublishDomainEventsAsync();

            return result;
        }

        private async Task PublishDomainEventsAsync()
        {
            var domainEvents = ChangeTracker
                .Entries<DomainEventEntity>()
                .Select(x => x.Entity)
                .SelectMany(x =>
                {
                    var domainEvents = x.Events;

                    x.ClearEvents();

                    return domainEvents;
                })
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }
        }
    }
}
