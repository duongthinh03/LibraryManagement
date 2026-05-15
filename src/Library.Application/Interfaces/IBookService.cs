using Library.Application.Common.Responses;
using Library.Application.Dtos.Book;

namespace Library.Application.Interfaces
{
    public interface IBookService
    {
        Task<BasePaginationResponse<BookListDto>> GetAllAsync(
            string? search,
            string? category,
            string? language,
            int? fromYear,
            int? toYear,
            string? sortBy,
            string? sortDirection,
            int pageNo,
            int pageSize);
    }
}
