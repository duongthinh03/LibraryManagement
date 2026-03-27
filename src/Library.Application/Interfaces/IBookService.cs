using Library.Application.Common.Requests;
using Library.Application.Common.Responses;
using Library.Application.Dtos.Book;


namespace Library.Application.Interfaces
{
    public interface IBookService
    {
        Task<BasePaginationResponse<BookDto>> GetAllAsync(PaginationRequest request);
    }
}
