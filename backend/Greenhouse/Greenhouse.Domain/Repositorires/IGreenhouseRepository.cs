using Greenhouse.Domain.Entities;

namespace Greenhouse.Domain.Repositorires
{
    public interface IGreenhouseRepository
    {
        Task<GreenhouseEntity> AddGreenhouse(GreenhouseEntity greenhouse);

        Task<GreenhouseEntity?> GetGreenhouse(long id);

        Task<IEnumerable<AgricultiralEvent>> GetGreenhouseEvents(long greenhouseId);

        Task<IEnumerable<AgricultiralEvent>> GetGreenhouseCompletedEvents(long greenhouseId);

        Task<IEnumerable<AgricultiralEvent>> GetGreenhouseEvents(long greenhouseId, DateOnly eventDay);

        Task<(IEnumerable<GreenhouseEntity> data, long maxCount)> GetGreenhouses(int page, int count);
    }
}
