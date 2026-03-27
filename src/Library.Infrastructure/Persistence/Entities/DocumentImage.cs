using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Persistence.Entities;

public partial class DocumentImage
{
    public int Id { get; set; }

    public int? DocumentId { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsMain { get; set; }

    public virtual Document? Document { get; set; }
}
