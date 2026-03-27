using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Persistence.Entities;

public partial class CopyTransaction
{
    public int Id { get; set; }

    public int? CopyId { get; set; }

    public string? Action { get; set; }

    public int? ReferenceId { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Copy? Copy { get; set; }
}
