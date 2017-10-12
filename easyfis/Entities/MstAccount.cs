using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class MstAccount
    {
        public Int32 Id { get; set; }
        public String AccountCode { get; set; }
        public String Account { get; set; }
        public Int32 AccountTypeId { get; set; }
        public String AccountType { get; set; }
        public Int32 AccountCashFlowId { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public String CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public String UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; } 
    }
}