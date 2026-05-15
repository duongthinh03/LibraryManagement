using Library.Application.Dtos.Login.Request;
using Library.Application.Dtos.Login.Response;

namespace Library.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task RegisterAsync(RegisterRequest request);
        Task VerifyAccountAsync(string token);
        Task ForgotPasswordAsync(ForgotPasswordRequest request);
        Task ResetPasswordAsync(ResetPasswordRequest request);
    }
}
