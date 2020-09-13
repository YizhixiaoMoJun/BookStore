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
    public class BookOrderCommandHandler : IRequestHandler<BookOrderCommand, string>
    {
        private readonly IMediator mediator;
        private readonly IBookOrderService bookOrderService;
        private readonly ILogger<BookOrderCommandHandler> logger;

        public BookOrderCommandHandler(IMediator mediator, IBookOrderService bookOrderService, ILogger<BookOrderCommandHandler> logger)
        {
            this.mediator = mediator;
            this.bookOrderService = bookOrderService;
            this.logger = logger;
        }

        public async Task<string> Handle(BookOrderCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("start book order ...");
            var orderId = await bookOrderService.OrderBook(request.BookOrder);
            await Task.Delay(1000);
            await mediator.Publish(new BookOrderedEvent { OrderId = orderId });
            return orderId;
        }
    }
}
