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
        public String BranchCode { get; set; }
        public String InventoryDate { get; set; }
        public Int32 ArticleId { get; set; }
        public String Article { get; set; }
        public Int32 ArticleInventoryId { get; set; }
        public String ArticleInventoryCode { get; set; }
        public Int32? RRId { get; set; }
        public String RRNumber { get; set; }
        public Int32? SIId { get; set; }
        public String SINumber { get; set; }
        public Int32? INId { get; set; }
        public String INNumber { get; set; }
        public Int32? OTId { get; set; }
        public String OTNumber { get; set; }
        public Int32? STId { get; set; }
        public String STNumber { get; set; }
        public Decimal QuantityIn { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal QuantityOut { get; set; }
        public Decimal Amount { get; set; }
        public String Particulars { get; set; }
        public String Code { get; set; }
        public Int32 UnitId { get; set; }
        public String Unit { get; set; }
        public Decimal? Cost { get; set; }
        public Decimal PositiveQuantity { get; set; }
        public Decimal PositiveAmount { get; set; }
        public String Document { get; set; }
        public DateTime DateTimeInventoryDate { get; set; }

        public Decimal BegQuantity { get; set; }
        public Decimal InQuantity { get; set; }
        public Decimal OutQuantity { get; set; }
        public Decimal EndQuantity { get; set; }

    }
}