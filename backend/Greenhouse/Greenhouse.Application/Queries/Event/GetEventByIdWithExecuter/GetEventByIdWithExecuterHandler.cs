using Application.Shared.Exceptions;
using Greenhouse.Application.Contracts.Event;
using Greenhouse.Domain.Repositorires;
using MediatR;

namespace Greenhouse.Application.Queries.Event.GetEventByIdWithExecuter
{
    public class GetEventByIdWithExecuterHandler :
        IRequestHandler<GetEventByIdWithExecuterQuery, EventResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetEventByIdWithExecuterHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<EventResponse> Handle(GetEventByIdWithExecuterQuery request, CancellationToken cancellationToken)
        {
            var eventEntity = await unitOfWork.AgricultiralEventRepository
                .GetAgricultiralEventWithExecuter(request.Id);
        
            if(eventEntity is null)
            {
                throw new NotFoundException("Event doesnt exist");
            }

            return new EventResponse
            {
                Id = eventEntity.Id,
                Name = eventEntity.Name,
                ActualDate = eventEntity.ActualCompletedDate,
                PlannedDate = eventEntity.PlannedDate,
                EventStatus = eventEntity.EventStatus,
                ExecutingEmail = eventEntity.Employee.Email,
                GreenhouseId = eventEntity.GreenhouseId
            };
        }
    }
}
