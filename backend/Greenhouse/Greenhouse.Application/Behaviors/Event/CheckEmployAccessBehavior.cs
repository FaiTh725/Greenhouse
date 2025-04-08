using Application.Shared.Exceptions;
using Greenhouse.Application.Common.BehaviorsInterfaces;
using Greenhouse.Domain.Repositorires;
using MediatR;

namespace Greenhouse.Application.Behaviors.Event
{
    public class CheckEmployAccessBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICheckEmployEvent
    {
        private readonly IUnitOfWork unitOfWork;

        public CheckEmployAccessBehavior(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            var eventEntity = await unitOfWork.AgricultiralEventRepository
                .GetAgricultiralEventWithExecuter(request.EventId);

            if (eventEntity is null)
            {
                throw new BadRequestException("Event doesnt exist");
            }

            if(eventEntity.Employee.Email != request.EmployeEmail &&
                request.Role != "Admin")
            {
                throw new ForbiddenAccessException("Only executer employe can manage events");
            }

            return await next();
        }
    }
}
