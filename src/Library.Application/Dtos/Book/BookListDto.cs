namespace Library.Application.Dtos.Book
{
    public class BookListDto : BookDto
    {
        public string? Title { get; set; }
        public string? Category { get; set; }
        public string? Publisher { get; set; }
        public int? PublishYear { get; set; }
        public string? Language { get; set; }
    }
}
