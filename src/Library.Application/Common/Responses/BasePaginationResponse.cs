namespace Library.Application.Common.Responses
{
    public class BasePaginationResponse<T>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public List<T> Data { get; set; } = new();
    }
}
