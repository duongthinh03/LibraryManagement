using Library.Domain.Common;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Domain.Queries;
using Library.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Persistence.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;
        private static readonly Dictionary<string, string> SortMappings = new(StringComparer.OrdinalIgnoreCase)
        {
            ["id"] = "Id",
            ["isbn"] = "Isbn",
            ["title"] = "Document.Title",
            ["publisher"] = "Document.Publisher",
            ["publishYear"] = "Document.PublishYear",
            ["language"] = "Document.Language",
            ["category"] = "Document.Category.CategoryName",
            ["pageCount"] = "PageCount"
        };

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<BookEntity>> GetAllAsync(BookQueryCriteria criteria)
        {
            var query = _context.Books
                .AsNoTracking()
                .Where(x => x.Document != null);

            if (!string.IsNullOrWhiteSpace(criteria.Search))
            {
                var keyword = $"%{criteria.Search.Trim()}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.Isbn!, keyword) ||
                    EF.Functions.Like(x.Document!.Title!, keyword) ||
                    EF.Functions.Like(x.Document!.Publisher!, keyword) ||
                    EF.Functions.Like(x.Document!.Language!, keyword)
                );
            }

            var filters = new Dictionary<string, object>();

            query = query.ApplyFilter(filters);

            if (criteria.FromYear.HasValue)
            {
                query = query.Where(x => x.Document!.PublishYear >= criteria.FromYear.Value);
            }

            if (criteria.ToYear.HasValue)
            {
                query = query.Where(x => x.Document!.PublishYear <= criteria.ToYear.Value);
            }

            var result = query
                .ApplySorting(ResolveSortBy(criteria.SortBy), criteria.SortDirection)
                .Select(x => new BookEntity
                {
                    Id = x.Id,
                    PageCount = x.PageCount,
                    Isbn = x.Isbn,
                    Document = new DocumentEntity
                    {
                        Title = x.Document!.Title,
                        Publisher = x.Document.Publisher,
                        PublishYear = x.Document.PublishYear,
                        Language = x.Document.Language,
                        Category = x.Document.Category == null ? null : new CategoryEntity
                        {
                            CategoryName = x.Document.Category.CategoryName
                        }
                    }
                });

            return await result.ToPagedResultAsync(criteria.PageNo, criteria.PageSize);
        }

        private static string ResolveSortBy(string? sortBy)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return "Id";
            }

            return SortMappings.TryGetValue(sortBy.Trim(), out var mappedSortBy) ? mappedSortBy : "Id";
        }
    }
}
