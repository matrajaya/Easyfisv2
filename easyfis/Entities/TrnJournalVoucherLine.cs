using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class TrnJournalVoucherLine
    {
        public Int32 Id { get; set; }
        public Int32 JVId { get; set; }
        public Int32 BranchId { get; set; }
        public Int32 AccountId { get; set; }
        public Int32 ArticleId { get; set; }
        public String Particulars { get; set; }
        public Decimal DebitAmount { get; set; }
        public Decimal CreditAmount { get; set; }
        public Int32 APRRId { get; set; }
        public Int32 ARSIId { get; set; }
        public Boolean IsClear { get; set; }
    }
}