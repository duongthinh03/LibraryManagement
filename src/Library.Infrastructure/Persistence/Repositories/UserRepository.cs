using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryDbContext _context;

        public UserRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<UserAccountEntity?> GetByUsernameAsync(string username)
        {
            var user = await _context.UserAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null) return null;

            return new UserAccountEntity
            {
                Id = user.Id,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                Role = user.Role,
                ReaderId = user.ReaderId,
                CreatedAt = user.CreatedAt,
                Status = Enum.TryParse<AccountStatus>(user.Status, out var status) ? status : AccountStatus.Pending,
                VerifyToken = user.VerifyToken,
                VerifyTokenExpiredAt = user.VerifyTokenExpiredAt,
            };
        }

        public async Task<int> CreateAsync(UserAccountEntity user)
        {
            if (user.ReaderId <= 0)
                throw new Exception("ReaderId is required"); // tránh FK lỗi
            var entity = new UserAccount
            {
                Username = user.Username.ToLower(),
                PasswordHash = user.PasswordHash,
                Role = user.Role,
                ReaderId = user.ReaderId,
                CreatedAt = DateTime.UtcNow,
                Status = user.Status.ToString(),
                VerifyToken = user.VerifyToken,
                VerifyTokenExpiredAt = user.VerifyTokenExpiredAt,
            };

            await _context.UserAccounts.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<UserAccountEntity?> GetByVerifyTokenAsync(string token)
        {
            var user = await _context.UserAccounts.AsNoTracking().FirstOrDefaultAsync(x => x.VerifyToken == token);
            
            if (user == null) return null;

            return new UserAccountEntity
            {
                Id = user.Id,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                Role = user.Role,
                ReaderId = user.ReaderId,
                CreatedAt = user.CreatedAt,
                Status = Enum.TryParse<AccountStatus>(user.Status, out var status) ? status : AccountStatus.Pending,
                VerifyToken = user.VerifyToken,
                VerifyTokenExpiredAt = user.VerifyTokenExpiredAt,
             };
        }

        public async Task UpdateAsync(UserAccountEntity user)
        {
            var entity = await _context.UserAccounts.FindAsync(user.Id);

            if (entity == null)
                throw new Exception("User not found");

            entity.Status = user.Status.ToString();
            entity.VerifyToken = user.VerifyToken;
            entity.VerifyTokenExpiredAt = user.VerifyTokenExpiredAt;

            _context.UserAccounts.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
