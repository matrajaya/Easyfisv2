using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class MstUserBranch
    {
        public Int32 Id { get; set; }
        public Int32 UserId { get; set; }
        public String User { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public Int32 CompanyId { get; set; }
        public String Company { get; set; }
    }
}