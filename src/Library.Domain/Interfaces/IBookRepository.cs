using Library.Domain.Entities;
using Library.Domain.Queries;
using Library.Domain.Common;

namespace Library.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<PagedResult<BookEntity>> GetAllAsync(BookQueryCriteria criteria);
    }
}
