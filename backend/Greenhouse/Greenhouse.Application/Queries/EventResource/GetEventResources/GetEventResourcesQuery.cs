using Greenhouse.Application.Contracts.EventResource;
using MediatR;

namespace Greenhouse.Application.Queries.EventResource.GetEventResources
{
    public class GetEventResourcesQuery :
        IRequest<IEnumerable<EventResourceResponse>>
    {
        public long EventId { get; set; }
    }
}
