using Authorize.Domain.Entities;
using Authorize.Domain.Repositories;
using Authorize.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Authorize.Infastructure.BackgroundServices
{
    public class InitializeRolesBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public InitializeRolesBackgroundService(
            IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await WaitInitializeDb(stoppingToken);

            using var scope = serviceScopeFactory.CreateAsyncScope();
            var migrationService = scope.ServiceProvider
                .GetRequiredService<IMigrationsService>();
            var logger = scope.ServiceProvider
                .GetRequiredService<ILogger<InitializeRolesBackgroundService>>();
            var unitOfWork = scope.ServiceProvider
                .GetRequiredService<IUnitOfWork>();

            var existingRoles = unitOfWork.RoleRepository.GetRoles();

            var requiredRoles = new List<string>
            {
                "Admin",
                "Manager"
            };

            await unitOfWork.BeginTransactionAsync();

            var addRolesTasks = requiredRoles
                .Select(async roleName =>
                {
                    if (existingRoles.FirstOrDefault(x => x.Name == roleName) is null)
                    {
                        using var innerScope = serviceScopeFactory.CreateAsyncScope();
                        var innerUnitOfWork = innerScope.ServiceProvider
                            .GetRequiredService<IUnitOfWork>();

                        var newRole = Role.Initialize(roleName);

                        if (newRole.IsFailure)
                        {
                            logger.LogError("Erorr initialize role");
                            return;
                        }

                        await innerUnitOfWork.RoleRepository.AddRole(newRole.Value);
                        await innerUnitOfWork.SaveChangesAsync();
                    }
                }).ToList();

            await Task.WhenAll(addRolesTasks);

            await unitOfWork.CommitTransactionAsync();
        }

        private async Task WaitInitializeDb(CancellationToken stoppingToken)
        {
            using var scope = serviceScopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider
                .GetRequiredService<IUnitOfWork>();

            while (!stoppingToken.IsCancellationRequested)
            {
                if(await dbContext.CanConnectAsync())
                {
                    return;
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
