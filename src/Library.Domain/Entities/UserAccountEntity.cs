using Library.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class UserAccountEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public int? ReaderId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public AccountStatus Status { get; set; }
        public string? VerifyToken { get; set; }
        public DateTime? VerifyTokenExpiredAt { get; set; }
    }
}
