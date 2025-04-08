using Application.Shared.Exceptions;
using Greenhouse.Domain.Enums;
using Greenhouse.Domain.Repositorires;
using MediatR;

namespace Greenhouse.Application.Commands.Event.CancelEvent
{
    public class CancelEventHandler :
        IRequestHandler<CancelEventCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public CancelEventHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(CancelEventCommand request, CancellationToken cancellationToken)
        {
            var getEventEntityTasks = request.IdList.Select(x => unitOfWork.AgricultiralEventRepository
                .GetAgricultiralEvent(x));

            var events = await Task.WhenAll(getEventEntityTasks);
        
            if(events.Any(x => x is null || 
                x.EventStatus != EventStatus.Planned))
            {
                throw new BadRequestException("Any events doest exist or " +
                    "event already completed");
            }

            await unitOfWork.AgricultiralEventRepository
                .CancelEvents(request.IdList);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
