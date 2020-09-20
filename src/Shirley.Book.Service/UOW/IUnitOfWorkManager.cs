using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shirley.Book.Service.UOW
{
    public interface IUnitOfWorkManager
    {
        IUnitOfWork Current { get; }
        Task<IUnitOfWork> Reserve();
    }
}
