using Application.Shared.Exceptions;
using Greenhouse.Application.Contracts.Event;
using Greenhouse.Domain.Entities;
using Greenhouse.Domain.Repositorires;
using MassTransit;
using MediatR;

namespace Greenhouse.Application.Commands.Event.AddEvent
{
    public class AddEventHandler :
        IRequestHandler<AddEventCommand, long>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBus bus;

        public AddEventHandler(
            IUnitOfWork unitOfWork,
            IBus bus)
        {
            this.unitOfWork = unitOfWork;
            this.bus = bus;
        }

        public async Task<long> Handle(AddEventCommand request, CancellationToken cancellationToken)
        {
            var employe = await unitOfWork.EmployeRepository
                .GetEmploye(request.EmployeId);

            if (employe is null)
            {
                throw new BadRequestException("Employe doest exist");
            }

            var greenhouse = await unitOfWork.GreenhouseRepository
                .GetGreenhouse(request.GreenhouseId);
        
            if(greenhouse is null)
            {
                throw new BadRequestException("Greenhouse doesnt exist");
            }

            var eventEntity = AgricultiralEvent.Initialize(
                request.Name,
                request.EventType,
                request.PlannedDate,
                request.GreenhouseId,
                request.EmployeId);

            if(eventEntity.IsFailure)
            {
                throw new BadRequestException("Incorrect request - " + eventEntity.Error);
            }

            var eventDb = await unitOfWork.AgricultiralEventRepository
                .AddAgricultiralEvent(eventEntity.Value);

            await unitOfWork.SaveChangesAsync();

            await bus.Publish(new EventNotification
            {
                EventName = eventEntity.Value.Name,
                ExecuterEmail = employe.Email,
                PlannedDate = eventEntity.Value.PlannedDate
            });

            return eventDb.Id;
        }
    }
}
