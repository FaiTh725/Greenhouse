namespace Greenhouse.Contracts.Resource
{
    public class EventResourceSpendingResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; } = string.Empty;

        public List<ResourceSpending> Resources { get; set; } = new();
    }
}
