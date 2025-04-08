using Application.Shared.Exceptions;
using Greenhouse.Application.Contracts.Event;
using Greenhouse.Application.Contracts.Greenhouse;
using Greenhouse.Domain.Repositorires;
using MediatR;

namespace Greenhouse.Application.Queries.Greenhouse.GetGreenhouseEvents
{
    public class GetGreenhouseEventsHandler :
        IRequestHandler<GetGreenhouseEventsQuery, GreenhouseEventsResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetGreenhouseEventsHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<GreenhouseEventsResponse> Handle(GetGreenhouseEventsQuery request, CancellationToken cancellationToken)
        {
            var greenhouse = await unitOfWork.GreenhouseRepository
                .GetGreenhouse(request.GreenhouseId);
        
            if(greenhouse is null)
            {
                throw new NotFoundException("Greenhouse doesnt exist");
            }

            var greenhouseEvents = await unitOfWork.GreenhouseRepository
                .GetGreenhouseEvents(greenhouse.Id);

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
