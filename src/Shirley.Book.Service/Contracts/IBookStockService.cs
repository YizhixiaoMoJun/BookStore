﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shirley.Book.Service.Contracts
{
    public interface IBookStockService
    {
        Task<bool> DecrementStocks(BookStockViewModel bookStockViewModel);
        Task<bool> IncrementStocks(BookStockViewModel bookStockViewModel);
    }
}
