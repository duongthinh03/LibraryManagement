using Library.Application.Dtos.Login.Request;
using Library.Application.Dtos.Login.Response;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Security.Cryptography;

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

        public AuthService(
            IUserRepository userRepository,
            IJwtService jwtService,
            IPasswordHasher passwordHasher,
            IReaderRepository readerRepository,
            IEmailService emailService,
            IConfiguration configuration)
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

            if (user.Status != AccountStatus.Active)
            {
                throw new Exception("Account is not verified. Please check your email.");
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
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new Exception("Username is required");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new Exception("Password is required");

            if (request.Password != request.ConfirmPassword)
                throw new Exception("Password does not match");

            if (request.Password.Length < 6)
                throw new Exception("Password must be at least 6 characters");

            var username = request.Username.Trim().ToLower();

            var existed = await _userRepository.GetByUsernameAsync(username);
            if (existed != null)
                throw new Exception("Username already exists");

            var hash = _passwordHasher.Hash(request.Password);

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

            var verifyToken = Guid.NewGuid().ToString();

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

            var baseUrl = _config["App:BaseUrl"];
            var link = $"{baseUrl}/verify?token={verifyToken}";

            var body = $@"
            Hello {request.FullName},

            Your account registration was successful.

            Please click the link below to verify your account:

            {link}

            This link will expire in 1 hour.

            If you did not make this request, you can ignore this email.

            Regards,
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

            if (user == null || user.Status != AccountStatus.Pending)
                throw new Exception("Invalid token");

            if (user.VerifyTokenExpiredAt < DateTime.UtcNow)
                throw new Exception("Token expired");

            user.Status = AccountStatus.Active;
            user.VerifyToken = null;
            user.VerifyTokenExpiredAt = null;

            await _userRepository.UpdateAsync(user);
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new Exception("Email is required");

            var email = request.Email.Trim();
            var reader = await _readerRepository.GetByEmailAsync(email);

            if (reader == null)
            {
                return;
            }

            var user = await _userRepository.GetByReaderIdAsync(reader.Id);

            if (user == null || user.Status != AccountStatus.Active)
            {
                return;
            }

            user.VerifyToken = await GenerateUniqueResetCodeAsync();
            user.VerifyTokenExpiredAt = DateTime.UtcNow.AddHours(1);

            await _userRepository.UpdateAsync(user);

            var name = string.IsNullOrWhiteSpace(reader.FullName) ? user.Username : reader.FullName;

            var body = $@"
            Hello {name},

            We received a request to reset your password.

            Your reset code is:

            {user.VerifyToken}

            This code will expire in 1 hour.

            If you did not request a password reset, you can ignore this email.

            Regards,
            Library System
            ";

            await _emailService.SendEmailAsync(
                email,
                "Reset your password",
                body
            );
        }

        public async Task ResetPasswordAsync(ResetPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
                throw new Exception("Code is required");

            if (string.IsNullOrWhiteSpace(request.NewPassword))
                throw new Exception("New password is required");

            if (request.NewPassword.Length < 6)
                throw new Exception("Password must be at least 6 characters");

            if (request.NewPassword != request.ConfirmPassword)
                throw new Exception("Password does not match");

            var user = await _userRepository.GetByVerifyTokenAsync(request.Code.Trim());

            if (user == null || user.Status != AccountStatus.Active)
                throw new Exception("Invalid code");

            if (user.VerifyTokenExpiredAt < DateTime.UtcNow)
                throw new Exception("Code expired");

            user.PasswordHash = _passwordHasher.Hash(request.NewPassword);
            user.VerifyToken = null;
            user.VerifyTokenExpiredAt = null;

            await _userRepository.UpdateAsync(user);
        }

        private async Task<string> GenerateUniqueResetCodeAsync()
        {
            while (true)
            {
                var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
                var existed = await _userRepository.GetByVerifyTokenAsync(code);

                if (existed == null)
                {
                    return code;
                }
            }
        }
    }
}
