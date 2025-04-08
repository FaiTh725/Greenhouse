using Application.Shared.Exceptions;
using Greenhouse.Application.Contracts.HarvestResource;
using Greenhouse.Domain.Enums;
using Greenhouse.Domain.Models.EventResource;
using Greenhouse.Domain.Repositorires;
using MassTransit;
using MediatR;

namespace Greenhouse.Application.Commands.Event.CompleteEvent
{
    public class CompleteEventHandler :
        IRequestHandler<CompleteEventCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBus bus;

        public CompleteEventHandler(
            IUnitOfWork unitOfWork,
            IBus bus)
        {
            this.unitOfWork = unitOfWork;
            this.bus = bus;
        }

        public async Task Handle(CompleteEventCommand request, CancellationToken cancellationToken)
        {
            if (request.ActualResources.Any(x => x.Amount < 0))
            {
                throw new BadRequestException("Any amount less than zero");
            }

            var eventEntity = await unitOfWork.AgricultiralEventRepository
                .GetAgricultiralEventWithResources(request.EventId);

            if(eventEntity is null)
            {
                throw new BadRequestException("Event doesnt exist");
            }

            var existedEventResourcesId = eventEntity.Resources.Select(x => x.Id);
            var requestEventResourcesId = request.ActualResources.Select(x => x.EventResourceId);

            var isSuperset = new HashSet<long>(existedEventResourcesId)
                .IsSubsetOf(requestEventResourcesId);

            if(!isSuperset)
            {
                throw new BadRequestException("Any resourceid doesnt exist");
            }

            try
            {
                eventEntity.ChangeEventStatus(EventStatus.Completed);
            }
            catch
            {
                throw new BadRequestException("Event doest in progress, or already completed");
            }

            await unitOfWork.AgricultiralEventResourceRepository
                .UpdateEventResources(request.ActualResources
                    .Select(x => new UpdateEventResource
                    {
                        Id = x.EventResourceId,
                        ActualAmount = x.Amount
                    })
                    .ToList());

            await unitOfWork.SaveChangesAsync();

            if(eventEntity.EventType == EventType.Harvesting)
            {
                await bus.Publish(new CreateHarvestResourceRequest
                {
                    EventId = eventEntity.Id
                });
            }
        }
    }
}
