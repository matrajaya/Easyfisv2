using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class TrnStockCountItem
    {
        public Int32 Id { get; set; }
        public Int32 SCId { get; set; }
        public Int32 ItemId { get; set; }
        public String Particulars { get; set; }
        public Decimal Quantity { get; set; }
    }
}