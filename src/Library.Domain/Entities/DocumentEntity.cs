using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class DocumentEntity
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

        public CategoryEntity? Category { get; set; }
    }
}
