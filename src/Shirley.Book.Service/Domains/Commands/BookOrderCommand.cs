using MediatR;
using Shirley.Book.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shirley.Book.Service.Domains.Commands
{
    public class BookOrderCommand : IRequest<string>
    {
        public BookOrderViewModel BookOrder { get; set; }
    }
}
