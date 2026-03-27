using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Persistence.Entities;

public partial class Thesis
{
    public int Id { get; set; }

    public int? DocumentId { get; set; }

    public string? StudentAuthor { get; set; }

    public string? Supervisor { get; set; }

    public string? ThesisType { get; set; }

    public virtual Document? Document { get; set; }
}
