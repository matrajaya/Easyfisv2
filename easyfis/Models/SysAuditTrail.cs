using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class SysAuditTrail
    {
        [Key]
        public Int32 Id { get; set; }
        public Int32 Userid { get; set; }
        public String User { get; set; }
        public String Audidate { get; set; }
        public String TableInformation { get; set; }
        public String RecordInformation { get; set; }
        public String FormInformation { get; set; }
        public String ActionInformation { get; set; }

    }
}