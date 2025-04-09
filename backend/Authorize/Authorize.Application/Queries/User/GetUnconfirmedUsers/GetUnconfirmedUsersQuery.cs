using Authorize.Application.Contracts.User;
using MediatR;

namespace Authorize.Application.Queries.User.GetUnconfirmedUsers
{
    public class GetUnconfirmedUsersQuery : 
        IRequest<IEnumerable<UserDataResponse>>
    {
    }
}
