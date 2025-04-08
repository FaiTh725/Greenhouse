using Application.Shared.Exceptions;
using Greenhouse.Application.Contracts.Greenhouse;
using Greenhouse.Domain.Repositorires;
using MediatR;

namespace Greenhouse.Application.Queries.Greenhouse.GetGreenhouesesPagination
{
    public class GetGreenhouesesPaginationHandler :
        IRequestHandler<GetGreenhouesesPaginationQuery, GreenhousePaginationResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetGreenhouesesPaginationHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<GreenhousePaginationResponse> Handle(
            GetGreenhouesesPaginationQuery request, CancellationToken cancellationToken)
        {
            if(request.Count <= 0 || 
                request.Page <= 0)
            {
                throw new BadRequestException("");
            }

            var greenhousePagination = await unitOfWork.GreenhouseRepository
                .GetGreenhouses(request.Page, request.Count);

            return new GreenhousePaginationResponse 
            { 
                MaxCount = greenhousePagination.maxCount,
                Greenhouses = greenhousePagination.data.Select(x => new GreenhouseResponse
                {
                    Id = x.Id,
                    Area = x.Area,
                    Location = x.Location,
                    NameGreenHouse = x.Name,
                    CropName = x.CropType.Name
                }).ToList()
            };
        }
    }
}
