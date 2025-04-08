using Greenhouse.Application.Common.BehaviorsInterfaces;
using Greenhouse.Application.Contracts.Greenhouse;
using MediatR;

namespace Greenhouse.Application.Queries.Greenhouse.GetGreenhouseEvents
{
    public class GetGreenhouseEventsQuery : 
        IRequest<GreenhouseEventsResponse>,
        ICacheQuery
    {
        public long GreenhouseId { get; set; }

        public string Key => $"GreenhouseEvents:{GreenhouseId}";

        public int ExpirationSeconds => 120;
    }
}
