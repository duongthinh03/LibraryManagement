using Library.Application.Common.Responses;
using Library.Application.Dtos.Book;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Domain.Queries;

namespace Library.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<BasePaginationResponse<BookListDto>> GetAllAsync(
            string? search,
            string? category,
            string? language,
            int? fromYear,
            int? toYear,
            string? sortBy,
            string? sortDirection,
            int pageNo,
            int pageSize)
        {
            var criteria = new BookQueryCriteria
            {
                Search = search,
                Category = category,
                Language = language,
                FromYear = fromYear,
                ToYear = toYear,
                SortBy = sortBy,
                SortDirection = sortDirection,
                PageNo = pageNo,
                PageSize = pageSize
            };

            var result = await _bookRepository.GetAllAsync(criteria);

            return new BasePaginationResponse<BookListDto>
            {
                PageNo = pageNo,
                PageSize = pageSize,
                TotalItems = result.TotalItems,
                TotalPages = (int)Math.Ceiling((double)result.TotalItems / pageSize),
                Data = result.Data.Select(MapBook).ToList()
            };
        }

        private static BookListDto MapBook(BookEntity book)
        {
            return new BookListDto
            {
                Id = book.Id,
                PageCount = book.PageCount,
                Isbn = book.Isbn,
                Title = book.Document?.Title,
                Publisher = book.Document?.Publisher,
                PublishYear = book.Document?.PublishYear,
                Language = book.Document?.Language,
                Category = book.Document?.Category?.CategoryName
            };
        }
    }
}
