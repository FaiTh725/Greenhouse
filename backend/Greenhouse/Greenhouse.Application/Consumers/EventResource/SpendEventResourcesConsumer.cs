using Greenhouse.Contracts.Resource;
using Greenhouse.Domain.Enums;
using Greenhouse.Domain.Repositorires;
using MassTransit;

namespace Greenhouse.Application.Consumers.EventResource
{
    public class SpendEventResourcesConsumer :
        IConsumer<EventResourceSpendingRequest>
    {
        private readonly IUnitOfWork unitOfWork;

        public SpendEventResourcesConsumer(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;   
        }

        public async Task Consume(
            ConsumeContext<EventResourceSpendingRequest> context)
        {
            var eventEntity = await unitOfWork.AgricultiralEventRepository
                .GetAgricultiralEvent(context.Message.EventId);

            if(eventEntity is null ||
                eventEntity.EventStatus != EventStatus.Completed)
            {
                await context.RespondAsync(new EventResourceSpendingResponse 
                { 
                    IsSuccess = false,
                    Message = "Event Doesnt Exist"
                });
            }

            var eventResources = await unitOfWork.AgricultiralEventResourceRepository
                .GetEventResourceWithResourcesByEventId(eventEntity!.Id);

            await context.RespondAsync(new EventResourceSpendingResponse 
            { 
                IsSuccess = true,
                Resources = eventResources.Select(x => new ResourceSpending
                {
                    Id = x.Id,
                    ActualAmount = x.ActualAmount,
                    Name = x.Resource.Name,
                    PlannedAmount = x.PlannedAmount,
                    Unit = x.Resource.Unit,
                    ResourceType = x.Resource.ResourceType.ToString()
                }).ToList()
            });

        }
    }
}
