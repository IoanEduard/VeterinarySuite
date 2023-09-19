
using Core.Models.enums;

namespace Core.Models
{
    public static class EmailTemplateSelector
    {
        public static string GetEmailTemplate(EmailTemplateSelectorParams templateParams)
        {
            switch (templateParams.EmailTemplateEnum)
            {
                case EmailTemplateSelectorEnum.ConfirmEmail:
                    return GenerateConfirmEmailTemplate(templateParams.ConfirmationLink);
                case EmailTemplateSelectorEnum.RecoverPassword:
                    return GenerateRecoverPasswordTemplate(templateParams.RecoverPasswordLink);
                case EmailTemplateSelectorEnum.AccountActivation:
                    return AccountActivationTemplate(templateParams.AccountActivationLink);
                default:
                    throw new ArgumentException("Invalid email template enum value");
            }
        }

        private static string GenerateConfirmEmailTemplate(string confirmationLink)
        {
            return $@"<html>
                        <head>
                            <meta charset=""utf-8"">
                            <title>Email Confirmation</title>
                        </head>
                        <body>
                            <div style=""font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;"">
                                <div style=""background-color: #ffffff; padding: 20px; border-radius: 5px;"">
                                    <h2>Email Confirmation</h2>
                                    <p>Dear [UserFirstName],</p>
                                    <p>Thank you for signing up with our service. To activate your account, please click the button below:</p>
                                    <p><a href=""{confirmationLink}"" style=""display: inline-block; padding: 10px 20px; background-color: #007bff; color: #ffffff; text-decoration: none; border-radius: 3px;"">Confirm Email</a></p>
                                    <p>If the button above does not work, you can also copy and paste the following link into your web browser:</p>
                                    <p>{confirmationLink}</p>
                                    <p>This link will expire in [ExpirationTime].</p>
                                    <p>If you did not sign up for our service, please ignore this email.</p>
                                    <p>Thank you,</p>
                                    <p>The Veterinary-Suite Team</p>
                                </div>
                            </div>
                        </body>
                    </html>";
        }

        private static string GenerateRecoverPasswordTemplate(string resetPasswordLink)
        {
            return $@"<html>
                        <head>
                            <meta charset=""utf-8"">
                            <title>Password Reset</title>
                        </head>
                        <body>
                            <div style=""font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;"">
                                <div style=""background-color: #ffffff; padding: 20px; border-radius: 5px;"">
                                    <h2>Password Reset</h2>
                                    <p>Dear [UserFirstName],</p>
                                    <p>We received a request to reset your password. To proceed with the password reset, please click the button below:</p>
                                    <p><a href=""{resetPasswordLink}"" style=""display: inline-block; padding: 10px 20px; background-color: #007bff; color: #ffffff; text-decoration: none; border-radius: 3px;"">Reset Password</a></p>
                                    <p>If the button above does not work, you can also copy and paste the following link into your web browser:</p>
                                    <p>{resetPasswordLink}</p>
                                    <p>This link will expire in [ExpirationTime].</p>
                                    <p>If you did not request a password reset, please ignore this email.</p>
                                    <p>Thank you,</p>
                                    <p>The Veterinary-Suite Team</p>
                                </div>
                            </div>
                        </body>
                    </html>";
        }

        private static string AccountActivationTemplate(string confirmationLink)
        {
            return $@"<html>
                        <head>
                            <meta charset=""utf-8"">
                            <title>Welcome to the Team</title>
                        </head>
                        <body>
                            <div style=""font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;"">
                                <div style=""background-color: #ffffff; padding: 20px; border-radius: 5px;"">
                                    <h2>Welcome to the Team, [UserFirstName]!</h2>
                                    <p>We are thrilled to have you on board. You've been added to our team by [BossName], and your account is ready to go. To activate your account and set your password, please click the button below:</p>
                                    <p><a href=""{confirmationLink}"" style=""display: inline-block; padding: 10px 20px; background-color: #007bff; color: #ffffff; text-decoration: none; border-radius: 3px;"">Activate Account</a></p>
                                    <p>If the button above does not work, you can also copy and paste the following link into your web browser:</p>
                                    <p>""{confirmationLink}""</p>
                                    <p>This link will expire in [ExpirationTime].</p>
                                    <p>If you have any questions or need assistance, please don't hesitate to contact us. Welcome to the team!</p>
                                    <p>Thank you,</p>
                                    <p>The [YourCompany] Team</p>
                                </div>
                            </div>
                        </body>
                        </html>";
        }
    }
}