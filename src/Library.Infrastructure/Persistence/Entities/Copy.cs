using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Persistence.Entities;

public partial class Copy
{
    public int Id { get; set; }

    public string? CopyCode { get; set; }

    public int? DocumentId { get; set; }

    public string? Status { get; set; }

    public int? LocationId { get; set; }

    public string? Condition { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();

    public virtual ICollection<CopyTransaction> CopyTransactions { get; set; } = new List<CopyTransaction>();

    public virtual Document? Document { get; set; }

    public virtual Location? Location { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
