using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        // ======
        // ADD AP
        // ======
        [HttpPost]
        [Route("api/disbursementLine/applyAP/ByRRId/{RRId}/{CVId}/{BranchId}")]
        public int PostAP(Models.TrnDisbursementLine disbursementLine, String RRId, String CVId, String BranchId)
        {
            try
            {
                var receivingReceipts = from d in db.TrnReceivingReceipts
                                        where d.Id == Convert.ToInt32(RRId)
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


                Data.TrnDisbursementLine newDisbursementLine = new Data.TrnDisbursementLine();

                foreach (var receivingReceipt in receivingReceipts)
                {
                    newDisbursementLine.CVId = Convert.ToInt32(CVId);
                    newDisbursementLine.BranchId = Convert.ToInt32(BranchId);
                    newDisbursementLine.AccountId = receivingReceipt.AccountId;
                    newDisbursementLine.ArticleId = receivingReceipt.SupplierId;
                    newDisbursementLine.RRId = Convert.ToInt32(RRId);
                    newDisbursementLine.Particulars = receivingReceipt.DocumentReference;
                    newDisbursementLine.Amount = receivingReceipt.Amount;
                }

                db.TrnDisbursementLines.InsertOnSubmit(newDisbursementLine);
                db.SubmitChanges();

                return newDisbursementLine.Id;
            }
            catch
            {
                return 0;
            }
        }

        // ==========
        // ADD ALL AP
        // ==========
        [HttpPost]
        [Route("api/disbursementLine/applyAllAP/BySupplierId/{SupplierId}/{CVId}/{BranchId}")]
        public HttpResponseMessage PostAllAP(Models.TrnDisbursementLine disbursementLine, String SupplierId, String CVId, String BranchId)
        {
            try
            {
                var receivingReceipts = from d in db.TrnReceivingReceipts
                                        where d.SupplierId == Convert.ToInt32(SupplierId)
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

                foreach (var receivingReceipt in receivingReceipts)
                {
                    Data.TrnDisbursementLine newDisbursementLine = new Data.TrnDisbursementLine();

                    newDisbursementLine.CVId = Convert.ToInt32(CVId);
                    newDisbursementLine.BranchId = Convert.ToInt32(BranchId);
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
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // ============
        // ADD Advances
        // ============
        [HttpPost]
        [Route("api/disbursementLine/applyAdvances/ByJournalId/{JournalId}/{CVId}/{BranchId}")]
        public int PostAdvances(Models.TrnDisbursementLine disbursementLine, String JournalId, String CVId, String BranchId)
        {
            try
            {
                var journals = from d in db.TrnJournals
                               where d.Id == Convert.ToInt32(JournalId)
                               select new Models.TrnJournal
                               {
                                   Id = d.Id,
                                   JournalDate = d.JournalDate.ToShortDateString(),
                                   BranchId = d.BranchId,
                                   Branch = d.MstBranch.Branch,
                                   AccountId = d.AccountId,
                                   Account = d.MstAccount.Account,
                                   AccountCode = d.MstAccount.AccountCode,
                                   ArticleId = d.ArticleId,
                                   Article = d.MstArticle.Article,
                                   Particulars = d.Particulars,
                                   DebitAmount = d.DebitAmount,
                                   CreditAmount = d.CreditAmount,
                                   ORId = d.ORId,
                                   CVId = d.CVId,
                                   JVId = d.JVId,
                                   RRId = d.RRId,
                                   SIId = d.SIId,
                                   INId = d.INId,
                                   OTId = d.OTId,
                                   STId = d.STId,
                                   DocumentReference = d.DocumentReference,
                                   APRRId = d.APRRId,
                                   ARSIId = d.ARSIId,
                               };

                
                Data.TrnDisbursementLine newDisbursementLine = new Data.TrnDisbursementLine();

                foreach (var journal in journals) 
                {
                    newDisbursementLine.CVId = Convert.ToInt32(CVId);
                    newDisbursementLine.BranchId = Convert.ToInt32(BranchId);
                    newDisbursementLine.AccountId = journal.AccountId;
                    newDisbursementLine.ArticleId = journal.ArticleId;
                    newDisbursementLine.RRId = journal.RRId;
                    newDisbursementLine.Particulars = "Supplier Advances";
                    newDisbursementLine.Amount = journal.CreditAmount - journal.DebitAmount;
                }

                db.TrnDisbursementLines.InsertOnSubmit(newDisbursementLine);
                db.SubmitChanges();

                return newDisbursementLine.Id;
            }
            catch
            {
                return 0;
            }
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
