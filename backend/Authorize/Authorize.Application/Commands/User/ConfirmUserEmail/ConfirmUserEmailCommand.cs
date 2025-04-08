using MediatR;

namespace Authorize.Application.Commands.User.ConfirmUserEmail
{
    public class ConfirmUserEmailCommand : IRequest
    {
        public string Email { get; set; } = string.Empty;
    }
}
