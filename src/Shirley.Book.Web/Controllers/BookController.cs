using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        public BookController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("order")]
        public async Task<BaseResponse> OrderBook(BookOrderViewModel model)
        {
            var order = await mediator.Send(new BookOrderCommand { BookOrder = model });
            return ApiResponse.OK(order);
        }
    }
}
