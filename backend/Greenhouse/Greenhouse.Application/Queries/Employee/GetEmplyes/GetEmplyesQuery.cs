using Greenhouse.Application.Common.BehaviorsInterfaces;
using Greenhouse.Application.Contracts.Employe;
using MediatR;

namespace Greenhouse.Application.Queries.Employee.GetEmplyes
{
    public class GetEmplyesQuery :
        IRequest<IEnumerable<EmployeeResponse>>,
        ICacheQuery
    {
        public string Key => "Employes";

        public int ExpirationSeconds => 120;
    }
}
