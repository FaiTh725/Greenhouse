namespace Authorize.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRoleRepository RoleRepository { get; }

        IUserRepository UserRepository { get; }

        Task<bool> SaveChangesAsync();

        bool SaveChanges();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollBackTransactionAsync();

        void BeginTransaction();

        void CommitTransaction();

        void RollBackTransaction();

        bool CanConnect();

        Task<bool> CanConnectAsync();
    } 
}
