
using Application.Shared.Exceptions;
using Authorize.Application.Common.Intefaces;
using Authorize.Domain.Repositories;
using MassTransit;
using MediatR;
using Notification.Contracts.Email;
using System.Security.Cryptography;

namespace Authorize.Application.Commands.User.ConfirmUserEmail
{
    public class ConfirmUserEmailHandler : IRequestHandler<ConfirmUserEmailCommand>
    {
        private readonly IBus bus;
        private readonly ICacheService cacheService;
        private readonly IUnitOfWork unitOfWork;

        public ConfirmUserEmailHandler(
            IBus bus,
            ICacheService cacheService, 
            IUnitOfWork unitOfWork)
        {
            this.bus = bus;
            this.cacheService = cacheService;
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(ConfirmUserEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository
                .GetUser(request.Email);
        
            if(user is not null)
            {
                user.Active();
                await unitOfWork.SaveChangesAsync();
                throw new ConflictException("User has alredy redistered");
            }

            var userCache = await cacheService
                .GetData<string>(request.Email);

            if(userCache.IsSuccess)
            {
                throw new ConflictException("Confirm request has already sent");
            }

            var randomNumbers = new byte[6];
            using var numberGenerator = RandomNumberGenerator.Create();
            numberGenerator.GetBytes(randomNumbers);

            var confirmCode = Convert.ToBase64String(randomNumbers);

            await cacheService
                .SetData(request.Email, confirmCode, 180);

            await bus.Publish(new SendEmailRequest
            {
                Consumer = request.Email,
                Message = $"Your confirm code is {confirmCode}",
                Title = "Confirm greenhouse email"
            });
        }
    }
}
