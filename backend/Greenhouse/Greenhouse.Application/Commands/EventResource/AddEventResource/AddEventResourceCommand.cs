using Greenhouse.Application.Common.BehaviorsInterfaces;
using Greenhouse.Domain.Enums;
using MediatR;

namespace Greenhouse.Application.Commands.EventResource.AddEventResource
{
    public class AddEventResourceCommand : 
        IRequest<long>, IClearCacheBehavior
    {
        public long EventId { get; set; }

        public double PlannedAmount { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public ResourceType ResourceType {  get; set; }

        public string Key => "GreenhouseEvents:";
    }
}
