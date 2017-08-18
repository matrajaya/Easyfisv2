using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.POSIntegrationEntities
{
    public class POSIntegrationTrnSalesInvoice
    {
        public Int32 Id { get; set; }
        public String SIDate { get; set; }
        public String BranchCode { get; set; }
        public String CustomerManualArticleCode { get; set; }
        public String Term { get; set; }
        public String DocumentReference { get; set; }
        public String ManualSINumber { get; set; }
        public String Remarks { get; set; }
        public Decimal Amount { get; set; }
        public Decimal PaidAmount { get; set; }
        public Decimal AdjustmentAmount { get; set; }
        public String CreatedBy { get; set; }
        public List<POSIntegrationEntities.POSIntegrationTrnSalesInvoiceItem> listPOSIntegrationTrnSalesInvoiceItem { get; set; }
    }
}