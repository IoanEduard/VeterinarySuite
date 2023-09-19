using Core.Interfaces;
using Core.Models;
using Core.Models.enums;
using Microsoft.Extensions.Options;

namespace API.Helpers
{
    public class EmailTemplateGeneratorObject : IEmailTemplateGeneratorObject
    {
        private readonly DevelopmentBackendPaths _developmentBackendPaths;

        public EmailTemplateGeneratorObject(IOptions<DevelopmentBackendPaths> developmentBackendPaths)
        {
            _developmentBackendPaths = developmentBackendPaths.Value;
        }

        public EmailTemplateSelectorParams CreateEmailTemplateObject(string email, string subject, string token, EmailTemplateSelectorEnum template)
        {

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Email and token must not be null or empty.");
            }

            var templateParams = new EmailTemplateSelectorParams
            {
                Email = email,
                Subject = subject,
                EmailTemplateEnum = template
            };

            switch (template)
            {
                case EmailTemplateSelectorEnum.ConfirmEmail:
                    templateParams.ConfirmationLink = UriBuilder(
                        EmailTemplatePaths.ConfirmEmail.TokenName,
                        EmailTemplatePaths.ConfirmEmail.Path,
                        token,
                        email
                    );
                    break;
                case EmailTemplateSelectorEnum.RecoverPassword:
                    templateParams.RecoverPasswordLink = UriBuilder(
                        EmailTemplatePaths.RecoverPassword.TokenName,
                        EmailTemplatePaths.RecoverPassword.Path,
                        token,
                        email
                    );
                    break;
                case EmailTemplateSelectorEnum.AccountActivation:
                    templateParams.AccountActivationLink = UriBuilder(
                        EmailTemplatePaths.AccountActivation.TokenName,
                        EmailTemplatePaths.AccountActivation.Path,
                        token,
                        email
                    );
                    break;
                default:
                    throw new ArgumentException("Invalid email template enum value");
            }

            return templateParams;
        }

        private string UriBuilder(string tokenName, string path, string token, string email)
        {
            return new UriBuilder
            {
                Scheme = _developmentBackendPaths.Scheme,
                Host = _developmentBackendPaths.Host,
                Port = _developmentBackendPaths.Port,
                Path = path,
                Query = $"{tokenName}={token}&email={email}"
            }.ToString();
        }
    }
}