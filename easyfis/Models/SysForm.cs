using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class SysForm
    {
        [Key]

        public Int32 Id { get; set; }
        public String FormName { get; set; }
        public String Particulars { get; set; }

    }
}