using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shirley.Book.Service.Contracts
{
    public class BookStockViewModel
    {
        [Required]
        public string OrderId { get; set; }

        [Required]
        public IReadOnlyList<StockViewModel> StockViewModels { get; set; }
    }

    public class StockViewModel
    {
        [Required]
        public string Sn { get; set; }
        public int Count { get; set; }
    }
}
