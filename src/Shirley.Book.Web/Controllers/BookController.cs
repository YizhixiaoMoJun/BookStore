using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Shirley.Book.DataAccess;
using Shirley.Book.Model;
using Shirley.Book.Service.Contracts;
using Shirley.Book.Service.Domains.Commands;
using Shirley.Book.Web.Infrastructure;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shirley.Book.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IConnectionMultiplexer redis;
        private readonly IDatabase redisdb;
        private readonly BookContext bookContext;

        public BookController(IMediator mediator, IConnectionMultiplexer redis, BookContext bookContext)
        {
            this.mediator = mediator;
            this.redis = redis;
            this.redisdb = redis.GetDatabase();
            this.bookContext = bookContext;
        }

        [HttpPost("order")]
        // [ServiceFilter(typeof(UnitOfWorkFilterAttribute))]
        [UnitOfWorkFilter]
        public async Task<BaseResponse> OrderBook(BookOrderViewModel model)
        {
            var order = await mediator.Send(new BookOrderCommand { BookOrder = model });
            return ApiResponse.OK(order);
        }

        [HttpGet("getStockCount")]
        public async Task<BaseResponse> GetStockCount()
        {
            var stockList = await bookContext.BookStocks.ToListAsync();
            return ApiResponse.OK(stockList);
        }
    }
}
