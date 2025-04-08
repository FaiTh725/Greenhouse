using Application.Shared.Exceptions;
using Greenhouse.Application.Contracts.Event;
using Greenhouse.Domain.Repositorires;
using MediatR;

namespace Greenhouse.Application.Queries.Event.GetEventOfEmploye
{
    public class GetEventOfEmployeHandler :
        IRequestHandler<GetEventOfEmployeQuery, IEnumerable<EventResponse>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetEventOfEmployeHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EventResponse>> Handle(
            GetEventOfEmployeQuery request, 
            CancellationToken cancellationToken)
        {
            var employe = await unitOfWork.EmployeRepository
                .GetEmploye(request.EmployeEmail);
        
            if(employe is null)
            {
                throw new BadRequestException("Employe doesnt exist");
            }

            var employeEvents = await unitOfWork.AgricultiralEventRepository
                .GetAgricultiralEventsOfEmploye(employe.Id);

            return employeEvents.Select(x => new EventResponse
            { 
                Id = x.Id,
                ActualDate = x.ActualCompletedDate,
                Name = x.Name,
                EventStatus = x.EventStatus,
                ExecutingEmail = employe.Email,
                PlannedDate = x.PlannedDate,
                GreenhouseId = x.GreenhouseId
            });

        }
    }
}
