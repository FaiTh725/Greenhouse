using Application.Shared.Exceptions;
using Authorize.Application.Contracts.User;
using Authorize.Domain.Repositories;
using MediatR;

namespace Authorize.Application.Queries.User.GetUserById
{
    public class GetUserByIdHandler :
        IRequestHandler<GetUserByIdQuery, UserResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetUserByIdHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository
                .GetUser(request.Id);

            if (user is null)
            {
                throw new NotFoundException("User doesnt exist");
            }

            return new UserResponse
            {
                Email = user.Email,
                Role = user.Role.Name
            };
        }
    }
}
