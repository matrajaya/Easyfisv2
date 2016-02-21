using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class TrnReceivingReceipt
    {
        [Key]

        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public String BranchCode { get; set; }
        public String RRDate { get; set; }
        public String RRNumber { get; set; } 
        public Int32 SupplierId { get; set; }
        public String Supplier { get; set; }
        public Int32 TermId { get; set; }
        public String Term { get; set; }
        public String DocumentReference { get; set; }
        public String ManualRRNumber { get; set; }
        public String Remarks { get; set; }
        public Decimal Amount { get; set; }
        public Decimal WTaxAmount { get; set; }
        public Decimal PaidAmount { get; set; }
        public Decimal AdjustmentAmount { get; set; }
        public Decimal BalanceAmount { get; set; }
        public Int32 ReceivedById { get; set; }
        public String ReceivedBy { get; set; }
        public String PreparedBy { get; set; }
        public Int32 PreparedById { get; set; }
        public String CheckedBy { get; set; }
        public Int32 CheckedById { get; set; }
        public String ApprovedBy { get; set; }
        public Int32 ApprovedById { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public String CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public String UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }

        public Int32 AccountId { get; set; }
        public String AccountCode { get; set; }
        public String Account { get; set; }
    }
}