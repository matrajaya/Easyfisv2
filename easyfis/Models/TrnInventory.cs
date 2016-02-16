using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class TrnInventory
    {
         [Key]
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public String InventoryDate { get; set; }
        public Int32 ArticleId { get; set; }
        public String Article { get; set; }
        public Int32 UnitId { get; set; }
        public String Unit { get; set; }
        public Decimal Cost { get; set; }
        public String ManualArticleCode { get; set; }
        public Int32 ArticleInventoryId { get; set; }
        public Int32? RRId { get; set; }
        public Int32? SIId { get; set; }
        public Int32? INId { get; set; }
        public Int32? OTId { get; set; }
        public Int32? STId { get; set; }
        public Decimal QuantityIn { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal QuantityOut { get; set; }
        public Decimal Amount { get; set; }
        public String Particulars { get; set; }
    }
}