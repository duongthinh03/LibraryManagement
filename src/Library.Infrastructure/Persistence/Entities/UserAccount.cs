using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Persistence.Entities;

public partial class UserAccount
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? PasswordHash { get; set; }

    public string? Role { get; set; }

    public int? ReaderId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Status { get; set; }

    public string? VerifyToken { get; set; }

    public DateTime? VerifyTokenExpiredAt { get; set; }

    public virtual Reader? Reader { get; set; }
}
