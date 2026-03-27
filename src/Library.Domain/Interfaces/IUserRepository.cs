using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<UserAccountEntity?> GetByUsernameAsync(string username);
        Task<int> CreateAsync(UserAccountEntity user);
        Task<UserAccountEntity?> GetByVerifyTokenAsync(string token);
        Task UpdateAsync(UserAccountEntity user);
    }
}
