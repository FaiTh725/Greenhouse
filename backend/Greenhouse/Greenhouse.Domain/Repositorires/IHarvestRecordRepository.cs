using Greenhouse.Domain.Entities;

namespace Greenhouse.Domain.Repositorires
{
    public interface IHarvestRecordRepository
    {
        Task AddHarvestRecords(List<HarvestRecord> harvestRecords);
    }
}
