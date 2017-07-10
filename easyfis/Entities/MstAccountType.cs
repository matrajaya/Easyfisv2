using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class MstAccountType
    {
        public Int32 Id { get; set; }
        public String AccountTypeCode { get; set; }
        public String AccountType { get; set; }
        public Int32 AccountCategoryId { get; set; }
        public String AccountCategory { get; set; }
        public List<Entities.MstAccountCategory> AccountCategoryList { get; set; }
        public String SubCategoryDescription { get; set; }
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