using Greenhouse.Domain.Enums;

namespace Greenhouse.Application.Contracts.EventResource
{
    public class EventResourceResponse
    {
        public long Id { get; set; }

        public double PlannedAmount { get; set; }

        public double? ActualAmount { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public ResourceType ResourceType { get; set; }
    }
}
