using Greenhouse.Domain.Entities;
using Greenhouse.Domain.Repositorires;

namespace Greenhouse.Dal.Repositories
{
    public class HarvestRecordRepository : 
        IHarvestRecordRepository
    {
        private readonly AppDbContext context;

        public HarvestRecordRepository(
            AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddHarvestRecords(List<HarvestRecord> harvestRecords)
        {
            await context.HarvestRecords
                .AddRangeAsync(harvestRecords);
        }
    }
}
