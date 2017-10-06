using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class MstPayType
    {
        public Int32 Id { get; set; }
        public String PayType { get; set; }
        public Int32 AccountId { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}