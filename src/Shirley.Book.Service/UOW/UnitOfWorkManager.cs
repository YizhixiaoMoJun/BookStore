using Microsoft.EntityFrameworkCore;
using Shirley.Book.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shirley.Book.Service.UOW
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly BookContext bookContext;

        private IUnitOfWork current;

        public UnitOfWorkManager(BookContext bookContext)
        {
            this.bookContext = bookContext;
        }

        public IUnitOfWork Current => current;

        public async Task<IUnitOfWork> Reserve()
        {
            if(bookContext.Database.CurrentTransaction == null)
            {
                await bookContext.Database.BeginTransactionAsync();
            }

            current = new UnitOfWork(bookContext.Database.CurrentTransaction);
            return current;
        }
    }
}
