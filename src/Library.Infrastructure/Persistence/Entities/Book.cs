using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Persistence.Entities;

public partial class Book
{
    public int Id { get; set; }

    public int? DocumentId { get; set; }

    public int? PageCount { get; set; }

    public string? Isbn { get; set; }

    public virtual Document? Document { get; set; }
}
