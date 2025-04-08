using Greenhouse.Application.Common.BehaviorsInterfaces;
using Greenhouse.Domain.Enums;
using MediatR;

namespace Greenhouse.Application.Commands.Event.AddEvent
{
    public class AddEventCommand : 
        IRequest<long>, IClearCacheBehavior
    {
        public string Name { get; set; } = string.Empty;

        public DateTime PlannedDate { get; set; }

        public EventType EventType { get; set; }

        public long EmployeId { get; set; }

        public long GreenhouseId { get; set; }

        public string Key => "GreenhouseEvents:";
    }
}
