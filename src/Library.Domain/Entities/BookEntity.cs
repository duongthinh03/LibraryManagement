namespace Library.Domain.Entities
{
    public class BookEntity
    {
        public int Id { get; set; }
        public int? DocumentId { get; set; }
        public int? PageCount { get; set; }
        public string? Isbn { get; set; }
        public DocumentEntity? Document { get; set; }
    }
}
