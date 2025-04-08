using Greenhouse.Domain.Entities;
using Greenhouse.Domain.Models.EventResource;
using Greenhouse.Domain.Repositorires;
using Microsoft.EntityFrameworkCore;

namespace Greenhouse.Dal.Repositories
{
    public class AgricultiralEventResourceRepository : 
        IAgricultiralEventResourceRepository
    {
        private readonly AppDbContext context;

        public AgricultiralEventResourceRepository(
            AppDbContext context)
        {
            this.context = context;
        }

        public async Task<AgricultiralEventResource> AggEventResource(AgricultiralEventResource resource)
        {
            var eventResourceEntity = await context.EventResources
                .AddAsync(resource);

            return eventResourceEntity.Entity;
        }

        public async Task<AgricultiralEventResource?> GetEventResourceWithResource(long id)
        {
            return await context.EventResources
                .Include(x => x.Resource)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<AgricultiralEventResource>> GetEventResourceWithResourcesByEventId(long eventId)
        {
            return await context.EventResources
                .Where(x => x.EventId == eventId)
                .Include(x => x.Resource)
                .ToListAsync();
        }

        public async Task UpdateEventResources(List<UpdateEventResource> resourceToUpdate)
        {
            var resourcesId = resourceToUpdate
                .Select(x => x.Id);

            var resources = await context.EventResources
                .Where(x => resourcesId.Contains(x.Id))
                .ToListAsync();

            foreach(var resource in resources)
            {
                var resourceUpdateFrom = resourceToUpdate
                    .FirstOrDefault(x => x.Id == resource.Id);

                if(resourceUpdateFrom is not null)
                {
                    resource.ActualAmount = resourceUpdateFrom.ActualAmount;
                }
            }
        }
    }
}
