using CSharpFunctionalExtensions;

namespace Greenhouse.Domain.Entities
{
    public class AgricultiralEventResource : Entity
    {
        public Resource Resource { get; private set; }

        public AgricultiralEvent Event { get; private set; }
        public long EventId { get; private set; }

        public double PlannedAmount { get; private set; }

        public double? ActualAmount { get; set; }

        public AgricultiralEventResource(){}

        private AgricultiralEventResource(
            Resource resource, 
            long eventId, 
            double plannedAmount, 
            double? actualAmount = null)
        {
            Resource = resource;
            EventId = eventId;
            PlannedAmount = plannedAmount;

            ActualAmount = actualAmount;
        }
      
        public static Result<AgricultiralEventResource> Initialize(
            Resource resource,
            long eventId,
            double plannedAmount)
        {
            if (plannedAmount < 0)
            {
                return Result.Failure<AgricultiralEventResource>("PlannedAmount less than zero");
            }

            if(resource is null)
            {
                return Result.Failure<AgricultiralEventResource>("Resource is null");
            }

            return Result.Success(new AgricultiralEventResource(
                resource, eventId, plannedAmount));
        }
    }
}
