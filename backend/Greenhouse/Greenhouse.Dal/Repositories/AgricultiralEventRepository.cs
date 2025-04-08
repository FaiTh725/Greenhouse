using Greenhouse.Domain.Entities;
using Greenhouse.Domain.Repositorires;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Greenhouse.Dal.Repositories
{
    public class AgricultiralEventRepository : 
        IAgricultiralEventRepository
    {
        private readonly AppDbContext context;

        public AgricultiralEventRepository(
            AppDbContext context)
        {
            this.context = context;
        }

        public async Task<AgricultiralEvent> AddAgricultiralEvent(AgricultiralEvent agricultiralEvent)
        {
            var agriculturalEventEntity = await context
                .Events.AddAsync(agricultiralEvent);

            return agriculturalEventEntity.Entity;
        }

        public async Task CancelEvents(List<long> eventIdList)
        {
            await context.Events
                .Where(x => eventIdList.Any(y => y == x.Id))
                .ExecuteDeleteAsync();

        }

        public async Task<AgricultiralEvent?> GetAgricultiralEvent(long id)
        {
            return await context.Events
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<AgricultiralEvent>> GetAgricultiralEventsOfEmploye(long employeId)
        {
            return await context.Events
                .Where(x => x.EmploeeId == employeId)
                .ToListAsync();
        }

        public async Task<AgricultiralEvent?> GetAgricultiralEventWithExecuter(long id)
        {
            return await context.Events
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AgricultiralEvent?> GetAgricultiralEventWithResources(long id)
        {
            return await context.Events
                .Where(x => x.Id == id)
                .Include(x => x.Resources)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateEvent(long eventId, AgricultiralEvent updatedEvent)
        {
            await context.Events
                .Where(x => x.Id == eventId)
                .ExecuteUpdateAsync(x => x
                .SetProperty(x => x.ActualCompletedDate, updatedEvent.ActualCompletedDate)
                .SetProperty(x => x.EventStatus, updatedEvent.EventStatus)
                .SetProperty(x => x.EventType, updatedEvent.EventType)
                .SetProperty(x => x.Name, updatedEvent.Name));
        }
    }
}
