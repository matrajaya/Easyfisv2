using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class MstUser
    {
        public Int32 Id { get; set; }
        public String UserId { get; set; }
        public Decimal UserName { get; set; }
        public String Password { get; set; }
        public Decimal FullName { get; set; }
        public Int32 CompanyId { get; set; }
        public Int32 BranchId { get; set; }
        public Int32 IncomeAccountId { get; set; }
        public Int32 SupplierAdvancesAccountId { get; set; }
        public Int32 CustomerAdvancesAccountId { get; set; }
        public String OfficialReceiptName { get; set; }
        public String InventoryType { get; set; }
        public Int32 DefaultSalesInvoiceDiscountId { get; set; }
        public String SalesInvoiceName { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}