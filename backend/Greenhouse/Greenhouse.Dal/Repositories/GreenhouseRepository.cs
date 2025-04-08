using CSharpFunctionalExtensions;
using Greenhouse.Domain.Entities;
using Greenhouse.Domain.Enums;
using Greenhouse.Domain.Repositorires;
using Microsoft.EntityFrameworkCore;

namespace Greenhouse.Dal.Repositories
{
    public class GreenhouseRepository : IGreenhouseRepository
    {
        private readonly AppDbContext context;

        public GreenhouseRepository(
            AppDbContext context)
        {
            this.context = context;
        }

        public async Task<GreenhouseEntity> AddGreenhouse(
            GreenhouseEntity greenhouse)
        {
            var greenhouseEntity = await context.Greenhouses
                .AddAsync(greenhouse);
        
            return greenhouseEntity.Entity;
        }

        public async Task<GreenhouseEntity?> GetGreenhouse(long id)
        {
            return await context.Greenhouses
                .Include(x => x.CropType)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<AgricultiralEvent>> GetGreenhouseCompletedEvents(long greenhouseId)
        {
            return await context.Events
                .Where(x => 
                    x.GreenhouseId == greenhouseId && 
                    x.EventStatus == EventStatus.Completed)
                .Include(x => x.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<AgricultiralEvent>> GetGreenhouseEvents(long greenhouseId)
        {
            return await context.Events
                .Where(x => x.GreenhouseId == greenhouseId)
                .Include(x => x.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<AgricultiralEvent>> GetGreenhouseEvents(
            long greenhouseId, DateOnly eventDay)
        {

            return await context.Events
                .Where(x => 
                    x.GreenhouseId == greenhouseId && 
                    DateOnly.FromDateTime(x.PlannedDate) == eventDay)
                .Include(x => x.Employee)
                .ToListAsync();
        }

        public async Task<(IEnumerable<GreenhouseEntity> data, long maxCount)> GetGreenhouses(int page, int count)
        {
            var query = context.Greenhouses.AsQueryable();

            var maxCount = await query.CountAsync();

            var data = await context.Greenhouses
                .Skip((page - 1) * count)
                .Take(count)
                .Include(x => x.CropType)
                .ToListAsync();
            
            return (data, maxCount);
        }
    }
}
