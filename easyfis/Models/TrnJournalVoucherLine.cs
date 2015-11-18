using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class TrnJournalVoucherLine
    {
        [Key]
        public Int32 Id { get; set; }
        public Int32 JVId { get; set; }
        public String JournalVoucher { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public Int32 AccountId { get; set; }
        public String Account { get; set; }
        public Int32 ArticleId { get; set; }
        public String Article { get; set; }
        public String Particulars { get; set; }
        public Decimal DebitAmount { get; set; }
        public Decimal CreditAmount { get; set; }
        public Int32 APRRId { get; set; }
        public Int32 ARSIId { get; set; }
        public Boolean IsClear { get; set; }
    }
}