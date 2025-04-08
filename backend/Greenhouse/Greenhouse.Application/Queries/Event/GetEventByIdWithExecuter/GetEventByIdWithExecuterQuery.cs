using Greenhouse.Application.Contracts.Event;
using MediatR;

namespace Greenhouse.Application.Queries.Event.GetEventByIdWithExecuter
{
    public class GetEventByIdWithExecuterQuery : IRequest<EventResponse>
    {
        public long Id { get; set; }
    }
}
