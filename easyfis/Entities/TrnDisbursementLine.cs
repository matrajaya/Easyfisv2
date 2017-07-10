using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class TrnDisbursementLine
    {
        public Int32 Id { get; set; }
        public Int32 CVId { get; set; }
        public String CVNumber { get; set; }
        public String CVDate { get; set; }
        public List<Entities.TrnDisbursement> CVList { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public List<Entities.MstBranch> BranchList { get; set; }
        public Int32 AccountId { get; set; }
        public String Account { get; set; }
        public List<Entities.MstAccount> AccountList { get; set; }
        public Int32 ArticleId { get; set; }
        public String Article { get; set; }
        public List<Entities.MstArticle> ArticleList { get; set; }
        public Int32 RRId { get; set; }
        public String RRNumber { get; set; }
        public String RRDate { get; set; }
        public List<Entities.TrnReceivingReceipt> RRList { get; set; }
        public String Particulars { get; set; }
        public Decimal Amount { get; set; }
    }
}