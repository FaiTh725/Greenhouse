using Greenhouse.Domain.Repositorires;
using Microsoft.EntityFrameworkCore.Storage;

namespace Greenhouse.Dal.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        private readonly EmployeRepository employeRepository;
        private readonly GreenhouseRepository greenhouseRepository;
        private readonly AgricultiralEventRepository agricultiralEventRepository;
        private readonly AgricultiralEventResourceRepository agricultiralEventResourceRepository;
        private readonly HarvestRecordRepository harvestRecordRepository;

        private IDbContextTransaction transaction;

        public UnitOfWork(
            AppDbContext context,
            IEmployeRepository employeRepository,
            IGreenhouseRepository greenhouseRepository,
            IAgricultiralEventRepository agricultiralEventRepository,
            IAgricultiralEventResourceRepository agricultiralEventResourceRepository,
            IHarvestRecordRepository harvestRecordRepository)
        {
            this.context = context;

            this.employeRepository = (EmployeRepository)employeRepository;
            this.greenhouseRepository = (GreenhouseRepository)greenhouseRepository;
            this.agricultiralEventRepository = (AgricultiralEventRepository)agricultiralEventRepository;
            this.agricultiralEventResourceRepository = (AgricultiralEventResourceRepository)agricultiralEventResourceRepository;
            this.harvestRecordRepository = (HarvestRecordRepository)harvestRecordRepository;
        }

        public IEmployeRepository EmployeRepository => employeRepository;

        public IGreenhouseRepository GreenhouseRepository => greenhouseRepository;

        public IAgricultiralEventRepository AgricultiralEventRepository => agricultiralEventRepository;

        public IAgricultiralEventResourceRepository AgricultiralEventResourceRepository => agricultiralEventResourceRepository;

        public IHarvestRecordRepository HarvestRecordRepository => harvestRecordRepository;

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
