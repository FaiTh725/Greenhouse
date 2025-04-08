using MediatR;

namespace Greenhouse.Application.Commands.GreenhouseEntity.CreateGreenhouse
{
    public class CreateGreenhouseCommand : IRequest<long>
    {
        public string GreenhouseName { get; set; } = string.Empty;

        public double Area { get; set; }

        public string Location { get; set; } = string.Empty;

        public string CropName {  get; set; } = string.Empty;
    }
}
