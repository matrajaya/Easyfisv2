using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiCollectionLineController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ====================
        // LIST Collection Line
        // ====================
        [Route("api/listCollectionLine")]
        public List<Models.TrnCollectionLine> Get()
        {
            var collectionLines = from d in db.TrnCollectionLines
                                  select new Models.TrnCollectionLine
                                  {
                                      Id = d.Id,
                                      ORId = d.ORId,
                                      OR = d.TrnCollection.ORNumber,
                                      ORDate = d.TrnCollection.ORDate.ToShortDateString(),
                                      Customer = d.TrnCollection.MstArticle.Article,
                                      BranchId = d.BranchId,
                                      Branch = d.MstBranch.Branch,
                                      AccountId = d.AccountId,
                                      Account = d.MstAccount.Account,
                                      ArticleId = d.ArticleId,
                                      Article = d.MstArticle.Article,
                                      SIId = d.SIId,
                                      SI = d.TrnSalesInvoice.SINumber,
                                      Particulars = d.Particulars,
                                      Amount = d.Amount,
                                      PayTypeId = d.PayTypeId,
                                      PayType = d.MstPayType.PayType,
                                      CheckNumber = d.CheckNumber,
                                      CheckDate = d.CheckDate.ToShortDateString(),
                                      CheckBank = d.CheckBank,
                                      DepositoryBankId = d.DepositoryBankId,
                                      DepositoryBank = d.MstArticle1.Article,
                                      IsClear = d.IsClear
                                  };
            return collectionLines.ToList();
        }

        // ============================
        // LIST Collection Line By ORId
        // ============================
        [Route("api/listCollectionLineByORId/{ORId}")]
        public List<Models.TrnCollectionLine> GetCollectionByORId(String ORId)
        {
            var collectionLine_ORId = Convert.ToInt32(ORId);
            var collectionLines = from d in db.TrnCollectionLines
                                  where d.ORId == collectionLine_ORId
                                  select new Models.TrnCollectionLine
                                  {
                                      Id = d.Id,
                                      ORId = d.ORId,
                                      OR = d.TrnCollection.ORNumber,
                                      ORDate = d.TrnCollection.ORDate.ToShortDateString(),
                                      Customer = d.TrnCollection.MstArticle.Article,
                                      BranchId = d.BranchId,
                                      Branch = d.MstBranch.Branch,
                                      AccountId = d.AccountId,
                                      Account = d.MstAccount.Account,
                                      ArticleId = d.ArticleId,
                                      Article = d.MstArticle.Article,
                                      SIId = d.SIId,
                                      SI = d.TrnSalesInvoice.SINumber,
                                      Particulars = d.Particulars,
                                      Amount = d.Amount,
                                      PayTypeId = d.PayTypeId,
                                      PayType = d.MstPayType.PayType,
                                      CheckNumber = d.CheckNumber,
                                      CheckDate = d.CheckDate.ToShortDateString(),
                                      CheckBank = d.CheckBank,
                                      DepositoryBankId = d.DepositoryBankId,
                                      DepositoryBank = d.MstArticle1.Article,
                                      IsClear = d.IsClear
                                  };
            return collectionLines.ToList();
        }

        // ===================================================================
        // LIST Collection Line By dispository Bank and Date start to Date end
        // ===================================================================
        [Route("api/listCollectionLineByDepositoryBankIdByORDate/{DepositoryBankId}/{DateStart}/{DateEnd}")]
        public List<Models.TrnCollectionLine> GetCollectionDepositoryBankIdByORDate(String DepositoryBankId, String DateStart, String DateEnd)
        {
            var collectionLines = from d in db.TrnCollectionLines
                                  where d.DepositoryBankId == Convert.ToInt32(DepositoryBankId)
                                  && d.TrnCollection.ORDate >= Convert.ToDateTime(DateStart)
                                  && d.TrnCollection.ORDate <= Convert.ToDateTime(DateEnd)
                                  select new Models.TrnCollectionLine
                                  {
                                      Id = d.Id,
                                      ORId = d.ORId,
                                      OR = d.TrnCollection.ORNumber,
                                      ORDate = d.TrnCollection.ORDate.ToShortDateString(),
                                      Customer = d.TrnCollection.MstArticle.Article,
                                      BranchId = d.BranchId,
                                      Branch = d.MstBranch.Branch,
                                      AccountId = d.AccountId,
                                      Account = d.MstAccount.Account,
                                      ArticleId = d.ArticleId,
                                      Article = d.MstArticle.Article,
                                      SIId = d.SIId,
                                      SI = d.TrnSalesInvoice.SINumber,
                                      Particulars = d.Particulars,
                                      Amount = d.Amount,
                                      PayTypeId = d.PayTypeId,
                                      PayType = d.MstPayType.PayType,
                                      CheckNumber = d.CheckNumber,
                                      CheckDate = d.CheckDate.ToShortDateString(),
                                      CheckBank = d.CheckBank,
                                      DepositoryBankId = d.DepositoryBankId,
                                      DepositoryBank = d.MstArticle1.Article,
                                      IsClear = d.IsClear
                                  };
            return collectionLines.ToList();
        }

        // ===================
        // ADD Collection Line
        // ===================
        [Route("api/addCollectionLine")]
        public int Post(Models.TrnCollectionLine collectionLine)
        {
            try
            {
                Data.TrnCollectionLine newCollectionLine = new Data.TrnCollectionLine();

                newCollectionLine.ORId = collectionLine.ORId;
                newCollectionLine.BranchId = collectionLine.BranchId;
                newCollectionLine.AccountId = collectionLine.AccountId;
                newCollectionLine.ArticleId = collectionLine.ArticleId;
                newCollectionLine.SIId = collectionLine.SIId;
                newCollectionLine.Particulars = collectionLine.Particulars;
                newCollectionLine.Amount = collectionLine.Amount;
                newCollectionLine.PayTypeId = collectionLine.PayTypeId;
                newCollectionLine.CheckNumber = collectionLine.CheckNumber;
                newCollectionLine.CheckDate = Convert.ToDateTime(collectionLine.CheckDate);
                newCollectionLine.CheckBank = collectionLine.CheckBank;
                newCollectionLine.DepositoryBankId = collectionLine.DepositoryBankId;
                newCollectionLine.IsClear = collectionLine.IsClear;

                db.TrnCollectionLines.InsertOnSubmit(newCollectionLine);
                db.SubmitChanges();

                return newCollectionLine.Id;
            }
            catch
            {
                return 0;
            }
        }

        // ======================
        // UPDATE Collection Line
        // ======================
        [Route("api/updateCollectionLine/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnCollectionLine collectionLine)
        {
            try
            {
                var collectionLineId = Convert.ToInt32(id);
                var collectionLines = from d in db.TrnCollectionLines where d.Id == collectionLineId select d;

                if (collectionLines.Any())
                {
                    var updateCollectionLine = collectionLines.FirstOrDefault();

                    updateCollectionLine.ORId = collectionLine.ORId;
                    updateCollectionLine.BranchId = collectionLine.BranchId;
                    updateCollectionLine.AccountId = collectionLine.AccountId;
                    updateCollectionLine.ArticleId = collectionLine.ArticleId;
                    updateCollectionLine.SIId = collectionLine.SIId;
                    updateCollectionLine.Particulars = collectionLine.Particulars;
                    updateCollectionLine.Amount = collectionLine.Amount;
                    updateCollectionLine.PayTypeId = collectionLine.PayTypeId;
                    updateCollectionLine.CheckNumber = collectionLine.CheckNumber;
                    updateCollectionLine.CheckDate = Convert.ToDateTime(collectionLine.CheckDate);
                    updateCollectionLine.CheckBank = collectionLine.CheckBank;
                    updateCollectionLine.DepositoryBankId = collectionLine.DepositoryBankId;
                    updateCollectionLine.IsClear = collectionLine.IsClear;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // ======================
        // DELETE Collection Line
        // ======================
        [Route("api/deleteCollectionLine/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var collectionLineId = Convert.ToInt32(id);
                var collectionLines = from d in db.TrnCollectionLines where d.Id == collectionLineId select d;

                if (collectionLines.Any())
                {
                    db.TrnCollectionLines.DeleteOnSubmit(collectionLines.First());
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

        // ========
        // Apply AR
        // ========
        [HttpPost]
        [Route("api/collectionLine/applyAR/BySalesId/{SalesId}/{ORId}/{BranchId}")]
        public HttpResponseMessage PostAR(String SalesId, String ORId, String BranchId)
        {
            try
            {
                var salesInvoices = from d in db.TrnSalesInvoices
                                    where d.Id == Convert.ToInt32(SalesId)
                                    select new Models.TrnSalesInvoice
                                    {
                                        Id = d.Id,
                                        BranchId = d.BranchId,
                                        Branch = d.MstBranch.Branch,
                                        SINumber = d.SINumber,
                                        SIDate = d.SIDate.ToShortDateString(),
                                        CustomerId = d.CustomerId,
                                        Customer = d.MstArticle.Article,
                                        TermId = d.TermId,
                                        Term = d.MstTerm.Term,
                                        DocumentReference = d.DocumentReference,
                                        ManualSINumber = d.ManualSINumber,
                                        Remarks = d.Remarks,
                                        Amount = d.Amount,
                                        PaidAmount = d.PaidAmount,
                                        AdjustmentAmount = d.AdjustmentAmount,
                                        BalanceAmount = d.BalanceAmount,
                                        AccountId = d.MstArticle.MstAccount.Id
                                    };

                if (salesInvoices.Any())
                {

                    Data.TrnCollectionLine newCollectionLine = new Data.TrnCollectionLine();

                    foreach (var salesInvoice in salesInvoices)
                    {
                        newCollectionLine.ORId = Convert.ToInt32(ORId);
                        newCollectionLine.BranchId = Convert.ToInt32(BranchId);
                        newCollectionLine.AccountId = salesInvoice.AccountId;
                        newCollectionLine.ArticleId = salesInvoice.CustomerId;
                        newCollectionLine.SIId = Convert.ToInt32(SalesId);
                        newCollectionLine.Particulars = salesInvoice.DocumentReference;
                        newCollectionLine.Amount = salesInvoice.Amount;
                        newCollectionLine.PayTypeId = (from d in db.MstPayTypes select d.Id).FirstOrDefault();
                        newCollectionLine.CheckNumber = "NA";
                        newCollectionLine.CheckDate =  DateTime.Now;
                        newCollectionLine.CheckBank = "NA";
                        newCollectionLine.DepositoryBankId = (from d in db.MstArticles where d.ArticleTypeId == 5 select d.Id).FirstOrDefault();
                        newCollectionLine.IsClear = true;
                    }

                    db.TrnCollectionLines.InsertOnSubmit(newCollectionLine);
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


        // ============
        // Apply All AR
        // ============
        [HttpPost]
        [Route("api/collectionLine/applyAllAR/ByCustomerId/{CustomerId}/{ORId}/{BranchId}")]
        public HttpResponseMessage PostAllAR(String CustomerId, String ORId, String BranchId)
        {
            try
            {
                var salesInvoices = from d in db.TrnSalesInvoices
                                    where d.CustomerId == Convert.ToInt32(CustomerId)
                                    && d.BalanceAmount > 0
                                    && d.IsLocked == true
                                    select new Models.TrnSalesInvoice
                                    {
                                        Id = d.Id,
                                        BranchId = d.BranchId,
                                        Branch = d.MstBranch.Branch,
                                        SINumber = d.SINumber,
                                        SIDate = d.SIDate.ToShortDateString(),
                                        CustomerId = d.CustomerId,
                                        Customer = d.MstArticle.Article,
                                        TermId = d.TermId,
                                        Term = d.MstTerm.Term,
                                        DocumentReference = d.DocumentReference,
                                        ManualSINumber = d.ManualSINumber,
                                        Remarks = d.Remarks,
                                        Amount = d.Amount,
                                        PaidAmount = d.PaidAmount,
                                        AdjustmentAmount = d.AdjustmentAmount,
                                        BalanceAmount = d.BalanceAmount,
                                        AccountId = d.MstArticle.MstAccount.Id
                                    };

                if (salesInvoices.Any())
                {
                    foreach (var salesInvoice in salesInvoices)
                    {
                        Data.TrnCollectionLine newCollectionLine = new Data.TrnCollectionLine();

                        newCollectionLine.ORId = Convert.ToInt32(ORId);
                        newCollectionLine.BranchId = Convert.ToInt32(BranchId);
                        newCollectionLine.AccountId = salesInvoice.AccountId;
                        newCollectionLine.ArticleId = salesInvoice.CustomerId;
                        newCollectionLine.SIId = salesInvoice.Id;
                        newCollectionLine.Particulars = salesInvoice.DocumentReference;
                        newCollectionLine.Amount = salesInvoice.Amount;
                        newCollectionLine.PayTypeId = (from d in db.MstPayTypes select d.Id).FirstOrDefault();
                        newCollectionLine.CheckNumber = "NA";
                        newCollectionLine.CheckDate = DateTime.Now;
                        newCollectionLine.CheckBank = "NA";
                        newCollectionLine.DepositoryBankId = (from d in db.MstArticles where d.ArticleTypeId == 5 select d.Id).FirstOrDefault();
                        newCollectionLine.IsClear = true;

                        db.TrnCollectionLines.InsertOnSubmit(newCollectionLine);
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

        // ============
        // ADD Advances
        // ============
        [HttpPost]
        [Route("api/collectionLine/applyAdvances/ByJournalId/{JournalId}/{ORId}/{BranchId}")]
        public int PostAdvances(Models.TrnDisbursementLine disbursementLine, String JournalId, String ORId, String BranchId)
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

                Data.TrnCollectionLine newCollectionLine = new Data.TrnCollectionLine();
                var accountId = 0;

                foreach (var journal in journals)
                {
                    accountId = (from d in db.MstArticles where d.Id == journal.ArticleId select d.AccountId).SingleOrDefault();

                    newCollectionLine.ORId = Convert.ToInt32(ORId);
                    newCollectionLine.BranchId = Convert.ToInt32(BranchId);
                    newCollectionLine.AccountId = accountId;
                    newCollectionLine.ArticleId = journal.ArticleId;
                    newCollectionLine.SIId = (from d in db.TrnSalesInvoices select d.Id).FirstOrDefault();
                    newCollectionLine.Particulars = "Customer Advances";
                    newCollectionLine.Amount = journal.CreditAmount - journal.DebitAmount;
                    newCollectionLine.PayTypeId = (from d in db.MstPayTypes where d.AccountId == journal.AccountId select d.Id).FirstOrDefault();
                    newCollectionLine.CheckNumber = "NA";
                    newCollectionLine.CheckDate = DateTime.Now;
                    newCollectionLine.CheckBank = "NA";
                    newCollectionLine.DepositoryBankId = null;
                    newCollectionLine.IsClear = true;
                }

                db.TrnCollectionLines.InsertOnSubmit(newCollectionLine);
                db.SubmitChanges();

                return newCollectionLine.Id;
            }
            catch
            {
                return 0;
            }
        }

    }
}
