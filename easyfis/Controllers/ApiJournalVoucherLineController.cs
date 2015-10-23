using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiJournalVoucherLineController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =========================
        // LIST Journal Voucher Line
        // =========================
        [Route("api/listJournalVoucherLine")]
        public List<Models.TrnJournalVoucherLine> Get()
        {
            var journalVoucherLines = from d in db.TrnJournalVoucherLines
                                      select new Models.TrnJournalVoucherLine
                                          {
                                              Id = d.Id,
                                              BranchId = d.BranchId,
                                              Branch = d.MstBranch.Branch,
                                              AccountId = d.AccountId,
                                              Account = d.MstAccount.Account,
                                              ArticleId = d.ArticleId,
                                              Article = d.MstArticle.Article,
                                              Particulars = d.Particulars,
                                              DebitAmount = d.DebitAmount,
                                              CreditAmount = d.CreditAmount,
                                              APRRId = d.APRRId,
                                              APSIId = d.ARSIId,
                                              IsClear = d.IsClear
                                          };
            return journalVoucherLines.ToList();
        }

        // ======================================
        // LIST Journal Voucher Line by branch Id
        // ======================================
        [Route("api/listJournalVoucherLineByBranchId/{branchId}")]
        public List<Models.TrnJournalVoucherLine> GetJournalVoucherLineById(String branchId)
        {
            var journalVoucherLineBranchId = Convert.ToInt32(branchId);
            var journalVoucherLines = from d in db.TrnJournalVoucherLines
                                      where d.BranchId == journalVoucherLineBranchId
                                      select new Models.TrnJournalVoucherLine
                                          {
                                              Id = d.Id,
                                              BranchId = d.BranchId,
                                              Branch = d.MstBranch.Branch,
                                              AccountId = d.AccountId,
                                              Account = d.MstAccount.Account,
                                              ArticleId = d.ArticleId,
                                              Article = d.MstArticle.Article,
                                              Particulars = d.Particulars,
                                              DebitAmount = d.DebitAmount,
                                              CreditAmount = d.CreditAmount,
                                              APRRId = d.APRRId,
                                              APSIId = d.ARSIId,
                                              IsClear = d.IsClear
                                          };
            return journalVoucherLines.ToList();
        }
    }
}
