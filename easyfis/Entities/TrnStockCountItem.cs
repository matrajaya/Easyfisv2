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
        public String SCNumber { get; set; }
        public String SCDate { get; set; }
        public List<Entities.TrnStockCount> SCList { get; set; }
        public Int32 ItemId { get; set; }
        public String Item { get; set; }
        public List<Entities.MstArticle> ItemList { get; set; }
        public String Particulars { get; set; }
        public Decimal Quantity { get; set; }
    }
}