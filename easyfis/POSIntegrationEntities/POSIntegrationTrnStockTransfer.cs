using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.POSIntegrationEntities
{
    public class POSIntegrationTrnStockTransfer
    {
        public String BranchCode { get; set; }
        public String Branch { get; set; }
        public String STNumber { get; set; }
        public String STDate { get; set; }
        public String ToBranch { get; set; }
        public String ToBranchCode { get; set; }
        public String Article { get; set; }
        public String Particulars { get; set; }
        public String ManualSTNumber { get; set; }
        public String PreparedBy { get; set; }
        public String CheckedBy { get; set; }
        public String ApprovedBy { get; set; }
        public Boolean IsLocked { get; set; }
        public String CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public String UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }
        public List<POSIntegrationTrnStockTransferItem> listPOSIntegrationTrnStockTransferItem { get; set; }
    }

    public class POSIntegrationTrnStockTransferItem
    {
        public Int32 STId { get; set; }
        public String ItemCode { get; set; }
        public String Item { get; set; }
        public String InventoryCode { get; set; }
        public String Particulars { get; set; }
        public String Unit { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Cost { get; set; }
        public Decimal Amount { get; set; }
        public String BaseUnit { get; set; }
        public Decimal BaseQuantity { get; set; }
        public Decimal BaseCost { get; set; }
    }
}