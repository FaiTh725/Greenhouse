using MediatR;

namespace Authorize.Application.Commands.User.RegisterUser
{
    public class RegisterUserCommand : IRequest<long>
    {
        public string Email { get; set; } = string.Empty;   

        public string? Name { get; set; }

        public string Password { get; set; } = string.Empty;
    }
}
