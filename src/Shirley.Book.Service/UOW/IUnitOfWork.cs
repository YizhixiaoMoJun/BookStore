using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shirley.Book.Service.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        Task RollbackAsync();
        Task CommitAsync();
    }
}
