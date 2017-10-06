using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class MstTaxType
    {
        public Int32 Id { get; set; }
        public String TaxType { get; set; }
        public Decimal TaxRate { get; set; }
        public Boolean IsInclusive { get; set; }
        public Int32 AccountId { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}