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

        // =====================
        // ADD Disbursement Line
        // =====================
        [Route("api/addDisbursementLine")]
        public int Post(Models.TrnDisbursementLine disbursementLine)
        {
            try
            {
                Data.TrnDisbursementLine newDisbursementLine = new Data.TrnDisbursementLine();

                newDisbursementLine.CVId = disbursementLine.CVId;
                newDisbursementLine.BranchId = disbursementLine.BranchId;
                newDisbursementLine.AccountId = disbursementLine.AccountId;
                newDisbursementLine.ArticleId = disbursementLine.ArticleId;
                newDisbursementLine.RRId = disbursementLine.RRId;
                newDisbursementLine.Particulars = disbursementLine.Particulars;
                newDisbursementLine.Amount = disbursementLine.Amount;

                db.TrnDisbursementLines.InsertOnSubmit(newDisbursementLine);
                db.SubmitChanges();

                return newDisbursementLine.Id;
            }
            catch
            {
                return 0;
            }
        }

        // ========================
        // UPDATE Disbursement Line
        // ========================
        [Route("api/updateDisbursementLine/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnDisbursementLine disbursementLine)
        {
            try
            {
                var disbursementLineId = Convert.ToInt32(id);
                var disbursementLines = from d in db.TrnDisbursementLines where d.Id == disbursementLineId select d;

                if (disbursementLines.Any())
                {
                    var updateDisbursementLine = disbursementLines.FirstOrDefault();

                    updateDisbursementLine.CVId = disbursementLine.CVId;
                    updateDisbursementLine.BranchId = disbursementLine.BranchId;
                    updateDisbursementLine.AccountId = disbursementLine.AccountId;
                    updateDisbursementLine.ArticleId = disbursementLine.ArticleId;
                    updateDisbursementLine.RRId = disbursementLine.RRId;
                    updateDisbursementLine.Particulars = disbursementLine.Particulars;
                    updateDisbursementLine.Amount = disbursementLine.Amount;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // ========================
        // DELETE Disbursement Line
        // ========================
        [Route("api/deleteDisbursementLine/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var disbursementLineId = Convert.ToInt32(id);
                var disbursementLines = from d in db.TrnDisbursementLines where d.Id == disbursementLineId select d;

                if (disbursementLines.Any())
                {
                    db.TrnDisbursementLines.DeleteOnSubmit(disbursementLines.First());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
