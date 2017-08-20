using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiDisbursementLineController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            return (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.BranchId).SingleOrDefault();
        }

        // list disbursement line
        [Authorize]
        [HttpGet]
        [Route("api/listDisbursementLine")]
        public List<Models.TrnDisbursementLine> listDisbursementLine()
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

        // pick accounts payable from receiving receipts
        [Authorize]
        [HttpPost]
        [Route("api/disbursementLine/applyAP/{CVId}")]
        public HttpResponseMessage insertDisbursementLineAccountsPayable(Models.TrnReceivingReceipt receivingReceipt, String CVId)
        {
            try
            {
                Data.TrnDisbursementLine newDisbursementLine = new Data.TrnDisbursementLine();

                newDisbursementLine.CVId = Convert.ToInt32(CVId);
                newDisbursementLine.BranchId = receivingReceipt.BranchId;
                newDisbursementLine.AccountId = receivingReceipt.AccountId;
                newDisbursementLine.ArticleId = receivingReceipt.SupplierId;
                newDisbursementLine.RRId = receivingReceipt.Id;
                newDisbursementLine.Particulars = receivingReceipt.DocumentReference;
                newDisbursementLine.Amount = receivingReceipt.Amount;

                db.TrnDisbursementLines.InsertOnSubmit(newDisbursementLine);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // apply all accounts payable from receiving receipts
        [Authorize]
        [HttpPost]
        [Route("api/disbursementLine/applyAllAP/BySupplierId/{supplierId}/{CVId}")]
        public HttpResponseMessage insertDisbursementLineAllAccountsPayable(Models.TrnDisbursementLine disbursementLine, String supplierId, String CVId)
        {
            try
            {
                var receivingReceipts = from d in db.TrnReceivingReceipts
                                        where d.BranchId == currentBranchId()
                                        && d.SupplierId == Convert.ToInt32(supplierId)
                                        && d.BalanceAmount > 0
                                        && d.IsLocked == true
                                        select new Models.TrnReceivingReceipt
                                        {
                                            Id = d.Id,
                                            BranchId = d.BranchId,
                                            Branch = d.MstBranch.Branch,
                                            RRDate = d.RRDate.ToShortDateString(),
                                            RRNumber = d.RRNumber,
                                            SupplierId = d.SupplierId,
                                            Supplier = d.MstArticle.Article,
                                            AccountId = d.MstArticle.AccountId,
                                            TermId = d.TermId,
                                            Term = d.MstTerm.Term,
                                            DocumentReference = d.DocumentReference,
                                            ManualRRNumber = d.ManualRRNumber,
                                            Remarks = d.Remarks,
                                            Amount = d.Amount,
                                            WTaxAmount = d.WTaxAmount,
                                            PaidAmount = d.PaidAmount,
                                            AdjustmentAmount = d.AdjustmentAmount,
                                            BalanceAmount = d.BalanceAmount,
                                            ReceivedById = d.ReceivedById,
                                            ReceivedBy = d.MstUser4.FullName,
                                            PreparedById = d.PreparedById,
                                            PreparedBy = d.MstUser3.FullName,
                                            CheckedById = d.CheckedById,
                                            CheckedBy = d.MstUser1.FullName,
                                            ApprovedById = d.ApprovedById,
                                            ApprovedBy = d.MstUser.FullName
                                        };

                if (receivingReceipts.Any())
                {
                    foreach (var receivingReceipt in receivingReceipts)
                    {
                        Data.TrnDisbursementLine newDisbursementLine = new Data.TrnDisbursementLine();

                        newDisbursementLine.CVId = Convert.ToInt32(CVId);
                        newDisbursementLine.BranchId = receivingReceipt.BranchId;
                        newDisbursementLine.AccountId = receivingReceipt.AccountId;
                        newDisbursementLine.ArticleId = receivingReceipt.SupplierId;
                        newDisbursementLine.RRId = receivingReceipt.Id;
                        newDisbursementLine.Particulars = receivingReceipt.DocumentReference;
                        newDisbursementLine.Amount = receivingReceipt.Amount;

                        db.TrnDisbursementLines.InsertOnSubmit(newDisbursementLine);
                        db.SubmitChanges();
                    }

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

        // pick advances from journals
        [Authorize]
        [HttpPost]
        [Route("api/disbursementLine/applyAdvances/ByArticleId/{articleId}/{CVId}")]
        public HttpResponseMessage insertDisbursementLineAdvances(Models.TrnDisbursementLine disbursementLine, String articleId, String CVId)
        {
            try
            {
                var SupplierAdvancesAccountId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.SupplierAdvancesAccountId).SingleOrDefault();

                var journals = from d in db.TrnJournals
                               where d.ArticleId == Convert.ToInt32(articleId)
                               && d.AccountId == SupplierAdvancesAccountId
                               && d.BranchId == currentBranchId()
                               group d by new
                               {
                                   BranchId = d.BranchId,
                                   Branch = d.MstBranch.Branch,
                                   AccountId = d.AccountId,
                                   Account = d.MstAccount.Account,
                                   AccountCode = d.MstAccount.AccountCode,
                                   ArticleId = d.ArticleId,
                                   Article = d.MstArticle.Article,
                                   RRId = d.RRId
                               } into g
                               select new Models.TrnJournal
                               {
                                   BranchId = g.Key.BranchId,
                                   Branch = g.Key.Branch,
                                   AccountId = g.Key.AccountId,
                                   Account = g.Key.Account,
                                   AccountCode = g.Key.AccountCode,
                                   ArticleId = g.Key.ArticleId,
                                   Article = g.Key.Article,
                                   RRId = g.Key.RRId,
                                   DebitAmount = g.Sum(d => d.DebitAmount),
                                   CreditAmount = g.Sum(d => d.CreditAmount),
                                   Balance = g.Sum(d => d.DebitAmount) - g.Sum(d => d.CreditAmount)
                               };

                if (journals.Any())
                {
                    Data.TrnDisbursementLine newDisbursementLine = new Data.TrnDisbursementLine();

                    foreach (var journal in journals)
                    {
                        newDisbursementLine.CVId = Convert.ToInt32(CVId);
                        newDisbursementLine.BranchId = journal.BranchId;
                        newDisbursementLine.AccountId = journal.AccountId;
                        newDisbursementLine.ArticleId = journal.ArticleId;
                        newDisbursementLine.RRId = journal.RRId;
                        newDisbursementLine.Particulars = "Supplier Advances";
                        newDisbursementLine.Amount = journal.Balance;
                    }

                    db.TrnDisbursementLines.InsertOnSubmit(newDisbursementLine);
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

        // list disbursement line by CVId
        [Authorize]
        [HttpGet]
        [Route("api/listDisbursementLineByCVId/{CVId}")]
        public List<Models.TrnDisbursementLine> listDisbursementLineByCVId(String CVId)
        {
            var disbursementLines = from d in db.TrnDisbursementLines
                                    where d.CVId == Convert.ToInt32(CVId)
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

        // add disbursement line
        [Authorize]
        [HttpPost]
        [Route("api/addDisbursementLine")]
        public Int32 insertDisbursementLine(Models.TrnDisbursementLine disbursementLine)
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

                var disbursement = from d in db.TrnDisbursements
                                   where d.Id == disbursementLine.CVId
                                   select d;

                if (disbursement.Any())
                {
                    var updateDisbursement = disbursement.FirstOrDefault();
                    updateDisbursement.Amount = disbursement.FirstOrDefault().TrnDisbursementLines.Sum(d => d.Amount);
                    db.SubmitChanges();
                }

                return newDisbursementLine.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update disbursement line
        [Authorize]
        [HttpPut]
        [Route("api/updateDisbursementLine/{id}")]
        public HttpResponseMessage updateDisbursementLine(String id, Models.TrnDisbursementLine disbursementLine)
        {
            try
            {
                var disbursementLines = from d in db.TrnDisbursementLines where d.Id == Convert.ToInt32(id) select d;
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

                    var disbursement = from d in db.TrnDisbursements
                                       where d.Id == disbursementLines.FirstOrDefault().CVId
                                       select d;

                    if (disbursement.Any())
                    {
                        var updateDisbursement = disbursement.FirstOrDefault();
                        updateDisbursement.Amount = disbursement.FirstOrDefault().TrnDisbursementLines.Sum(d => d.Amount);
                        db.SubmitChanges();
                    }

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

        // delete disbursement line
        [Authorize]
        [HttpDelete]
        [Route("api/deleteDisbursementLine/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var disbursementLines = from d in db.TrnDisbursementLines where d.Id == Convert.ToInt32(id) select d;
                if (disbursementLines.Any())
                {
                    var CVId = disbursementLines.FirstOrDefault().CVId;

                    db.TrnDisbursementLines.DeleteOnSubmit(disbursementLines.First());
                    db.SubmitChanges();

                    var disbursement = from d in db.TrnDisbursements
                                       where d.Id == CVId
                                       select d;

                    if (disbursement.Any())
                    {
                        var updateDisbursement = disbursement.FirstOrDefault();
                        updateDisbursement.Amount = disbursement.FirstOrDefault().TrnDisbursementLines.Sum(d => d.Amount);
                        db.SubmitChanges();
                    }

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
