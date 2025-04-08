using Greenhouse.Domain.Enums;

namespace Greenhouse.Application.Contracts.Event
{
    public class EventResponse
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public EventStatus EventStatus { get; set; }

        public DateTime PlannedDate { get; set; }

        public DateTime? ActualDate { get; set;}

        public string ExecutingEmail { get; set; } = string.Empty;

        public long GreenhouseId { get; set; }
    }
}
