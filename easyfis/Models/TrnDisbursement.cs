using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class TrnDisbursement
    {
        [Key]
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public String CVNumber { get; set; }
        public String CVDate { get; set; }
        public Int32 SupplierId { get; set; }
        public String Supplier { get; set; }
        public String Payee { get; set; }
        public Int32 PayTypeId { get; set; }
        public String PayType { get; set; }
        public Int32 BankId { get; set; }
        public String Bank { get; set; }
        public String ManualCVNumber { get; set; }
        public String Particulars { get; set; }
        public String CheckNumber { get; set; }
        public String CheckDate { get; set; }
        public Decimal Amount { get; set; }
        public Boolean IsCrossCheck { get; set; }
        public Boolean IsClear { get; set; }
        public Int32 PreparedById { get; set; }
        public String PreparedBy { get; set; }
        public Int32 CheckedById { get; set; }
        public String CheckedBy { get; set; }
        public Int32 ApprovedById { get; set; }
        public String ApprovedBy { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public String CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public String UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}