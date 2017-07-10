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
        public String JVNumber { get; set; }
        public String JVDate { get; set; }
        public List<Entities.TrnJournalVoucher> JVList { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public List<Entities.MstBranch> BranchList { get; set; }
        public Int32 AccountId { get; set; }
        public String Account { get; set; }
        public List<Entities.MstAccount> AccountList { get; set; }
        public Int32 ArticleId { get; set; }
        public String Article { get; set; }
        public List<Entities.MstArticle> ArticleList { get; set; }
        public String Particulars { get; set; }
        public Decimal DebitAmount { get; set; }
        public Decimal CreditAmount { get; set; }
        public Int32 APRRId { get; set; }
        public String APRRNumber { get; set; }
        public String APRRDate { get; set; }
        public List<Entities.TrnReceivingReceipt> APRRList { get; set; }
        public Int32 ARSIId { get; set; }
        public String ARSINumber { get; set; }
        public String ARSIDate { get; set; }
        public List<Entities.TrnSalesInvoice> ARSIList { get; set; }
        public Boolean IsClear { get; set; }
    }
}