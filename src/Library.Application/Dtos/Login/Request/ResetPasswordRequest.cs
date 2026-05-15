namespace Library.Application.Dtos.Login.Request
{
    public class ResetPasswordRequest
    {
        public string? Code { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
