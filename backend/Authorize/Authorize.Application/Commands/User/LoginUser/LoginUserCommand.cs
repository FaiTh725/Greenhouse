using MediatR;

namespace Authorize.Application.Commands.User.LoginUser
{
    public class LoginUserCommand : IRequest<long>
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
