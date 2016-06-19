using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class MstUser
    {
        [Key]
        public Int32 Id { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String FullName { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32? CreatedById { get; set; }
        public String CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32? UpdatedById { get; set; }
        public String UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }

        // user defaults
        public String UserId { get; set; }
        public Int32 CompanyId { get; set; }
        public String Company { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public Int32 IncomeAccountId { get; set; }
        public String IncomeAccount { get; set; }
        public Int32 SupplierAdvancesAccountId { get; set; }
        public String SupplierAdvancesAccount { get; set; }
        public Int32 CustomerAdvancesAccountId { get; set; }
        public String CustomerAdvancesAccount { get; set; }
    }
}