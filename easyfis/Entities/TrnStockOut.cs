using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class TrnStockOut
    {
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public String OTNumber { get; set; }
        public DateTime OTDate { get; set; }
        public Int32 AccountId { get; set; }
        public Int32 ArticleId { get; set; }
        public String Particulars { get; set; }
        public String ManualOTNumber { get; set; }
        public Int32 PreparedById { get; set; }
        public Int32 CheckedById { get; set; }
        public Int32 ApprovedById { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}