using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class TrnJournal
    {
        public Int32 Id { get; set; }
        public String JournalDate { get; set; }
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
        public Int32 ORId { get; set; }
        public String ORNumber { get; set; }
        public String ORDate { get; set; }
        public List<Entities.TrnCollection> ORList { get; set; }
        public Int32 CVId { get; set; }
        public String CVNumber { get; set; }
        public String CVDate { get; set; }
        public List<Entities.TrnDisbursement> CVList { get; set; }
        public Int32 JVId { get; set; }
        public String JVNumber { get; set; }
        public String JVDate { get; set; }
        public List<Entities.TrnJournalVoucher> JVList { get; set; }
        public Int32 RRId { get; set; }
        public String RRNumber { get; set; }
        public String RRDate { get; set; }
        public List<Entities.TrnReceivingReceipt> RRList { get; set; }
        public Int32 SIId { get; set; }
        public String SINumber { get; set; }
        public String SIDate { get; set; }
        public List<Entities.TrnSalesInvoice> SIList { get; set; }
        public Int32 INId { get; set; }
        public String INNumber { get; set; }
        public String INDate { get; set; }
        public List<Entities.TrnStockIn> INList { get; set; }
        public Int32 OTId { get; set; }
        public String OTNumber { get; set; }
        public String OTDate { get; set; }
        public List<Entities.TrnStockOut> OTList { get; set; }
        public Int32 STId { get; set; }
        public String STNumber { get; set; }
        public String STDate { get; set; }
        public List<Entities.TrnStockTransfer> STList { get; set; }
        public String DocumentReference { get; set; }
        public Int32 APRRId { get; set; }
        public String APRRNumber { get; set; }
        public String APRRDate { get; set; }
        public List<Entities.TrnReceivingReceipt> APRRList { get; set; }
        public Int32 ARSIId { get; set; }
        public String ARSINumber { get; set; }
        public String ARSIDate { get; set; }
        public List<Entities.TrnSalesInvoice> ARSIList { get; set; }
    }
}