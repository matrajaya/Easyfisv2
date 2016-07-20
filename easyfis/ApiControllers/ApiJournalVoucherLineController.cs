using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace easyfis.Controllers
{
    public class ApiJournalVoucherLineController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // list journal voucher line
        [Authorize]
        [HttpGet]
        [Route("api/listJournalVoucherLine")]
        public List<Models.TrnJournalVoucherLine> listJournalVoucherLine()
        {
            var journalVoucherLines = from d in db.TrnJournalVoucherLines
                                      select new Models.TrnJournalVoucherLine
                                        {
                                            Id = d.Id,
                                            JVId = d.JVId,
                                            JVNumber = d.TrnJournalVoucher.JVNumber,
                                            JVDate = d.TrnJournalVoucher.JVDate.ToShortDateString(),
                                            JVParticulars = d.TrnJournalVoucher.Particulars,
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
                                            APRR = d.TrnReceivingReceipt.RRNumber,
                                            APRRBranch = d.TrnReceivingReceipt.MstBranch.Branch,
                                            ARSIId = d.ARSIId,
                                            ARSI = d.TrnSalesInvoice.SINumber,
                                            ARSIBranch = d.TrnSalesInvoice.MstBranch.Branch,
                                            IsClear = d.IsClear
                                        };

            return journalVoucherLines.ToList();
        }

        // list journal voucher by JV Date start and end for bank reconciliation
        [Authorize]
        [HttpGet]
        [Route("api/listJournalVoucherLineByJVDate/{ArticleId}/{DateStart}/{DateEnd}")]
        public List<Models.TrnJournalVoucherLine> listtJournalVoucherLineByJVDate(String ArticleId, String DateStart, String DateEnd)
        {
            var journalVoucherLines = from d in db.TrnJournalVoucherLines
                                      where d.ArticleId == Convert.ToInt32(ArticleId)
                                      && d.TrnJournalVoucher.JVDate >= Convert.ToDateTime(DateStart)
                                      && d.TrnJournalVoucher.JVDate <= Convert.ToDateTime(DateEnd)
                                      select new Models.TrnJournalVoucherLine
                                      {
                                          Id = d.Id,
                                          JVId = d.JVId,
                                          JVNumber = d.TrnJournalVoucher.JVNumber,
                                          JVDate = d.TrnJournalVoucher.JVDate.ToShortDateString(),
                                          JVParticulars = d.TrnJournalVoucher.Particulars,
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
                                          APRR = d.TrnReceivingReceipt.RRNumber,
                                          APRRBranch = d.TrnReceivingReceipt.MstBranch.Branch,
                                          ARSIId = d.ARSIId,
                                          ARSI = d.TrnSalesInvoice.SINumber,
                                          ARSIBranch = d.TrnSalesInvoice.MstBranch.Branch,
                                          IsClear = d.IsClear
                                      };

            var journalVoucherLinesIsNotClear = from d in db.TrnJournalVoucherLines
                                                where d.ArticleId == Convert.ToInt32(ArticleId)
                                                && d.TrnJournalVoucher.JVDate < Convert.ToDateTime(DateStart)
                                                && d.IsClear == false
                                                select new Models.TrnJournalVoucherLine
                                                {
                                                    Id = d.Id,
                                                    JVId = d.JVId,
                                                    JVNumber = d.TrnJournalVoucher.JVNumber,
                                                    JVDate = d.TrnJournalVoucher.JVDate.ToShortDateString(),
                                                    JVParticulars = d.TrnJournalVoucher.Particulars,
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
                                                    APRR = d.TrnReceivingReceipt.RRNumber,
                                                    APRRBranch = d.TrnReceivingReceipt.MstBranch.Branch,
                                                    ARSIId = d.ARSIId,
                                                    ARSI = d.TrnSalesInvoice.SINumber,
                                                    ARSIBranch = d.TrnSalesInvoice.MstBranch.Branch,
                                                    IsClear = d.IsClear
                                                };


            return journalVoucherLines.Union(journalVoucherLinesIsNotClear).ToList();
        }

        // list journal voucher lune by JVId
        [Authorize]
        [HttpGet]
        [Route("api/listJournalVoucherLineByJVId/{JVId}")]
        public List<Models.TrnJournalVoucherLine> listJournalVoucherLineByJVId(String JVId)
        {
            var journalVoucherLines = from d in db.TrnJournalVoucherLines
                                      where d.JVId == Convert.ToInt32(JVId)
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
                                              APRR = d.TrnReceivingReceipt.RRNumber,
                                              APRRBranch = d.TrnReceivingReceipt.MstBranch.Branch,
                                              ARSIId = d.ARSIId,
                                              ARSI = d.TrnSalesInvoice.SINumber,
                                              ARSIBranch = d.TrnSalesInvoice.MstBranch.Branch,
                                              IsClear = d.IsClear
                                          };

            return journalVoucherLines.ToList();
        }

        // add journal voucher line
        [Authorize]
        [HttpPost]
        [Route("api/addJournalVoucherLine")]
        public Int32 insertJournalVoucherLine(Models.TrnJournalVoucherLine journalVoucherLine)
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

        // update journal voucher line
        [Authorize]
        [HttpPut]
        [Route("api/updateJournalVoucherLine/{id}")]
        public HttpResponseMessage updateJournalVoucherLine(String id, Models.TrnJournalVoucherLine journalVoucherLine)
        {
            try
            {
                var journalVoucherLines = from d in db.TrnJournalVoucherLines where d.Id == Convert.ToInt32(id) select d;
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

        // delete journal voucher line
        [Authorize]
        [HttpDelete]
        [Route("api/deleteJournalVoucherLine/{id}")]
        public HttpResponseMessage deleteJournalVoucherLine(String id)
        {
            try
            {
                var journalVoucherLines = from d in db.TrnJournalVoucherLines where d.Id == Convert.ToInt32(id) select d;
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
