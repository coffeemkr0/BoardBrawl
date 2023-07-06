using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BoardBrawl.WebApp.MVC.Utils
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;
        private string? _sendGridKey;

        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
            _sendGridKey = Environment.GetEnvironmentVariable("BoardBrawl_SendGrid_Key");
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(_sendGridKey))
            {
                throw new Exception("Null SendGridKey");
            }
            await Execute(_sendGridKey, subject, message, toEmail);
        }

        public async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("support@boardbrawl.com", "Board Brawl Support"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            _logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Failure Email to {toEmail}");
        }
    }
}