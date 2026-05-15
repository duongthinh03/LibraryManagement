using Library.Domain.Entities;

namespace Library.Domain.Interfaces
{
    public interface IReaderRepository
    {
        Task<ReaderEntity?> GetByEmailAsync(string email);
        Task<ReaderEntity> CreateAsync(ReaderEntity reader);
    }
}
