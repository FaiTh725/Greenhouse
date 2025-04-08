using Application.Shared.Exceptions;
using Authorize.Application.Common.Intefaces;
using Authorize.Domain.Repositories;
using MediatR;
using UserEntity = Authorize.Domain.Entities.User;

namespace Authorize.Application.Commands.User.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, long>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICacheService cacheService;
        private readonly IHashService hashService;

        public RegisterUserHandler(
            IUnitOfWork unitOfWork,
            ICacheService cacheService,
            IHashService hashService)
        {
            this.unitOfWork = unitOfWork;
            this.cacheService = cacheService;
            this.hashService = hashService;
        }

        public async Task<long> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var validToRegister = await cacheService
                .GetData<string>("confirmed:" + request.Email);

            if(validToRegister.IsFailure)
            {
                throw new ConflictException("Email have to confirmed");
            }

            var userExist = await unitOfWork
                .UserRepository.GetUser(request.Email);

            if (userExist is not null)
            {
                throw new ConflictException("Email has alredy registered");
            }

            var managerRole = await unitOfWork.RoleRepository
                .GetRole("Manager") ?? 
                throw new InternalServerApiException("Unknown error with registration");

            if(!UserEntity.IsValidPassword(request.Password))
            {
                throw new BadRequestException("Incorrect password, " + 
                    "should has 1 letter and 1 number, length from " +
                    UserEntity.MIN_PASSWORD_LENGHT + " to " +
                    UserEntity.MAX_PASSWORD_LENGHT);
            }

            var passwordHash = hashService.GenerateHash(request.Password);

            var userEntity = UserEntity.Initialize(
                request.Email, passwordHash, 
                managerRole, request.Name);

            if(userEntity.IsFailure)
            {
                throw new BadRequestException(userEntity.Error);
            }

            var newUser = await unitOfWork.UserRepository.AddUser(userEntity.Value);

            await unitOfWork.SaveChangesAsync();

            await cacheService.RemoveData("confirmed:" + request.Email);

            return newUser.Id;
        }
    }
}
