using Core.Models;

namespace Core.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(EmailTemplateSelectorParams templateParams);
    }
}