using System;
using System.Collections.Generic;
using System.Text;

namespace Shirley.Book.Model
{
    public class BookOrderDetail
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string Sn { get; set; }
        public int Count { get; set; }
    }
}
