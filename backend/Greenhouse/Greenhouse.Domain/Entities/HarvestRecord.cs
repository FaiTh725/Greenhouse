using CSharpFunctionalExtensions;

namespace Greenhouse.Domain.Entities
{
    public class HarvestRecord : Entity
    {
        public string ResourceName { get; private set; } = string.Empty;

        public string YieldUnit { get; private set; } = string.Empty;

        public double ActualYield { get; private set; }

        public AgricultiralEvent Event { get; private set; }
        public long EventId { get; private set; }

        public HarvestRecord() {}

        private HarvestRecord(
            string resourceName,
            string yieldUnit,
            double actualYield,
            long eventId)
        {
            ResourceName = resourceName;
            YieldUnit = yieldUnit;
            ActualYield = actualYield;
            EventId = eventId;
        }

        public static Result<HarvestRecord> Initialize(
            string resourceName,
            string yieldUnit,
            double actualYield,
            long eventId)
        {
            if(string.IsNullOrWhiteSpace(resourceName))
            {
                return Result.Failure<HarvestRecord>("Resource Name is null");
            }

            if(string.IsNullOrWhiteSpace(yieldUnit))
            {
                return Result.Failure<HarvestRecord>("Yieldunit is null");
            }

            if(actualYield < 0)
            {
                return Result.Failure<HarvestRecord>("ActualYield is less than zero");
            }

            return Result.Success(new HarvestRecord(
                resourceName, yieldUnit, actualYield, eventId));
        }
    }
}
