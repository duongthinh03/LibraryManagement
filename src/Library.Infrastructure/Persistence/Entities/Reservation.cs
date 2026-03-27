using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Persistence.Entities;

public partial class Reservation
{
    public int Id { get; set; }

    public int? ReaderId { get; set; }

    public int? DocumentId { get; set; }

    public int? CopyId { get; set; }

    public DateTime? ReservationDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? Status { get; set; }

    public int? Priority { get; set; }

    public virtual Copy? Copy { get; set; }

    public virtual Document? Document { get; set; }

    public virtual Reader? Reader { get; set; }
}
