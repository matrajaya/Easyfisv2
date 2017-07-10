using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class TrnPurchaseOrder
    {
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public List<Entities.MstBranch> BranchList { get; set; }
        public String PONumber { get; set; }
        public String PODate { get; set; }
        public Int32 SupplierId { get; set; }
        public String Supplier { get; set; }
        public List<Entities.MstArticle> SupplierList { get; set; }
        public Int32 TermId { get; set; }
        public String Term { get; set; }
        public List<Entities.MstTerm> TermList { get; set; }
        public String ManualRequestNumber { get; set; }
        public String ManualPONumber { get; set; }
        public String DateNeeded { get; set; }
        public String Remarks { get; set; }
        public Boolean IsClose { get; set; }
        public Int32 RequestedById { get; set; }
        public String RequestedBy { get; set; }
        public List<Entities.MstUser> RequestedByUserList { get; set; }
        public Int32 PreparedById { get; set; }
        public String PreparedBy { get; set; }
        public List<Entities.MstUser> PreparedByUserList { get; set; }
        public Int32 CheckedById { get; set; }
        public String CheckedBy { get; set; }
        public List<Entities.MstUser> CheckedByUserList { get; set; }
        public Int32 ApprovedById { get; set; }
        public String ApprovedBy { get; set; }
        public List<Entities.MstUser> ApprovedByUserList { get; set; }
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