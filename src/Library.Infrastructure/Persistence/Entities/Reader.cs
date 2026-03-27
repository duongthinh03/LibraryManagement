using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Persistence.Entities;

public partial class Reader
{
    public int Id { get; set; }

    public string ReaderCode { get; set; } = null!;

    public string? FullName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? ReaderType { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual UserAccount? UserAccount { get; set; }
}
