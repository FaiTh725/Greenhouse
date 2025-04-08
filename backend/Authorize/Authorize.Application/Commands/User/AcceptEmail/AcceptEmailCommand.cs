using MediatR;

namespace Authorize.Application.Commands.User.AcceptEmail
{
    public class AcceptEmailCommand : IRequest
    {
        public string UserEmail { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;    
    }
}
