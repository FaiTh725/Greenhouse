using Greenhouse.Domain.Entities;

namespace Greenhouse.Domain.Repositorires
{
    public interface IAgricultiralEventRepository
    {
        Task<AgricultiralEvent> AddAgricultiralEvent(AgricultiralEvent agricultiralEvent);

        Task<AgricultiralEvent?> GetAgricultiralEvent(long id);

        Task<AgricultiralEvent?> GetAgricultiralEventWithExecuter(long id);

        Task<AgricultiralEvent?> GetAgricultiralEventWithResources(long id);

        Task<IEnumerable<AgricultiralEvent>> GetAgricultiralEventsOfEmploye(long employeId);

        Task CancelEvents(List<long> eventIdList);

        Task UpdateEvent(long eventId, AgricultiralEvent updatedEvent);
    }
}
