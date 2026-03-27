using Library.Application.Dtos.Login.Request;
using Library.Application.Dtos.Login.Response;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IReaderRepository _readerRepository;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepository, IJwtService jwtService, IPasswordHasher passwordHasher, 
                           IReaderRepository readerRepository, IEmailService emailService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _readerRepository = readerRepository;
            _emailService = emailService;
            _config = configuration;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Invalid username or password");
            }

            // CHẶN LOGIN nếu chưa verify
            if (user.Status != AccountStatus.Active)
            {
                throw new Exception("Tài khoản chưa được xác thực. Vui lòng kiểm tra email.");
            }

            var token = _jwtService.GenerateToken(user);

            return new AuthResponse
            {
                Token = token,
                Username = user.Username,
                Role = user.Role,
                ExpireAt = DateTime.UtcNow.AddHours(2)
            };
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            //1. VALIDATE
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new Exception("Username is required");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new Exception("Password is required");

            if (request.Password != request.ConfirmPassword)
                throw new Exception("Password does not match");

            if (request.Password.Length < 6)
                throw new Exception("Password must be at least 6 characters");

            var username = request.Username.Trim().ToLower();

            // 2. CHECK USER 
            var existed = await _userRepository.GetByUsernameAsync(username);
            if (existed != null)
                throw new Exception("Username already exists");

            // 3. HASH PASSWORD
            var hash = _passwordHasher.Hash(request.Password);

            // 4. CHECK / CREATE READER 
            var reader = await _readerRepository.GetByEmailAsync(request.Email);

            if (reader == null)
            {
                reader = new ReaderEntity
                {
                    ReaderCode = Guid.NewGuid().ToString(),
                    FullName = request.FullName,
                    Email = request.Email,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                reader = await _readerRepository.CreateAsync(reader); 
            }

            // 5. CREATE VERIFY TOKEN 
            var verifyToken = Guid.NewGuid().ToString();

            // 6. CREATE USER 
            var user = new UserAccountEntity
            {
                Username = username,
                PasswordHash = hash,
                Role = "User",
                ReaderId = reader.Id,
                CreatedAt = DateTime.UtcNow,
                Status = AccountStatus.Pending,
                VerifyToken = verifyToken,
                VerifyTokenExpiredAt = DateTime.UtcNow.AddHours(1)
            };

            await _userRepository.CreateAsync(user);

            // 7. SEND EMAIL 

            var baseUrl = _config["App:BaseUrl"];

            var link = $"{baseUrl}/verify?token={verifyToken}";

            var body = $@"
            Xin chào {request.FullName},

            Bạn đã đăng ký tài khoản thành công.

            Vui lòng click vào link bên dưới để xác thực tài khoản:

            {link}

            Link sẽ hết hạn sau 1 giờ !!!

            Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email.

            Trân trọng,
            Library System
            ";

            await _emailService.SendEmailAsync(
                request.Email,
                "Verify your account",
                body
            );
        }

        public async Task VerifyAccountAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("Token is required");

            var user = await _userRepository.GetByVerifyTokenAsync(token);

            if (user == null)
                throw new Exception("Invalid token");

            if (user.VerifyTokenExpiredAt < DateTime.UtcNow)
                throw new Exception("Token expired");

            user.Status = AccountStatus.Active;
            user.VerifyToken = null;
            user.VerifyTokenExpiredAt = null;

            await _userRepository.UpdateAsync(user);
        }
    }
}
