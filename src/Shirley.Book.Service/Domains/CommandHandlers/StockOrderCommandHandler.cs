using MediatR;
using Microsoft.Extensions.Logging;
using Shirley.Book.Service.Contracts;
using Shirley.Book.Service.Domains.Commands;
using Shirley.Book.Service.Domains.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shirley.Book.Service.Domains.CommandHandlers
{
    public class StockOrderCommandHandler : IRequestHandler<StockOrderCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IBookStockService bookStockService;
        private readonly ILogger<StockOrderCommandHandler> logger;

        public StockOrderCommandHandler(IMediator mediator, IBookStockService bookStockService, ILogger<StockOrderCommandHandler> logger)
        {
            this.mediator = mediator;
            this.bookStockService = bookStockService;
            this.logger = logger;
        }

        public async Task<bool> Handle(StockOrderCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("start stock for order {orderId} ...", request.BookStock.OrderId);
            await Task.Delay(500);
            var stock = await bookStockService.DecrementStocks(request.BookStock);
            if (!stock)
            {
                logger.LogInformation("order {orderId} stock failed!", request.BookStock.OrderId);
                throw new DomainException($"order {request.BookStock.OrderId} stock failed!");
            }

            await mediator.Publish(new BookStockedEvent { OrderId = request.BookStock.OrderId });
            return stock;
        }
    }
}
