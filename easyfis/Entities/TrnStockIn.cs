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
        public String INNumber { get; set; }
        public String INDate { get; set; }
        public Int32 AccountId { get; set; }
        public Int32 ArticleId { get; set; }
        public String Particulars { get; set; }
        public String ManualINNumber { get; set; }
        public Boolean IsProduced { get; set; }
        public Int32 PreparedById { get; set; }
        public Int32 CheckedById { get; set; }
        public Int32 ApprovedById { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}