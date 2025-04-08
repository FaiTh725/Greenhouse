using Application.Shared.Exceptions;
using Greenhouse.Domain.Entities;
using Greenhouse.Domain.Repositorires;
using MediatR;
using GreenhouseModel = Greenhouse.Domain.Entities.GreenhouseEntity;

namespace Greenhouse.Application.Commands.GreenhouseEntity.CreateGreenhouse
{
    public class CreateGreenhouseHandler :
        IRequestHandler<CreateGreenhouseCommand, long>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateGreenhouseHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<long> Handle(CreateGreenhouseCommand request, CancellationToken cancellationToken)
        {
            var cropType = CropType.Initialize(request.CropName);

            if(cropType.IsFailure)
            {
                throw new BadRequestException("Crop Name is required");
            }

            var greenhouse = GreenhouseModel.Initialize(
                request.GreenhouseName,
                request.Area,
                request.Location,
                cropType.Value);

            if(greenhouse.IsFailure)
            {
                throw new BadRequestException(greenhouse.Error);
            }

            var greenhouseDb = await unitOfWork
                .GreenhouseRepository.AddGreenhouse(greenhouse.Value);

            await unitOfWork.SaveChangesAsync();

            return greenhouseDb.Id;
        }
    }
}
