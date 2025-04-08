using Greenhouse.Application.Contracts.Greenhouse;
using MediatR;

namespace Greenhouse.Application.Queries.Greenhouse.GetGreenhouseCompletedEvents
{
    public class GetGreenhouseCompletedEventsQuery : 
        IRequest<GreenhouseEventsResponse>
    {
        public long Id { get; set; }
    }
}
