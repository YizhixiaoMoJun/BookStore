using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Shirley.Book.Model;
using Shirley.Book.Service.Contracts;
using Shirley.Book.Service.Domains.Commands;
using Shirley.Book.Web.Infrastructure;
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
        private readonly IDistributedCache distributedCache;

        public BookController(IMediator mediator,IDistributedCache distributedCache)
        {
            this.mediator = mediator;
            this.distributedCache = distributedCache;
        }

        [HttpPost("order")]
        public async Task<BaseResponse> OrderBook(BookOrderViewModel model)
        {
            distributedCache.SetString("shiley","test");
            var str = distributedCache.GetString("shirley");
            var order = await mediator.Send(new BookOrderCommand { BookOrder = model });
            return ApiResponse.OK(order);
        }
    }
}
