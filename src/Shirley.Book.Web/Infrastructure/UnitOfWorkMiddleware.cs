using Microsoft.AspNetCore.Http;
using Shirley.Book.Service.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shirley.Book.Web.Infrastructure
{
    public class UnitOfWorkMiddleware : IMiddleware
    {
        private readonly IUnitOfWorkManager unitOfWorkManager;

        public UnitOfWorkMiddleware(IUnitOfWorkManager unitOfWorkManager)
        {
            this.unitOfWorkManager = unitOfWorkManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            using var uow = await unitOfWorkManager.Reserve();
            await next.Invoke(context);
            await uow.CommitAsync();
        }
    }
}
