using Application.Shared.Exceptions;
using Greenhouse.Application.Contracts.EventResource;
using Greenhouse.Domain.Repositorires;
using MediatR;

namespace Greenhouse.Application.Queries.EventResource.GetEventResources
{
    public class GetEventResourcesHandler :
        IRequestHandler<GetEventResourcesQuery, IEnumerable<EventResourceResponse>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetEventResourcesHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EventResourceResponse>> Handle(GetEventResourcesQuery request, CancellationToken cancellationToken)
        {
            var eventEntity = await unitOfWork.AgricultiralEventRepository
                .GetAgricultiralEvent(request.EventId);
        
            if(eventEntity is null)
            {
                throw new NotFoundException("Event doesnt exist");
            }

            var eventResources = await unitOfWork.AgricultiralEventResourceRepository
                .GetEventResourceWithResourcesByEventId(request.EventId);

            return eventResources.Select(x => new EventResourceResponse 
            { 
                Id = x.Id,
                Name = x.Resource.Name,
                ActualAmount = x.ActualAmount,
                PlannedAmount = x.PlannedAmount,
                ResourceType = x.Resource.ResourceType,
                Unit = x.Resource.Unit
            });
        }
    }
}
