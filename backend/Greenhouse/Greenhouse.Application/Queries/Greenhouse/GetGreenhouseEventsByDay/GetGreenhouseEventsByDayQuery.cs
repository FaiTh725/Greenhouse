using Greenhouse.Application.Common.BehaviorsInterfaces;
using Greenhouse.Application.Contracts.Greenhouse;
using MediatR;

namespace Greenhouse.Application.Queries.Greenhouse.GetGreenhouseEventsByDay
{
    public class GetGreenhouseEventsByDayQuery : 
        IRequest<GreenhouseEventsResponse>,
        ICacheQuery
    {
        public long GreenhouseId { get; set; }

        public DateOnly EventsDay { get; set; }

        public string Key => $"GreenhouseEvents:{GreenhouseId}-{EventsDay}";

        public int ExpirationSeconds => 120;
    }
}
