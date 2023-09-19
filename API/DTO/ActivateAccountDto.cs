namespace API.DTO
{
    public class ActivateAccountDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ActivationLinkToken { get; set; }
    }
}