using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class TrnSalesInvoiceItem
    {
        public Int32 Id { get; set; }
        public Int32 SIId { get; set; }
        public String SINumber { get; set; }
        public String SIDate { get; set; }
        public List<Entities.TrnSalesInvoice> SIList { get; set; }
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
        public Decimal Price { get; set; }
        public Int32 DiscountId { get; set; }
        public String Discount { get; set; }
        public List<Entities.MstDiscount> DiscountList { get; set; }
        public Decimal DiscountRate { get; set; }
        public Decimal DiscountAmount { get; set; }
        public Decimal NetPrice { get; set; }
        public Decimal Amount { get; set; }
        public Int32 VATId { get; set; }
        public String VAT { get; set; }
        public List<Entities.MstTaxType> VATList { get; set; }
        public Decimal VATPercentage { get; set; }
        public Decimal VATAmount { get; set; }
        public Int32 BaseUnitId { get; set; }
        public String BaseUnit { get; set; }
        public List<Entities.MstUnit> BaseUnitList { get; set; }
        public Decimal BaseQuantity { get; set; }
        public Decimal BaseCost { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public String CreatedBy { get; set; }
        public List<Entities.MstUser> CreatedByUserList { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public String UpdatedBy { get; set; }
        public List<Entities.MstUser> UpdatedByUserList { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}