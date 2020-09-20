using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Shirley.Book.Service.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shirley.Book.Web.Infrastructure
{
    public class UnitOfWorkFilterAttribute : ActionFilterAttribute
    {
        //private readonly IUnitOfWorkManager unitOfWorkManager;

        //public UnitOfWorkFilterAttribute(IUnitOfWorkManager unitOfWorkManager)
        //{
        //    this.unitOfWorkManager = unitOfWorkManager;
        //}

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var uowInstance = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWorkManager>();
            using var uow = await uowInstance.Reserve();
            //using var uow = await unitOfWorkManager.Reserve();
            await next.Invoke();
            await uow.CommitAsync();
        }
    }
}
