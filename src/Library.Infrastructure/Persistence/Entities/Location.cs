using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Persistence.Entities;

public partial class Location
{
    public int Id { get; set; }

    public string? LocationCode { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Copy> Copies { get; set; } = new List<Copy>();
}
