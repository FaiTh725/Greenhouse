using Application.Shared.Exceptions;
using Greenhouse.Domain.Enums;
using Greenhouse.Domain.Repositorires;
using MediatR;

namespace Greenhouse.Application.Commands.Event.ProcessEvent
{
    public class ProcessEventHandler :
        IRequestHandler<ProcessEventCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public ProcessEventHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(ProcessEventCommand request, CancellationToken cancellationToken)
        {
            var eventEntity = await unitOfWork.AgricultiralEventRepository
                .GetAgricultiralEvent(request.EventId);
        
            if(eventEntity is null)
            {
                throw new BadRequestException("Event doesnt exist");
            }

            try
            {

                eventEntity.ChangeEventStatus(EventStatus.InProgress);
            }
            catch
            {
                throw new BadRequestException("Event has already in progress or completed");
            }

            await unitOfWork.AgricultiralEventRepository
                .UpdateEvent(eventEntity.Id, eventEntity);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
