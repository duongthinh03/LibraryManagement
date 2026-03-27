using Library.Domain.Entities;

namespace Library.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<(List<BookEntity> Data, int TotalItems)> GetAllAsync(int pageNo, int pageSize);
    }
}
