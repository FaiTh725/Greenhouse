using MediatR;

namespace Authorize.Application.Commands.User.ActiveUsers
{
    public class ActiveUsersCommand : IRequest
    {
        public List<string> UserEmails { get; set; } = new List<string>();
    }
}
