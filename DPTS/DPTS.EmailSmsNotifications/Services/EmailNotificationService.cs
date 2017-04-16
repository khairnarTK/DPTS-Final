// using SendGrid's C# Library
// https://github.com/sendgrid/sendgrid-csharp
using DPTS.EmailSmsNotifications.IServices;
using DPTS.EmailSmsNotifications.ServiceModels;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace DPTS.EmailSmsNotifications.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private NotificationServiceConfig _config;

        public EmailNotificationService()
        {
            _config = new NotificationServiceConfig();

        }

        public async Task SendEmail(EmailNotificationModel model)
        {
            string apiKey = _config.SendGridApiKey;
            dynamic sg = new SendGridAPIClient(apiKey);

            Email from = new Email(model.from);
            //string subject = "Sending with SendGrid is Fun";
            string subject = model.subject;
            Email to = new Email(model.to);
            //Content content = new Content("text/plain", "and easy to do anywhere, even with C#");
            Content content = new Content("text/plain", model.content);
            Mail mail = new Mail(from, subject, to, content);

            dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
        }
    }
}