namespace Library.Application.Common.Requests
{
    public class PaginationRequest
    {
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
