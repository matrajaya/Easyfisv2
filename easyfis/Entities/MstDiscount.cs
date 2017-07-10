using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class MstDiscount
    {
        public Int32 Id { get; set; }
        public String Discount { get; set; }
        public Decimal DiscountRate { get; set; }
        public Boolean IsInclusive { get; set; }
        public Int32 AccountId { get; set; }
        public String Account { get; set; }
        public List<Entities.MstAccount> AccountList { get; set; }
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