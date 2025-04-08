using Application.Shared.Exceptions;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using Notification.Application.Common.Interfaces;
using Notification.Contracts.Email;
using Notification.Infastructure.Configurations;

namespace Notification.Infastructure.Implementations
{
    public class SendEmailService : INotificationService<SendEmailRequest>
    {
        private readonly ILogger<SendEmailService> logger;
        private readonly SmtpConf smtpConf;

        public SendEmailService(
            ILogger<SendEmailService> logger,
            IConfiguration configuration)
        {
            this.logger = logger;

            smtpConf = configuration.GetSection("EmailSettings")
                .Get<SmtpConf>() ??
                throw new AppConfigurationException("Smtp settings");
        }

        public async Task Send(SendEmailRequest message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(
                "Greenhouse", smtpConf.ReciverEmail));
            emailMessage.To.Add(new MailboxAddress("", message.Consumer));
            emailMessage.Subject = message.Title;
            emailMessage.Body = new TextPart
            {
                Text = message.Message
            };

            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync("smtp.mail.ru", 465);
                await client.AuthenticateAsync(
                    smtpConf.ReciverEmail,
                    smtpConf.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch
            {
                logger.LogError("Send email with error");
            }
        }
    }
}
