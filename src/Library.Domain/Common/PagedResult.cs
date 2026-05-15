namespace Library.Domain.Common
{
    public class PagedResult<T>
    {
        public List<T> Data { get; set; } = new();
        public int TotalItems { get; set; }
    }
}
