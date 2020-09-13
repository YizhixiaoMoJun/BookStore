using MediatR;
using Microsoft.Extensions.Logging;
using Shirley.Book.Service.Domains.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shirley.Book.Service.Domains.EventHandlers
{
    public class BookStockedEventHandler : INotificationHandler<BookStockedEvent>
    {
        private readonly ILogger<BookStockedEventHandler> logger;

        public BookStockedEventHandler(ILogger<BookStockedEventHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(BookStockedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("order {orderId} stocked successful!", notification.OrderId);
            return Task.CompletedTask;
        }
    }
}
