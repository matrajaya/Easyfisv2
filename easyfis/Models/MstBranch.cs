using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class MstBranch
    {
        [Key]
        public Int32 Id { get; set; }
        public Int32 CompanyId { get; set; }
        public String Company { get; set; }
        public String BranchCode { get; set; }
        public String Branch { get; set; }
        public String Address { get; set; }
        public String ContactNumber { get; set; }
        public String TaxNumber { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public String CreatedByIdUserName { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public String UpdatedByIdUserName { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}