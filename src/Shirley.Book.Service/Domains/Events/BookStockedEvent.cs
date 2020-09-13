using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shirley.Book.Service.Domains.Events
{
    public class BookStockedEvent : INotification
    {
        public string OrderId { get; set; }
    }
}
