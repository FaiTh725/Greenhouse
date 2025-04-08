namespace Greenhouse.Application.Contracts.Greenhouse
{
    public class GreenhouseResponse
    {
        public long Id { get; set; }

        public string NameGreenHouse { get; set; } = string.Empty;

        public double Area { get; set; }

        public string Location { get; set; } = string.Empty;

        public string CropName { get; set; } = string.Empty;
    }
}
