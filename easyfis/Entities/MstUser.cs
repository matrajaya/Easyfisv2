using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class MstUser
    {
        public Int32 Id { get; set; }
        public String UserId { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String FullName { get; set; }
        public Int32 CompanyId { get; set; }
        public String Company { get; set; }
        public List<Entities.MstCompany> CompanyList { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public List<Entities.MstBranch> BranchList { get; set; }
        public Int32 IncomeAccountId { get; set; }
        public String IncomeAccount { get; set; }
        public List<Entities.MstAccount> IncomeAccountList { get; set; }
        public Int32 SupplierAdvancesAccountId { get; set; }
        public String SupplierAdvancesAccount { get; set; }
        public List<Entities.MstAccount> SupplierAdvancesAccountList { get; set; }
        public Int32 CustomerAdvancesAccountId { get; set; }
        public String CustomerAdvancesAccount { get; set; }
        public List<Entities.MstAccount> CustomerAdvancesAccountList { get; set; }
        public String OfficialReceiptName { get; set; }
        public String InventoryType { get; set; }
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