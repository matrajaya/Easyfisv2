using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class TrnSalesInvoice
    {
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public String SINumber { get; set; }
        public String SIDate { get; set; }
        public Int32 CustomerId { get; set; }
        public String Customer { get; set; }
        public Int32 TermId { get; set; }
        public String DocumentReference { get; set; }
        public String ManualSINumber { get; set; }
        public String Remarks { get; set; }
        public Decimal Amount { get; set; }
        public Decimal PaidAmount { get; set; }
        public Decimal AdjustmentAmount { get; set; }
        public Decimal BalanceAmount { get; set; }
        public Int32 SoldById { get; set; }
        public Int32 PreparedById { get; set; }
        public Int32 CheckedById { get; set; }
        public Int32 ApprovedById { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public String CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public String UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}