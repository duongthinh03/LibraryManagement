using Library.Domain.Entities;

namespace Library.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<UserAccountEntity?> GetByUsernameAsync(string username);
        Task<UserAccountEntity?> GetByReaderIdAsync(int readerId);
        Task<int> CreateAsync(UserAccountEntity user);
        Task<UserAccountEntity?> GetByVerifyTokenAsync(string token);
        Task UpdateAsync(UserAccountEntity user);
    }
}
