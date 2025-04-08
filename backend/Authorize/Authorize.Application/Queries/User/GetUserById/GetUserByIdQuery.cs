using Authorize.Application.Contracts.User;
using MediatR;

namespace Authorize.Application.Queries.User.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserResponse>
    {
        public long Id { get; set; }    
    }
}
