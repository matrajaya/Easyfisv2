using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class TrnStockInItem
    {
        public Int32 Id { get; set; }
        public Int32 INId { get; set; }
        public String INNumber { get; set; }
        public String INDate { get; set; }
        public List<Entities.TrnStockIn> INList { get; set; }
        public Int32 ItemId { get; set; }
        public String Item { get; set; }
        public List<Entities.MstArticle> ItemList { get; set; }
        public String Particulars { get; set; }
        public Int32 UnitId { get; set; }
        public String Unit { get; set; }
        public List<Entities.MstUnit> UnitList { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Cost { get; set; }
        public Decimal Amount { get; set; }
        public Int32 BaseUnitId { get; set; }
        public String BaseUnit { get; set; }
        public List<Entities.MstUnit> BaseUnitList { get; set; }
        public Decimal BaseQuantity { get; set; }
        public Decimal BaseCost { get; set; }
    }
}