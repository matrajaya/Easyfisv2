using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class MstArticleInventory
    {

        [Key]
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public Int32 ArticleId { get; set; }
        public String Article { get; set; }
        public String InventoryCode { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Cost { get; set; }
        public Decimal Amount { get; set; }
        public String Particulars { get; set; }

        public String ManualArticleCode { get; set; }
        public Int32 UnitId { get; set; }
        public String Unit { get; set; }

        public Decimal BegQuantity { get; set; }
        public Decimal InQuantity { get; set; }
        public Decimal OutQuantity { get; set; }
        public Decimal EndQuantity { get; set; }

        public String Document { get; set; }

        public String DateAquired { get; set; }
        public Decimal NoOfYears { get; set; }
        public Decimal SalvageValue { get; set; }
        public Decimal AccumulatedDepreciation { get; set; }
        public Decimal AdjustedTotalAmount { get; set; }

        public Boolean Inventory { get; set; }
        public Decimal Price { get; set; }
        
    }
}