using Application.Shared.Exceptions;
using Greenhouse.Application.Contracts.Greenhouse;
using Greenhouse.Domain.Repositorires;
using MediatR;

namespace Greenhouse.Application.Queries.Greenhouse.GetGreenhouseById
{
    public class GetGreenhouseByIdHandler :
        IRequestHandler<GetGreenhouseByIdQuery, GreenhouseResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetGreenhouseByIdHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<GreenhouseResponse> Handle(GetGreenhouseByIdQuery request, CancellationToken cancellationToken)
        {
            var greenhouse = await unitOfWork
                .GreenhouseRepository.GetGreenhouse(request.Id);
        
            if(greenhouse is null)
            {
                throw new NotFoundException("Greenhouse doesnt exist");
            }

            return new GreenhouseResponse
            {
                Id = greenhouse.Id,
                Location = greenhouse.Location,
                NameGreenHouse = greenhouse.Name,
                Area = greenhouse.Area,
                CropName = greenhouse.CropType.Name
            };
        }
    }
}
