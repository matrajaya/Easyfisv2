using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class MstCompany
    {
        [Key]
        public Int32 Id { get; set; }
        public String Company { get; set; }
        public String Address { get; set; }
        public String ContactNumber { get; set; }
        public String TaxNumber { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public String CreateDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}