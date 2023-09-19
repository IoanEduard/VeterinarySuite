
using Core.Models.enums;

namespace Core.Models
{
    public class EmailTemplateSelectorParams
    {

        public string Subject { get; set; }
        public string Email { get; set; }
        public EmailTemplateSelectorEnum EmailTemplateEnum { get; set; }
        public string ConfirmationLink { get; set; }
        public string RecoverPasswordLink { get; set; }
        public string AccountActivationLink { get; set; }
    }
}