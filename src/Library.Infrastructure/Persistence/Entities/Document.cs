using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Persistence.Entities;

public partial class Document
{
    public int Id { get; set; }

    public string DocumentCode { get; set; } = null!;

    public string? Title { get; set; }

    public int? PublishYear { get; set; }

    public string? Publisher { get; set; }

    public string? DocumentType { get; set; }

    public string? Description { get; set; }

    public string? Language { get; set; }

    public int? CategoryId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Copy> Copies { get; set; } = new List<Copy>();

    public virtual ICollection<DocumentImage> DocumentImages { get; set; } = new List<DocumentImage>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual Thesis? Thesis { get; set; }

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
}
