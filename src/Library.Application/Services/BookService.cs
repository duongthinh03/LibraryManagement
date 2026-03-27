using Library.Application.Common.Requests;
using Library.Application.Common.Responses;
using Library.Application.Dtos.Book;
using Library.Application.Interfaces;
using Library.Domain.Interfaces;

namespace Library.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<BasePaginationResponse<BookDto>> GetAllAsync( PaginationRequest request)
        {
            var (data, totalItems) = await _bookRepository.GetAllAsync(request.PageNo, request.PageSize);

            return new BasePaginationResponse<BookDto>
            {
                PageNo = request.PageNo,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                TotalPages = (totalItems + request.PageSize - 1) / request.PageSize,
                Data = data.Select(x => new BookDto
                {
                    Id = x.Id,
                    PageCount = x.PageCount,
                    Isbn = x.Isbn,
                }).ToList()
            };
        }
    }
}
