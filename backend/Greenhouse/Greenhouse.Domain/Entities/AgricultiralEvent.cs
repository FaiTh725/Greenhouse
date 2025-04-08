using CSharpFunctionalExtensions;
using Greenhouse.Domain.Enums;

namespace Greenhouse.Domain.Entities
{
    public class AgricultiralEvent : Entity
    {
        public string Name { get; private set; } = string.Empty;
    
        public EventType EventType { get; private set; }

        public EventStatus EventStatus { get; private set; }

        public DateTime PlannedDate { get; private set; }

        public DateTime? ActualCompletedDate { get; private set; }

        public GreenhouseEntity Greenhouse { get; private set; }
        public long GreenhouseId { get; private set; }

        public Employee Employee { get; private set; }
        public long EmploeeId { get; private set; }

        public List<AgricultiralEventResource> Resources { get; private set; } = new List<AgricultiralEventResource>();

        public AgricultiralEvent(){}

        private AgricultiralEvent(
            string name, 
            EventType eventType, 
            DateTime plannedDate,
            long greenhouseId,
            long employeId)
        {
            Name = name;
            EventType = eventType;
            PlannedDate = plannedDate;
            GreenhouseId = greenhouseId;
            EmploeeId = employeId;

            ActualCompletedDate = null;
            EventStatus = EventStatus.Planned;
        }
      
        public void ChangeEventStatus(EventStatus status)
        {
            if(status == EventStatus.Planned ||
                (status == EventStatus.InProgress && 
                EventStatus == EventStatus.Completed))
            {
                throw new InvalidOperationException("Change event status failure");
            }

            EventStatus = status;

            if(status == EventStatus.Completed)
            {
                ActualCompletedDate= DateTime.UtcNow;
            }
        }

        public static Result<AgricultiralEvent> Initialize(

            string name,
            EventType eventType,
            DateTime plannedDate,
            long greenhouseId,
            long employeId)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<AgricultiralEvent>("Name is null");
            }

            if(plannedDate <= DateTime.UtcNow)
            {
                return Result.Failure<AgricultiralEvent>("Planned date is the past time");
            }

            return Result.Success(new AgricultiralEvent(
                name, eventType, plannedDate,
                greenhouseId, employeId));
        }
    }
}
