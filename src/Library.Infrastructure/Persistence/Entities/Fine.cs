using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Persistence.Entities;

public partial class Fine
{
    public int Id { get; set; }

    public int? BorrowRecordId { get; set; }

    public int? LateDays { get; set; }

    public decimal? Amount { get; set; }

    public decimal? PaidAmount { get; set; }

    public bool? IsPaid { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual BorrowRecord? BorrowRecord { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
