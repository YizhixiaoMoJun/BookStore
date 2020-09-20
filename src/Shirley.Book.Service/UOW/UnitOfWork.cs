using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shirley.Book.Service.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContextTransaction dbContextTransaction;

        public UnitOfWork(IDbContextTransaction dbContextTransaction)
        {
            this.dbContextTransaction = dbContextTransaction;
        }

        public async Task CommitAsync()
        {
           await dbContextTransaction.CommitAsync();
        }

        public void Dispose()
        {
            dbContextTransaction.Dispose();
        }

        public async Task RollbackAsync()
        {
            await dbContextTransaction.RollbackAsync();
        }
    }
}
