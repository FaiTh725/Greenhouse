using Greenhouse.Application.Common.BehaviorsInterfaces;
using Greenhouse.Application.Contracts.Event;
using MediatR;

namespace Greenhouse.Application.Queries.Event.GetEventOfEmploye
{
    public class GetEventOfEmployeQuery : 
        IRequest<IEnumerable<EventResponse>>,
        ICacheQuery
    {
        public string EmployeEmail { get; set; } = string.Empty;

        public string Key => "EmployeEvents:" + EmployeEmail;

        public int ExpirationSeconds => 180;
    }
}
