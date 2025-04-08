namespace Greenhouse.Application.Contracts.Greenhouse
{
    public class GreenhousePaginationResponse
    {
        public List<GreenhouseResponse> Greenhouses { get; set; } = new List<GreenhouseResponse>();

        public long MaxCount {  get; set; }
    }
}
