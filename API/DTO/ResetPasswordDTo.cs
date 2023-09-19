namespace API.DTO
{
    public class ResetPasswordDTo
    {
        public string Email{ get; set; }
        public string Password{ get; set; }
        public string Token { get; set; }
    }
}