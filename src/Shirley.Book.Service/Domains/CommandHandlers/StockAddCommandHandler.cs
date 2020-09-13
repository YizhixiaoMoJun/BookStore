using MediatR;
using Shirley.Book.Service.Contracts;
using Shirley.Book.Service.Domains.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shirley.Book.Service.Domains.CommandHandlers
{
    public class StockAddCommandHandler : IRequestHandler<StockAddCommand, bool>
    {
        private readonly IBookStockService bookStockService;

        public StockAddCommandHandler(IBookStockService bookStockService)
        {
            this.bookStockService = bookStockService;
        }

        public async Task<bool> Handle(StockAddCommand request, CancellationToken cancellationToken)
        {
            return await bookStockService.IncrementStocks(request.BookStock);
        }
    }
}
