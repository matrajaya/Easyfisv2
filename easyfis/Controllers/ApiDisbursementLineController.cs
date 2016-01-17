using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiDisbursementLineController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ======================
        // LIST Disbursement Line
        // ======================
        [Route("api/listDisbursementLine")]
        public List<Models.TrnDisbursementLine> Get()
        {
            var disbursementLines = from d in db.TrnDisbursementLines
                                    select new Models.TrnDisbursementLine
                                    {
                                        Id = d.Id,
                                        CVId = d.CVId,
                                        CV = d.TrnDisbursement.CVNumber,
                                        BranchId = d.BranchId,
                                        Branch = d.MstBranch.Branch,
                                        AccountId = d.AccountId,
                                        Account = d.MstAccount.Account,
                                        ArticleId = d.ArticleId,
                                        Article = d.MstArticle.Article,
                                        RRId = d.RRId,
                                        RR = d.TrnReceivingReceipt.RRNumber,
                                        Particulars = d.Particulars,
                                        Amount = d.Amount
                                    };
            return disbursementLines.ToList();
        }

        // ======================
        // LIST Disbursement Line
        // ======================
        [Route("api/listDisbursementLineByCVId/{CVId}")]
        public List<Models.TrnDisbursementLine> GetDisbursementByCVId(String CVId)
        {
            var disbursementLine_CVId = Convert.ToInt32(CVId);
            var disbursementLines = from d in db.TrnDisbursementLines
                                    where d.CVId == disbursementLine_CVId
                                    select new Models.TrnDisbursementLine
                                    {
                                        Id = d.Id,
                                        CVId = d.CVId,
                                        CV = d.TrnDisbursement.CVNumber,
                                        BranchId = d.BranchId,
                                        Branch = d.MstBranch.Branch,
                                        AccountId = d.AccountId,
                                        Account = d.MstAccount.Account,
                                        ArticleId = d.ArticleId,
                                        Article = d.MstArticle.Article,
                                        RRId = d.RRId,
                                        RR = d.TrnReceivingReceipt.RRNumber,
                                        Particulars = d.Particulars,
                                        Amount = d.Amount
                                    };
            return disbursementLines.ToList();
        }
    }
}
