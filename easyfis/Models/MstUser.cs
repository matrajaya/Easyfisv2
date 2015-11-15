using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class MstUser
    {
        [Key]
        public Int32 Id { get; set; }
        public String UserId { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String FullName { get; set; }
        public Boolean IsLocked { get; set; }
    }
}