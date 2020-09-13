﻿using MediatR;
using Shirley.Book.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shirley.Book.Service.Domains.Commands
{
    public class StockOrderCommand : IRequest<bool>
    {
        public BookStockViewModel BookStock { get; set; }
    }
}
