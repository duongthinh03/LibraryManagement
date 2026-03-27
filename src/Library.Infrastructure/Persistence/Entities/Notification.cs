using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Persistence.Entities;

public partial class Notification
{
    public int Id { get; set; }

    public int? ReaderId { get; set; }

    public string? Message { get; set; }

    public bool? IsRead { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Reader? Reader { get; set; }
}
