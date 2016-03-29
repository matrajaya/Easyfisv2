using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class MstTaxType
    {
        [Key]
        public Int32 Id { get; set; }
        public String TaxType { get; set; }
        public Decimal TaxRate { get; set; }
        public Boolean IsInclusive { get; set; }
        public Int32 AccountId { get; set; }
        public String Account { get; set; }
        public String AccountCode { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public String CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public String UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}