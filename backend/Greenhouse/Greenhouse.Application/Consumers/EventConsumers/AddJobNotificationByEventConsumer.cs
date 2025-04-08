using Greenhouse.Application.Common.BackgroundWrappers;
using Greenhouse.Application.Common.Intrefaces;
using Greenhouse.Application.Contracts.Event;
using MassTransit;
using Notification.Contracts.Email;

namespace Greenhouse.Application.Consumers.EventConsumers
{
    public class AddJobNotificationByEventConsumer :
        IConsumer<EventNotification>
    {
        private readonly IBus bus;
        private readonly IBackgroundService backgroundService;

        public AddJobNotificationByEventConsumer(
            IBus bus,
            IBackgroundService backgroundService)
        {
            this.bus = bus;
            this.backgroundService = backgroundService;
        }

        public Task Consume(ConsumeContext<EventNotification> context)
        {
            var sendEmailRequest = new SendEmailRequest
            {
                Consumer = context.Message.ExecuterEmail,
                Title = "Notification to execute job " + context.Message.EventName,
                Message = "After 15 minutes you have to complete a job"
            };

            if (context.Message.PlannedDate.AddMinutes(15) <
                DateTime.UtcNow)
            {
                sendEmailRequest.Message = "Now you have to compete a job";

                backgroundService.Enqueue<BusWrapper>(x =>
                    x.Publish(sendEmailRequest));
            }
            else
            {
                backgroundService.Schedule<BusWrapper>(x =>
                    x.Publish(sendEmailRequest),
                context.Message.PlannedDate.AddMinutes(-15));

                sendEmailRequest.Message = "After 30 minutes you have to complete a job";
                backgroundService.Schedule<BusWrapper>(x =>
                    x.Publish(sendEmailRequest),
                context.Message.PlannedDate.AddMinutes(-30));
            }

            return Task.CompletedTask;
        }
    }
}
