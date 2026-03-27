using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Persistence.Entities;

public partial class ReaderPolicy
{
    public string ReaderType { get; set; } = null!;

    public int? MaxBorrowCount { get; set; }

    public int? MaxBorrowDays { get; set; }

    public decimal? FinePerDay { get; set; }
}
