using System;
using System.Collections.Generic;
using System.Text;

namespace Shirley.Book.Model
{
    public class BookStock
    {
        public string Id { get; set; }
        public string Sn { get; set; }
        public int StockCount { get; set; }
        public int FreezeStock { get; set; }
        //public string ConcurrencyStamp { get; set; }
    }
}
