using Application.Shared.Exceptions;
using Authorize.Application.Common.Intefaces;
using Authorize.Domain.Repositories;
using MediatR;

namespace Authorize.Application.Commands.User.LoginUser
{
    public class LoginUserHandler :
        IRequestHandler<LoginUserCommand, long>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHashService hashService;

        public LoginUserHandler(
            IUnitOfWork unitOfWork,
            IHashService hashService)
        {
            this.unitOfWork = unitOfWork;
            this.hashService = hashService;
        }

        public async Task<long> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository
                .GetActiveUser(request.Email);

            if (user is null)
            {
                throw new NotFoundException("User doesnt registered");
            }

            var isValidHash = hashService
                .VerifyHash(request.Password, user.PasswordHash);

            if (!isValidHash)
            {
                throw new BadRequestException("Incrorrect credentials or " +
                    "email does registered");
            }

            return user.Id;
        }
    }
}
