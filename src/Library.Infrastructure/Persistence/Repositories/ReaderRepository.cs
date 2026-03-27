using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Persistence.Repositories
{
    public class ReaderRepository : IReaderRepository
    {
        private readonly LibraryDbContext _context;

        public ReaderRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<ReaderEntity?> GetByEmailAsync(string email)
        {
            var reader = await _context.Readers
                .FirstOrDefaultAsync(x => x.Email == email);

            if (reader == null) return null;

            return new ReaderEntity
            {
                ReaderCode = reader.ReaderCode,
                Id = reader.Id,
                FullName = reader.FullName,
                Email = reader.Email,
                Phone = reader.Phone,
                Address = reader.Address,
                CreatedAt = reader.CreatedAt ?? DateTime.UtcNow,
                IsActive = reader.IsActive ?? false
            };
        }

        public async Task<ReaderEntity> CreateAsync(ReaderEntity reader)
        {
            var entity = new Reader
            {
                ReaderCode = Guid.NewGuid().ToString(),
                FullName = reader.FullName,
                Email = reader.Email,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _context.Readers.AddAsync(entity);
            await _context.SaveChangesAsync();

            reader.Id = entity.Id;
            return reader;
        }
    }
}
