using Greenhouse.Application.Contracts.Employe;
using Greenhouse.Domain.Repositorires;
using MediatR;

namespace Greenhouse.Application.Queries.Employee.GetEmplyes
{
    public class GetEmplyesHandler :
        IRequestHandler<GetEmplyesQuery, IEnumerable<EmployeeResponse>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetEmplyesHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EmployeeResponse>> Handle(GetEmplyesQuery request, CancellationToken cancellationToken)
        {
            var employes = await unitOfWork.EmployeRepository
                .GetEmployes();

            return employes.Select(x => new EmployeeResponse 
            { 
                Email = x.Email,
                Id = x.Id,
                Name = x.Name
            });
        }
    }
}
