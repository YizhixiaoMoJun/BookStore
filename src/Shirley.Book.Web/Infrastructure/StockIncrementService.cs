using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shirley.Book.Service.Contracts;
using Shirley.Book.Service.Domains.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shirley.Book.Web.Infrastructure
{
    public class StockIncrementService : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;

        public StockIncrementService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        private readonly List<string> sns = new List<string>
        {
            "c2e85634-3988-4c00-9523-16297336c208",
            "9b8b1c26-f051-4949-a1da-2a80110df39c",
            "b586b6d9-9be6-4e9d-a614-87a6fbb3141e",
            "44b48f3a-b3f9-42c1-a588-9603735e863b",
            "f82cd999-10a1-4105-814c-1f44a7fc0c7a",
            "9820980b-ccd0-4952-9db4-896cf320c244",
            "ac31dd4b-eaba-4b59-b779-f7055253f90d",
            "7f0bdfcb-8cf9-4990-8ac4-221abbb49169",
            "4dd1eb86-33fc-40c5-ab7b-3a803674b685",
            "149159cb-e5e4-41f2-8c9b-3243daada9a3",
        };

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(5000);

            using var scope = serviceProvider.CreateScope();
            var sp = scope.ServiceProvider;
            var logger = sp.GetRequiredService<ILogger<StockIncrementService>>();

            var mediator = sp.GetRequiredService<IMediator>();

            try
            {
                await mediator.Send(new StockAddCommand
                {
                    BookStock = new BookStockViewModel
                    {
                        StockViewModels = sns.
                             Select(x => new StockViewModel() { Count = 100, Sn = x })
                            .ToList()
                    }
                });

            }
            catch (Exception e)
            {
                logger.LogWarning(e, "error occured while add stock ");
            }

        }
    }
}
