using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Models
{
    public class TrnJournalVoucher : ApiController
    {

        [Key]
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public String JVNumber { get; set; }
        public Boolean JVDate { get; set; }
        public String Particulars { get; set; }
        public String ManualJVNumber { get; set; }
        public Int32 PreparedById { get; set; }
        public Int32 CheckedById { get; set; }
        public Int32 ApprovedById { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedById { get; set; }
        public Boolean CreatedDateTime { get; set; }
        public Int32 UpdatedById { get; set; }
        public Boolean UpdatedDateTime { get; set; }

       
    }
}
