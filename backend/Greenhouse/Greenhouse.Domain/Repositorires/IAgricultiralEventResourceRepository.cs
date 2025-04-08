using Greenhouse.Domain.Entities;
using Greenhouse.Domain.Models.EventResource;

namespace Greenhouse.Domain.Repositorires
{
    public interface IAgricultiralEventResourceRepository
    {
        Task<AgricultiralEventResource> AggEventResource(AgricultiralEventResource resource);

        Task<AgricultiralEventResource?> GetEventResourceWithResource(long id);

        Task UpdateEventResources(List<UpdateEventResource> resourceToUpdate);

        Task<IEnumerable<AgricultiralEventResource>> GetEventResourceWithResourcesByEventId(long eventId);
    }
}
