using Authorize.Domain.Events;
using MassTransit;
using MediatR;
using Notification.Contracts.Email;

namespace Authorize.Application.EventHandlers.User
{
    public class SendEmailToActiveUserEventHandler : INotificationHandler<ActiveUserEvent>
    {
        private readonly IBus bus;

        public SendEmailToActiveUserEventHandler(
            IBus bus)
        {
            this.bus = bus;
        }

        public async Task Handle(ActiveUserEvent notification, CancellationToken cancellationToken)
        {
            await bus.Publish(new SendEmailRequest
            {
                Consumer = notification.UserEmail,
                Title = "Welcome",
                Message = "Congratulations!! Your account activated and ready to use"
            });
        }
    }
}
