namespace Library.Application.Common.Requests
{
    public class BasePaginationRequest
    {
        private const int MaxPageSize = 100;

        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public void Normalize()
        {
            if (PageNo <= 0) PageNo = 1;

            if (PageSize <= 0) PageSize = 10;
            if (PageSize > MaxPageSize) PageSize = MaxPageSize;
        }
    }
}
