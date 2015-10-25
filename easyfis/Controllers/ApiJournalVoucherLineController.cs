using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

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
                                              JVId = d.JVId,
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
                                              ARSIId = d.ARSIId,
                                              IsClear = d.IsClear
                                          };
            return journalVoucherLines.ToList();
        }

        // ======================================
        // LIST Journal Voucher Line by branch Id
        // ======================================
        [Route("api/listJournalVoucherLineByBranchId/{JVId}")]
        public List<Models.TrnJournalVoucherLine> GetJournalVoucherLineById(String JVId)
        {
            var journalVoucherLineJVId = Convert.ToInt32(JVId);
            var journalVoucherLines = from d in db.TrnJournalVoucherLines
                                      where d.JVId == journalVoucherLineJVId
                                      select new Models.TrnJournalVoucherLine
                                          {
                                              Id = d.Id,
                                              JVId = d.JVId,
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
                                              ARSIId = d.ARSIId,
                                              IsClear = d.IsClear
                                          };
            return journalVoucherLines.ToList();
        }

        // ========================
        // ADD Journal Voucher Line
        // ========================
        [Route("api/addJournalVoucherLine")]
        public int Post(Models.TrnJournalVoucherLine journalVoucherLine)
        {
            try
            {
                Data.TrnJournalVoucherLine newJournalVoucherLine = new Data.TrnJournalVoucherLine();

                newJournalVoucherLine.JVId = journalVoucherLine.JVId;
                newJournalVoucherLine.BranchId = journalVoucherLine.BranchId;
                newJournalVoucherLine.AccountId = journalVoucherLine.AccountId;
                newJournalVoucherLine.ArticleId = journalVoucherLine.ArticleId;
                newJournalVoucherLine.Particulars = journalVoucherLine.Particulars;
                newJournalVoucherLine.DebitAmount = journalVoucherLine.DebitAmount;
                newJournalVoucherLine.CreditAmount = journalVoucherLine.CreditAmount;
                newJournalVoucherLine.APRRId = journalVoucherLine.APRRId;
                newJournalVoucherLine.ARSIId = journalVoucherLine.ARSIId;
                newJournalVoucherLine.IsClear = journalVoucherLine.IsClear;

                db.TrnJournalVoucherLines.InsertOnSubmit(newJournalVoucherLine);
                db.SubmitChanges();

                return newJournalVoucherLine.Id;
            }
            catch
            {
                return 0;
            }
        }

        // ===========================
        // UPDATE Journal Voucher Line
        // ===========================
        [Route("api/updateJournalVoucherLine/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnJournalVoucherLine journalVoucherLine)
        {
            try
            {
                var journalVoucherLineId = Convert.ToInt32(id);
                var journalVoucherLines = from d in db.TrnJournalVoucherLines where d.Id == journalVoucherLineId select d;

                if (journalVoucherLines.Any())
                {
                    var updatejournalVoucherLine = journalVoucherLines.FirstOrDefault();

                    updatejournalVoucherLine.JVId = journalVoucherLine.JVId;
                    updatejournalVoucherLine.BranchId = journalVoucherLine.BranchId;
                    updatejournalVoucherLine.AccountId = journalVoucherLine.AccountId;
                    updatejournalVoucherLine.ArticleId = journalVoucherLine.ArticleId;
                    updatejournalVoucherLine.Particulars = journalVoucherLine.Particulars;
                    updatejournalVoucherLine.DebitAmount = journalVoucherLine.DebitAmount;
                    updatejournalVoucherLine.CreditAmount = journalVoucherLine.CreditAmount;
                    updatejournalVoucherLine.APRRId = journalVoucherLine.APRRId;
                    updatejournalVoucherLine.ARSIId = journalVoucherLine.ARSIId;
                    updatejournalVoucherLine.IsClear = journalVoucherLine.IsClear;

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

        // ===========================
        // DELETE Journal Voucher Line
        // ===========================
        [Route("api/deleteJournalVoucherLine/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var journalVoucherLineId = Convert.ToInt32(id);
                var journalVoucherLines = from d in db.TrnJournalVoucherLines where d.Id == journalVoucherLineId select d;

                if (journalVoucherLines.Any())
                {
                    db.TrnJournalVoucherLines.DeleteOnSubmit(journalVoucherLines.First());
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
