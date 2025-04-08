using CSharpFunctionalExtensions;

namespace Greenhouse.Domain.Entities
{
    public class CropType : Entity
    {
        public string Name { get; private set; } = string.Empty;

        public GreenhouseEntity Greenhouse {  get; private set; }
        public long GreenhouseId { get; private set; }

        public CropType() {}

        private CropType (
            string name)
        {
            Name = name;
        }

        public static Result<CropType> Initialize(
            string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<CropType>("Name is null");
            }

            return Result.Success(
                new CropType(name));
        }
    }
}
