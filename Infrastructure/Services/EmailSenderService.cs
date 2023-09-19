
using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly SendGridSettings _sendGridSettings;
        public EmailSenderService(ISendGridClient sendGridClient,
        
        IOptions<SendGridSettings> sendGridSettings)
        {
            _sendGridClient = sendGridClient;
            _sendGridSettings = sendGridSettings.Value;
        }
        public async Task SendEmailAsync(EmailTemplateSelectorParams templateParams)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_sendGridSettings.FromEmail, _sendGridSettings.EmailName),
                Subject = templateParams.Subject,
                HtmlContent = EmailTemplateSelector.GetEmailTemplate(templateParams)
            };
            msg.AddTo(templateParams.Email);
            await _sendGridClient.SendEmailAsync(msg);
        }
    }


}