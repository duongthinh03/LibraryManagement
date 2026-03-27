using Library.Application.Dtos.Login.Request;
using Library.Application.Dtos.Login.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest Request);
        Task RegisterAsync(RegisterRequest request);
        Task VerifyAccountAsync(string token);
    }
}
