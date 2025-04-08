using CSharpFunctionalExtensions;
using Greenhouse.Domain.Enums;

namespace Greenhouse.Domain.Entities
{
    public class Resource : Entity
    {
        public string Name { get; private set; } = string.Empty;

        public string Unit { get; private set; } = string.Empty;
    
        public ResourceType ResourceType { get; private set; }

        public AgricultiralEventResource AgricultiralEventResource { get; private set; }
        
        public long AgricultiralEventResourceId {  get; private set; }

        public Resource() { }

        private Resource(
            string name, 
            string unit, 
            ResourceType resourceType)
        {
            Name = name;
            Unit = unit;
            ResourceType = resourceType;
        }

        public static Result<Resource> Initialize(
            string name,
            string unit,
            ResourceType resourceType)
        {
            if(string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(unit))
            {
                return Result.Failure<Resource>("Name or Unit is null or empty");
            }

            return Result.Success(new Resource(
                name, unit, resourceType));
        }
    }
}
