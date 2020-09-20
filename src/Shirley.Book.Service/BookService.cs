using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Shirley.Book.DataAccess;
using Shirley.Book.Model;
using Shirley.Book.Service.Contracts;
using Shirley.Book.Service.Domains;
using Shirley.Book.Service.Domains.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shirley.Book.Service
{
    public class BookService : IBookOrderService, IBookStockService
    {
        private readonly IMediator mediator;
        private readonly BookContext bookContext;
        private readonly ILogger<BookService> logger;
        private readonly IDistributedLockProvder distributedLockProvider;

        public BookService(IMediator mediator, BookContext bookContext, ILogger<BookService> logger, IDistributedLockProvder distributedLockProvider)
        {
            this.mediator = mediator;
            this.bookContext = bookContext;
            this.logger = logger;
            this.distributedLockProvider = distributedLockProvider;
        }


        public async Task<bool> DecrementStocks(BookStockViewModel bookStockViewModel)
        {
            var books = bookStockViewModel.StockViewModels.GroupBy(s => s.Sn)
                .Select(g => new { Sn = g.Key, Count = g.Sum(s => s.Count) })
                .ToList();

            var sns = books.Select(b => b.Sn).ToList();
            //var lockKey = string.Join(",", sns);

            // using var distributedLock = await distributedLockProvider.Acquire(lockKey);
            var stocks = await bookContext.BookStocks
                .Where(s => sns.Contains(s.Sn))
                .ToListAsync();

            await Task.Delay(500);
            var errors = new List<string>();
            foreach (var book in books)
            {
                var stock = stocks.Find(s => s.Sn == book.Sn);
                if (stock == null)
                {
                    errors.Add($"book {book.Sn} doesn't exist in stocks");
                    continue;
                }

                if (stock.StockCount < book.Count)
                {
                    errors.Add($"book {book.Sn} doesn't have enough stocks");
                    continue;
                }

                stock.StockCount -= book.Count;
                stock.FreezeStock -= book.Count;

                bookContext.OrderStocks
                    .Add(new OrderStock
                    {
                        OrderId = bookStockViewModel.OrderId,
                        Sn = book.Sn,
                        Count = book.Count
                    });
            }

            if (errors.Any())
            {
                throw new DomainException(string.Join(",", errors));
            }

            await bookContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IncrementStocks(BookStockViewModel bookStockViewModel)
        {
            var books = bookStockViewModel.StockViewModels.GroupBy(s => s.Sn)
                .Select(g => new { Sn = g.Key, Count = g.Sum(s => s.Count) })
                .ToList();

            var sns = books.Select(b => b.Sn).ToList();

            var stocks = await bookContext.BookStocks
                .Where(s => sns.Contains(s.Sn))
                .ToListAsync();

            foreach (var book in books)
            {
                var stock = stocks.Find(s => s.Sn == book.Sn);
                if (stock == null)
                {
                    stock = new BookStock
                    {
                        Sn = book.Sn,
                        StockCount = 0
                    };

                    bookContext.BookStocks.Add(stock);
                }

                if (stock.StockCount < 0 || stock.FreezeStock > stock.StockCount)
                {
                    logger.LogError("detected data {Id} run into concurrent conflict state! freeze {freezeCount}, stock {stockCount}", stock.Id, stock.FreezeStock, stock.StockCount);
                    continue;
                }
                //test
                //stock.StockCount += book.Count;

                stock.StockCount = 30;
                stock.FreezeStock = 0;
            }

            await bookContext.SaveChangesAsync();

            return true;
        }

        public async Task<string> OrderBook(BookOrderViewModel orderViewModel)
        {
            // here we just generate order id in code. 
            // we can also use navigation property and let ef generate id for us, but it would be another topic.
            var bookOrder = new BookOrder
            {
                Id = Guid.NewGuid().ToString(),
                OrderDetails = new List<BookOrderDetail>()
            };

            foreach (var detail in orderViewModel.OrderDetails)
            {
                bookOrder.OrderDetails
                    .Add(new BookOrderDetail
                    {
                        Sn = detail.Sn,
                        Count = detail.Count,
                        OrderId = bookOrder.Id
                    });
            }

            var sns = orderViewModel.OrderDetails.Select(o => o.Sn)
                .OrderBy(x => x)
                .ToList();

            var lockKey = string.Join(",", sns);

            using var distributedLock = await distributedLockProvider.Acquire(lockKey);

            var stocks = await bookContext.BookStocks
            .Where(s => sns.Contains(s.Sn))
            .ToListAsync();
            var errors = new List<string>();
            foreach (var book in orderViewModel.OrderDetails)
            {
                var stock = stocks.Find(s => s.Sn == book.Sn);
                if (stock == null)
                {
                    errors.Add($"book {book.Sn} doesn't exist in stock.");
                    continue;
                }

                if (stock.StockCount < book.Count + stock.FreezeStock)
                {
                    errors.Add($"book {book.Sn} doesn't have enough stock.");
                    continue;
                }

                stock.FreezeStock += book.Count;
            }

            if (errors.Any())
            {
                throw new DomainException(string.Join(",", errors));
            }

            bookContext.BookOrders.Add(bookOrder);

            await bookContext.SaveChangesAsync();

            await Task.Delay(500);

            await mediator.Send(new StockOrderCommand
            {
                BookStock = new BookStockViewModel
                {
                    OrderId = bookOrder.Id,
                    StockViewModels = orderViewModel.OrderDetails
                        .Select(o => new StockViewModel { Sn = o.Sn, Count = o.Count })
                        .ToList()
                }
            });

            return bookOrder.Id;
        }

    }
}
