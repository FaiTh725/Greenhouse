using Application.Shared.Exceptions;
using Greenhouse.Domain.Entities;
using Greenhouse.Domain.Repositorires;
using MediatR;

namespace Greenhouse.Application.Commands.EventResource.AddEventResource
{
    public class AddEventResourceHandler :
        IRequestHandler<AddEventResourceCommand, long>
    {
        private readonly IUnitOfWork unitOfWork;

        public AddEventResourceHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<long> Handle(AddEventResourceCommand request, CancellationToken cancellationToken)
        {
            var eventResource = Resource.Initialize(
                request.Name,
                request.Unit,
                request.ResourceType);

            if(eventResource.IsFailure)
            {
                throw new BadRequestException("Incorrect request - " +
                    eventResource.Error);
            }

            var eventResourcePlane = AgricultiralEventResource.Initialize(
                eventResource.Value,
                request.EventId,
                request.PlannedAmount);

            if(eventResourcePlane.IsFailure)
            {
                throw new BadRequestException("Incorrect request - " +
                    eventResourcePlane.Error);
            }

            var eventResourceDb = await unitOfWork.AgricultiralEventResourceRepository
                .AggEventResource(eventResourcePlane.Value);

            await unitOfWork.SaveChangesAsync();

            return eventResourceDb.Id;
        }
    }
}
