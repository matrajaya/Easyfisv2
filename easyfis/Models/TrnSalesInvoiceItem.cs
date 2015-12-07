using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class TrnSalesInvoiceItem
    {
        [Key]

        public Int32 Id { get; set; }
        public Int32 SIId { get; set; }
        public String SI { get; set; }
        public Int32 ItemId { get; set; }
        public String Item { get; set; }
        public Int32 ItemInventoryId { get; set; }
        public String ItemInventory { get; set; }
        public String Particulars { get; set; }
        public Int32 UnitId { get; set; }
        public String Unit { get; set; }
        public Boolean Quantity { get; set; }
        public Boolean Price { get; set; }
        public Int32 DiscountId { get; set; }
        public String Discount { get; set; }
        public Boolean DiscountRate { get; set; }
        public Boolean DiscountAmount { get; set; }
        public Boolean NetPrice { get; set; }
        public Boolean Amount { get; set; }
        public Int32 VATId { get; set; }
        public String VAT { get; set; }
        public Boolean VATPercentage { get; set; }
        public Boolean VATAmount { get; set; }
        public Int32 BaseUnitId { get; set; }
        public String BaseUnit { get; set; }
        public Boolean BaseQuantity { get; set; }
        public Boolean BasePrice { get; set; }

    }
}