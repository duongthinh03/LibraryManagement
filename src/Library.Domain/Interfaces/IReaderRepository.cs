using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IReaderRepository
    {
        Task<ReaderEntity?> GetByEmailAsync(string email);
        Task<ReaderEntity> CreateAsync(ReaderEntity reader);
    }
}
