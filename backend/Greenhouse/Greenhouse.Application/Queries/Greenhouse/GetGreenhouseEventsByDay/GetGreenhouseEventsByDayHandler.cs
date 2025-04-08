using Application.Shared.Exceptions;
using Greenhouse.Application.Contracts.Event;
using Greenhouse.Application.Contracts.Greenhouse;
using Greenhouse.Domain.Repositorires;
using MediatR;

namespace Greenhouse.Application.Queries.Greenhouse.GetGreenhouseEventsByDay
{
    public class GetGreenhouseEventsByDayHandler :
        IRequestHandler<GetGreenhouseEventsByDayQuery, GreenhouseEventsResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetGreenhouseEventsByDayHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<GreenhouseEventsResponse> Handle(
            GetGreenhouseEventsByDayQuery request, 
            CancellationToken cancellationToken)
        {
            var greenhouse = await unitOfWork.GreenhouseRepository
                .GetGreenhouse(request.GreenhouseId);

            if(greenhouse is null)
            {
                throw new NotFoundException("Greenhouse doesnt exist");
            }

            var events = await unitOfWork.GreenhouseRepository
                .GetGreenhouseEvents(request.GreenhouseId, request.EventsDay);

            return new GreenhouseEventsResponse
            {
                Id = greenhouse.Id,
                Events = events.Select(x => new EventResponse
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
