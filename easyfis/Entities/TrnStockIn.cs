using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class TrnStockIn
    {
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public List<Entities.MstBranch> BranchList { get; set; }
        public String INNumber { get; set; }
        public String INDate { get; set; }
        public Int32 AccountId { get; set; }
        public String Account { get; set; }
        public List<Entities.MstAccount> AccountList { get; set; }
        public Int32 ArticleId { get; set; }
        public String Article { get; set; }
        public List<Entities.MstArticle> ArticleList { get; set; }
        public String Particulars { get; set; }
        public String ManualINNumber { get; set; }
        public Boolean IsProduced { get; set; }
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