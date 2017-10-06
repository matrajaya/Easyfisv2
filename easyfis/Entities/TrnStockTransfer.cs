using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class TrnStockTransfer
    {
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public String STNumber { get; set; }
        public String STDate { get; set; }
        public Int32 ToBranchId { get; set; }
        public Int32 ArticleId { get; set; }
        public String Particulars { get; set; }
        public String ManualSTNumber { get; set; }
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