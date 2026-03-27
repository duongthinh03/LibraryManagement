using Library.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using  Library.Domain.Entities;

namespace Library.Infrastructure.Persistence.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<(List<BookEntity> Data, int TotalItems)> GetAllAsync(int pageNo, int pageSize)
    {
        var query = _context.Books.AsQueryable();

        var TotalItems = await query.CountAsync();

        var data = await query.Skip((pageNo-1)* pageSize).Take(pageSize).Select( x => new BookEntity
        {
            Id = x.Id,
            DocumentId = x.DocumentId,
            PageCount = x.PageCount,
            Isbn = x.Isbn
        }).ToListAsync();

        return (data, TotalItems);
    }
}