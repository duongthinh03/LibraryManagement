using Library.Application.Common.Requests;

namespace Library.Application.Dtos.Book.Request
{
    public class BookQueryRequest : BasePaginationRequest
    {
        public string? Search { get; set; }
        public string? Category { get; set; }
        public string? Language { get; set; }
        public int? FromYear { get; set; }
        public int? ToYear { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }
}
