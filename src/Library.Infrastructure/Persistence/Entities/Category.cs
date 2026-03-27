using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Persistence.Entities;

public partial class Category
{
    public int Id { get; set; }

    public string? CategoryName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
