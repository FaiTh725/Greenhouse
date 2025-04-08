using Greenhouse.Application.Common.BehaviorsInterfaces;
using MediatR;

namespace Greenhouse.Application.Commands.Event.ProcessEvent
{
    public class ProcessEventCommand : 
        IRequest, ICheckEmployEvent,
        IClearCacheBehavior
    {
        public long EventId {  get; set; }

        public string EmployeEmail { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string Key => "GreenhouseEvents:";
    }
}
