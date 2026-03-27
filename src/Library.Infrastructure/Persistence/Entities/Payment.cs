using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Persistence.Entities;

public partial class Payment
{
    public int Id { get; set; }

    public int? FineId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? Method { get; set; }

    public virtual Fine? Fine { get; set; }
}
