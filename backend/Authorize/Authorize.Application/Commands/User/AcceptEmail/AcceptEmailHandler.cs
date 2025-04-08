
using Application.Shared.Exceptions;
using Authorize.Application.Common.Intefaces;
using MediatR;

namespace Authorize.Application.Commands.User.AcceptEmail
{
    public class AcceptEmailHandler :
        IRequestHandler<AcceptEmailCommand>
    {
        private readonly ICacheService cacheService;

        public AcceptEmailHandler(
            ICacheService cacheService)
        {
            this.cacheService = cacheService;
        }

        public async Task Handle(AcceptEmailCommand request, CancellationToken cancellationToken)
        {
            var userCode = await cacheService
                .GetData<string>(request.UserEmail);

            if(userCode.IsFailure ||
                userCode.Value != request.Code)
            {
                throw new NotFoundException("Incorrect code");
            }

            await cacheService.RemoveData(request.UserEmail);
            await cacheService.SetData("confirmed:" + request.UserEmail, "confirmed", 10800);
        }
    }
}
