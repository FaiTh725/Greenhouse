using MassTransit;
using Notification.Application.Common.Interfaces;
using Notification.Contracts.Email;

namespace Notification.Application.Consumers.Email
{
    public class SentEmailConsumer : IConsumer<SendEmailRequest>
    {
        private readonly INotificationService<SendEmailRequest> notificationService;

        public SentEmailConsumer(
            INotificationService<SendEmailRequest> notificationService)
        {
            this.notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<SendEmailRequest> context)
        {
            await notificationService.Send(context.Message);
        }
    }
}
