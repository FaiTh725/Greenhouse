using Greenhouse.Application.Contracts.Event;

namespace Greenhouse.Application.Contracts.Greenhouse
{
    public class GreenhouseEventsResponse
    {
        public long Id { get; set; }

        public IEnumerable<EventResponse> Events { get; set; } = Enumerable.Empty<EventResponse>();
    }
}
