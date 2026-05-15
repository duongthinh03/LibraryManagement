namespace Library.Domain.Queries
{
    public class BookQueryCriteria
    {
        public string? Search { get; set; }
        public string? Category { get; set; }
        public string? Language { get; set; }
        public int? FromYear { get; set; }
        public int? ToYear { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
