using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class BookEntity
    {
        public int Id { get; set; }
        public int? DocumentId { get; set; }
        public int? PageCount { get; set; }
        public string? Isbn { get; set; }
    }
}
