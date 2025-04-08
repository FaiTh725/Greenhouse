using Greenhouse.Application.Contracts.EventResource;
using MediatR;

namespace Greenhouse.Application.Queries.EventResource.GetEventWithResById
{
    public class GetEventWithResByIdQuery : IRequest<EventResourceResponse>
    {
        public long EventResourceId { get; set; }
    }
}
