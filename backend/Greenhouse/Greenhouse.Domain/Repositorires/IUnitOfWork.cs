namespace Greenhouse.Domain.Repositorires
{
    public interface IUnitOfWork
    {
        IEmployeRepository EmployeRepository { get; }

        IGreenhouseRepository GreenhouseRepository { get; }
        IAgricultiralEventRepository AgricultiralEventRepository { get; }
        IAgricultiralEventResourceRepository AgricultiralEventResourceRepository { get; }
        IHarvestRecordRepository HarvestRecordRepository { get; }

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
