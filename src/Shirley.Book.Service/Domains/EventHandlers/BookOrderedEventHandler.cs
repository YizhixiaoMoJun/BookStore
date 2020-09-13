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
    public class BookOrderedEventHandler : INotificationHandler<BookOrderedEvent>
    {
        private readonly ILogger<BookOrderedEventHandler> logger;

        public BookOrderedEventHandler(ILogger<BookOrderedEventHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(BookOrderedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("order {orderId} booked successful!", notification.OrderId);
            return Task.CompletedTask;
        }
    }
}
