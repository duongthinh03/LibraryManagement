using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Persistence.Entities;

public partial class AuditLog
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Action { get; set; }

    public string? EntityName { get; set; }

    public int? EntityId { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? Description { get; set; }
}
