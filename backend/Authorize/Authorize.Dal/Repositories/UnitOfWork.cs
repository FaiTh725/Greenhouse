using Authorize.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Authorize.Dal.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        private UserRepository userRepository;
        private RoleRepository roleRepository;

        private IDbContextTransaction transaction;

        public UnitOfWork(
            AppDbContext context,
            IUserRepository userRepository,
            IRoleRepository roleRepository)
        {
            this.context = context;
            this.userRepository = (UserRepository)userRepository;
            this.roleRepository = (RoleRepository)roleRepository;
        }

        public IRoleRepository RoleRepository => roleRepository;

        public IUserRepository UserRepository => userRepository;

        public void BeginTransaction()
        {
            transaction = context.Database.BeginTransaction();
        }

        public async Task BeginTransactionAsync()
        {
            transaction = await context.Database.BeginTransactionAsync();
        }

        public bool CanConnect()
        {
            return context.Database.CanConnect();
        }

        public async Task<bool> CanConnectAsync()
        {
            return await context.Database.CanConnectAsync();
        }

        public void CommitTransaction()
        {
            AssuranceTransaction();

            transaction.Commit();
        }

        public async Task CommitTransactionAsync()
        {
            AssuranceTransaction();

            await transaction.CommitAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public void RollBackTransaction()
        {
            AssuranceTransaction();

            transaction.Rollback();
        }

        public async Task RollBackTransactionAsync()
        {
            AssuranceTransaction();

            await transaction.RollbackAsync();
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        private void AssuranceTransaction()
        {
            if (transaction == null)
            {
                throw new InvalidOperationException("Transaction has not been started.");
            }
        }
    }
}
