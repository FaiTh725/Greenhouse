using Greenhouse.Application.Contracts.HarvestResource;
using Greenhouse.Domain.Entities;
using Greenhouse.Domain.Repositorires;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Greenhouse.Application.Consumers.HarvestResource
{
    public class CreateHarvestResourceConsumer :
        IConsumer<CreateHarvestResourceRequest>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CreateHarvestResourceConsumer> logger;

        public CreateHarvestResourceConsumer(
            IUnitOfWork unitOfWork,
            ILogger<CreateHarvestResourceConsumer> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateHarvestResourceRequest> context)
        {
            var eventResource = await unitOfWork.AgricultiralEventResourceRepository
                .GetEventResourceWithResourcesByEventId(
                    context.Message.EventId);

            var harvestingResources = eventResource.Select(x => 
                HarvestRecord.Initialize(x.Resource.Name, x.Resource.Unit,
                x.ActualAmount.GetValueOrDefault(), x.EventId));
        
            if(harvestingResources.Any(x => x.IsFailure))
            {
                logger.LogError("Error initialize harvest record");
                return;
            }

            await unitOfWork.HarvestRecordRepository
                .AddHarvestRecords(harvestingResources
                    .Select(x => x.Value)
                    .ToList());

            await unitOfWork.SaveChangesAsync();
        }
    }
}
