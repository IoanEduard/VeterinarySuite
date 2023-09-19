using Core.Models;
using Core.Models.enums;

namespace Core.Interfaces
{
    public interface IEmailTemplateGeneratorObject
    {
        EmailTemplateSelectorParams CreateEmailTemplateObject(string email, string subject, string token, EmailTemplateSelectorEnum template);
    }
}