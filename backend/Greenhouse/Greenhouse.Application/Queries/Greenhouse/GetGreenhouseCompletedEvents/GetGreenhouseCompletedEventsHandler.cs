using Application.Shared.Exceptions;
using Greenhouse.Application.Contracts.Event;
using Greenhouse.Application.Contracts.Greenhouse;
using Greenhouse.Domain.Repositorires;
using MediatR;

namespace Greenhouse.Application.Queries.Greenhouse.GetGreenhouseCompletedEvents
{
    public class GetGreenhouseCompletedEventsHandler :
        IRequestHandler<GetGreenhouseCompletedEventsQuery, GreenhouseEventsResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetGreenhouseCompletedEventsHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<GreenhouseEventsResponse> Handle(GetGreenhouseCompletedEventsQuery request, CancellationToken cancellationToken)
        {
            var greenhouse = await unitOfWork.GreenhouseRepository
                .GetGreenhouse(request.Id);
        
            if(greenhouse is null)
            {
                throw new NotFoundException("Greenhouse doesnt exist");
            }

            var greenhouseEvents = await unitOfWork.GreenhouseRepository
                .GetGreenhouseCompletedEvents(greenhouse.Id);

            return new GreenhouseEventsResponse
            {
                Id = greenhouse.Id,
                Events = greenhouseEvents.Select(x => new EventResponse
                {
                    GreenhouseId = x.GreenhouseId,
                    Id = x.Id,
                    ActualDate = x.ActualCompletedDate,
                    PlannedDate = x.PlannedDate,
                    EventStatus = x.EventStatus,
                    Name = x.Name,
                    ExecutingEmail = x.Employee.Email
                })
            };
        }
    }
}
