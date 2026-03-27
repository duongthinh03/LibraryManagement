using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Persistence.Entities;

public partial class BorrowRecord
{
    public int Id { get; set; }

    public int? ReaderId { get; set; }

    public int? CopyId { get; set; }

    public DateTime? BorrowDate { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Copy? Copy { get; set; }

    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();

    public virtual Reader? Reader { get; set; }
}
