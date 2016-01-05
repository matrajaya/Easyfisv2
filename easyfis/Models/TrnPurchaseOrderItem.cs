using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class TrnPurchaseOrderItem
    {
        [Key]

        public Int32 Id { get; set; }
        public Int32 POId { get; set; }
        public String PO { get; set; } 
        public Int32 ItemId { get; set; }
        public String ItemCode { get; set; }
        public String Item { get; set; }
        public String Particulars { get; set; }
        public Int32 UnitId { get; set; }
        public String Unit { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Cost { get; set; }
        public Decimal Amount { get; set; }
    
    }
}