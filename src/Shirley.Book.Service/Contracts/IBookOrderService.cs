using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shirley.Book.Service.Contracts
{
    public interface IBookOrderService
    {
        Task<string> OrderBook(BookOrderViewModel orderViewModel);
    }
}
