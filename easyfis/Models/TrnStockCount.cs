using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class TrnStockCount
    {
        [Key]
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public String SCNumber { get; set; }
        public String SCDate { get; set; }
        public String Particulars { get; set; }
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
    }
}