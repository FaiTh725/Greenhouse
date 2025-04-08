using Application.Shared.Exceptions;
using Greenhouse.Application.Contracts.EventResource;
using Greenhouse.Domain.Repositorires;
using MediatR;

namespace Greenhouse.Application.Queries.EventResource.GetEventWithResById
{
    public class GetEventWithResByIdHandler :
        IRequestHandler<GetEventWithResByIdQuery, EventResourceResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetEventWithResByIdHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;   
        }

        public async Task<EventResourceResponse> Handle(GetEventWithResByIdQuery request, CancellationToken cancellationToken)
        {
            var eventResource = await unitOfWork.AgricultiralEventResourceRepository
                .GetEventResourceWithResource(request.EventResourceId);

            if(eventResource is null)
            {
                throw new NotFoundException("Event resource doesnt exist");
            }

            return new EventResourceResponse
            {
                Id = eventResource.Id,
                ActualAmount = eventResource.ActualAmount,
                PlannedAmount = eventResource.PlannedAmount,
                Name = eventResource.Resource.Name,
                Unit = eventResource.Resource.Unit,
                ResourceType = eventResource.Resource.ResourceType
            };
        }
    }
}
