using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class MstBranch
    {
        public Int32 Id { get; set; }
        public Int32 CompanyId { get; set; }
        public String Company { get; set; }
        public List<Entities.MstCompany> CompanyList { get; set; }
        public String BranchCode { get; set; }
        public String Branch { get; set; }
        public String Address { get; set; }
        public String ContactNumber { get; set; }
        public String TaxNumber { get; set; }
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