using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class TrnStockTransferItem
    {
        public Int32 Id { get; set; }
        public Int32 STId { get; set; }
        public String STNumber { get; set; }
        public String STDate { get; set; }
        public List<Entities.TrnStockTransfer> STList { get; set; }
        public Int32 ItemId { get; set; }
        public String Item { get; set; }
        public List<Entities.MstArticle> ItemList { get; set; }
        public Int32 ItemInventoryId { get; set; }
        public String ItemInventoryCode { get; set; }
        public List<Entities.MstArticleInventory> ItemInventoryList { get; set; }
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