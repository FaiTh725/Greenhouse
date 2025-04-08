namespace Greenhouse.Application.Contracts.Event
{
    public class EventNotification
    {
        public string ExecuterEmail { get; set; } = string.Empty;

        public string EventName { get; set; } = string.Empty;

        public DateTime PlannedDate { get; set; }
    }
}
