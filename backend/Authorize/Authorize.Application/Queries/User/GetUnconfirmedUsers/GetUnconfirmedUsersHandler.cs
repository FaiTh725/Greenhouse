using Authorize.Application.Contracts.User;
using Authorize.Domain.Repositories;
using MediatR;

namespace Authorize.Application.Queries.User.GetUnconfirmedUsers
{
    public class GetUnconfirmedUsersHandler :
        IRequestHandler<GetUnconfirmedUsersQuery, IEnumerable<UserDataResponse>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetUnconfirmedUsersHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserDataResponse>> Handle(GetUnconfirmedUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await unitOfWork.UserRepository
                .GetUnconfirmedUsers();

            return users.Select(x => new UserDataResponse
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name ?? ""
            });
        }
    }
}
