using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class TrnStockTransferItem
    {
        public Int32 Id { get; set; }
        public Int32 STId { get; set; }
        public String ST { get; set; }
        public Int32 ItemId { get; set; }
        public String Item { get; set; }
        public Int32 ItemInventoryId { get; set; }
        public String ItemInventory { get; set; }
        public String Particulars { get; set; }
        public Int32 UnitId { get; set; }
        public String Unit { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Cost { get; set; }
        public Decimal Amount { get; set; }
        public Int32 BaseUnitId { get; set; }
        public String BaseUnit { get; set; }
        public Decimal BaseQuantity { get; set; }
        public Decimal BaseCost { get; set; }

    }
}