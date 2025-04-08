using Greenhouse.Application.Contracts.EventResource;

namespace Greenhouse.API.Contracts.GreenhouseEvent
{
    public class CompleteEventRequest
    {
        public long EventId { get; set; }

        public List<EventResourceAmount> ActualResources { get; set; } = new();
    }
}
