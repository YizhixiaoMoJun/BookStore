using System;
using System.Collections.Generic;
using System.Text;

namespace Shirley.Book.Model
{
    public class BookOrder
    {
        public string Id { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public ICollection<BookOrderDetail> OrderDetails { get; set; }
    }
}
