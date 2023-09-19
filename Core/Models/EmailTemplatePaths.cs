namespace Core.Models
{
    public static class EmailTemplatePaths
    {
        public static readonly (string TokenName, string Path) ConfirmEmail = ("confirm_email_token", "/api/account/confirmEmail");
        public static readonly (string TokenName, string Path) RecoverPassword = ("recover_password_token", "/api/account/resetPassword");
        public static readonly (string TokenName, string Path) AccountActivation = ("account_activation_token", "/api/account/setPasswordToAccount");
    }
}