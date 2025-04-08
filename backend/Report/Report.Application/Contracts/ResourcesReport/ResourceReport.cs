namespace Report.Application.Contracts.ResourcesReport
{
    public class ResourceReport
    {
        public long ResourceId { get; set; }

        public string Name { get; set; } = string.Empty;

        public double ActualAmount { get; set; }

        public double PlannedAmount { get; set; }

        public string Unit { get; set; } = string.Empty;

        public string ResourceType {  get; set; } = string.Empty;
    }
}
