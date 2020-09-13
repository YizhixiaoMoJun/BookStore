using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shirley.Book.Service.Contracts
{
    public class BookOrderViewModel
    {
        [Required]
        public IReadOnlyList<OrderDetialViewModel> OrderDetails { get; set; }
    }

    public class OrderDetialViewModel
    {
        [Required]
        public string Sn { get; set; }
        public int Count { get; set; }
    }
}
