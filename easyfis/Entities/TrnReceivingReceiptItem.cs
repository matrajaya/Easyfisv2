using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class TrnReceivingReceiptItem
    {
        public Int32 Id { get; set; }
        public Int32 RRId { get; set; }
        public String RRNumber { get; set; }
        public String RRDate { get; set; }
        public List<Entities.TrnReceivingReceipt> RRList { get; set; }
        public Int32 POId { get; set; }
        public String PONumber { get; set; }
        public String PODate { get; set; }
        public List<Entities.TrnPurchaseOrder> POList { get; set; }
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
        public Int32 VATId { get; set; }
        public String VAT { get; set; }
        public List<Entities.MstTaxType> VATList { get; set; }
        public Decimal VATPercentage { get; set; }
        public Decimal VATAmount { get; set; }
        public Int32 WTAXId { get; set; }
        public String WTAX { get; set; }
        public List<Entities.MstTaxType> WTAXList { get; set; }
        public Decimal WTAXPercentage { get; set; }
        public Decimal WTAXAmount { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public List<Entities.MstBranch> BranchList { get; set; }
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