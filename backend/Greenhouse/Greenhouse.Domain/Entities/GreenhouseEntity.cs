using CSharpFunctionalExtensions;

namespace Greenhouse.Domain.Entities
{
    public class GreenhouseEntity : Entity
    {
        public string Name { get; private set; } = string.Empty;

        public double Area { get; private set; }

        public string Location { get; private set; } = string.Empty;

        public List<AgricultiralEvent> Events { get; private set; } = new List<AgricultiralEvent>();
    
        public CropType CropType { get; private set; }

        public GreenhouseEntity(){}

        private GreenhouseEntity(
            string name,
            double area,
            string location,
            CropType cropType)
        {
            Name = name;
            Area = area;
            Location = location;
            CropType = cropType;
        }

        public static Result<GreenhouseEntity> Initialize(
            string name,
            double area,
            string location,
            CropType cropType)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<GreenhouseEntity>("Name is null");
            }

            if (string.IsNullOrWhiteSpace(location))
            {
                return Result.Failure<GreenhouseEntity>("location is null");
            }

            if(area <= 0)
            {
                return Result.Failure<GreenhouseEntity>("Area less or equals zero");
            }

            if(cropType is null)
            {
                return Result.Failure<GreenhouseEntity>("Croptype is null");
            }

            return Result.Success(
                new GreenhouseEntity(
                    name, area,
                    location, cropType));
        }
    }
}
