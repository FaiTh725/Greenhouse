using Greenhouse.Application.Common.BehaviorsInterfaces;
using Greenhouse.Application.Contracts.EventResource;
using MediatR;

namespace Greenhouse.Application.Commands.Event.CompleteEvent
{
    public class CompleteEventCommand :
        IRequest, ICheckEmployEvent,
        IClearCacheBehavior
    {
        public long EventId { get; set; }

        public string EmployeEmail { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public List<EventResourceAmount> ActualResources { get; set; } = new();

        public string Key => "GreenhouseEvents:";
    }
}
