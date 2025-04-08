namespace Greenhouse.Contracts.Resource
{
    public class ResourceSpending
    {
        public long Id { get; set; }

        public double PlannedAmount { get; set; }

        public double? ActualAmount { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public string ResourceType { get; set; } = string.Empty;
    }
}
