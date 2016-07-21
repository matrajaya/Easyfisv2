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
    public class ApiCollectionLineController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            return (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.BranchId).SingleOrDefault();
        }

        // list colletion line
        [Authorize]
        [HttpGet]
        [Route("api/listCollectionLine")]
        public List<Models.TrnCollectionLine> listCollectionLine()
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

        // list collection line by ORId
        [Authorize]
        [HttpGet]
        [Route("api/listCollectionLineByORId/{ORId}")]
        public List<Models.TrnCollectionLine> listCollectionLineByORId(String ORId)
        {
            var collectionLines = from d in db.TrnCollectionLines
                                  where d.ORId == Convert.ToInt32(ORId)
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

        // list collection line by depositoryBankId and by ORDate for bank reconciliation
        [Authorize]
        [HttpGet]
        [Route("api/listCollectionLineByDepositoryBankIdByORDate/{depositoryBankId}/{dateStart}/{dateEnd}")]
        public List<Models.TrnCollectionLine> listCollectionLineDepositoryBankIdByORDate(String depositoryBankId, String dateStart, String dateEnd)
        {
            var collectionLines = from d in db.TrnCollectionLines
                                  where d.DepositoryBankId == Convert.ToInt32(depositoryBankId)
                                  && d.TrnCollection.ORDate >= Convert.ToDateTime(dateStart)
                                  && d.TrnCollection.ORDate <= Convert.ToDateTime(dateEnd)
                                  && d.Amount > 0
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

            var collectionLinesIsNotClear = from d in db.TrnCollectionLines
                                            where d.DepositoryBankId == Convert.ToInt32(depositoryBankId)
                                            && d.TrnCollection.ORDate < Convert.ToDateTime(dateStart)
                                            && d.IsClear == false
                                            && d.Amount > 0
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

            return collectionLines.Union(collectionLinesIsNotClear).ToList();
        }

        // add collection line
        [Authorize]
        [HttpPost]
        [Route("api/addCollectionLine")]
        public Int32 insertCollectionLine(Models.TrnCollectionLine collectionLine)
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

        // update collection line
        [Authorize]
        [HttpPut]
        [Route("api/updateCollectionLine/{id}")]
        public HttpResponseMessage updateCollectionLine(String id, Models.TrnCollectionLine collectionLine)
        {
            try
            {
                var collectionLines = from d in db.TrnCollectionLines where d.Id == Convert.ToInt32(id) select d;
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
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // delete collection line
        [Authorize]
        [HttpDelete]
        [Route("api/deleteCollectionLine/{id}")]
        public HttpResponseMessage deleteCollectionLine(String id)
        {
            try
            {
                var collectionLines = from d in db.TrnCollectionLines where d.Id == Convert.ToInt32(id) select d;
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

        // pick accounts receivable from sales invoice
        [Authorize]
        [HttpPost]
        [Route("api/collectionLine/applyAR/BySalesId/{SalesId}/{ORId}")]
        public HttpResponseMessage insertCollectionLineAccountsReceivable(String SalesId, String ORId)
        {
            try
            {
                var salesInvoices = from d in db.TrnSalesInvoices
                                    where d.BranchId == currentBranchId()
                                    && d.Id == Convert.ToInt32(SalesId)
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
                        newCollectionLine.BranchId = salesInvoice.BranchId;
                        newCollectionLine.AccountId = salesInvoice.AccountId;
                        newCollectionLine.ArticleId = salesInvoice.CustomerId;
                        newCollectionLine.SIId = Convert.ToInt32(SalesId);
                        newCollectionLine.Particulars = salesInvoice.DocumentReference;
                        newCollectionLine.Amount = salesInvoice.Amount;
                        newCollectionLine.PayTypeId = (from d in db.MstPayTypes select d.Id).FirstOrDefault();
                        newCollectionLine.CheckNumber = "NA";
                        newCollectionLine.CheckDate = DateTime.Now;
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

        // apply all accounts receivable from sales invoice
        [Authorize]
        [HttpPost]
        [Route("api/collectionLine/applyAR/{ORId}")]
        public HttpResponseMessage PostAllAR(Models.TrnSalesInvoice salesInvoice, String ORId)
        {
            try
            {
                Data.TrnCollectionLine newCollectionLine = new Data.TrnCollectionLine();

                newCollectionLine.ORId = Convert.ToInt32(ORId);
                newCollectionLine.BranchId = salesInvoice.BranchId;
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

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // pick advances from journals
        [Authorize]
        [HttpPost]
        [Route("api/collectionLine/applyAdvances/ByArticleId/{articleId}/{ORId}")]
        public HttpResponseMessage PostAdvances(Models.TrnDisbursementLine disbursementLine, String articleId, String ORId)
        {
            try
            {
                var CustomerAdvancesAccountId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.CustomerAdvancesAccountId).SingleOrDefault();

                var journals = from d in db.TrnJournals
                               where d.ArticleId == Convert.ToInt32(articleId)
                               && d.AccountId == CustomerAdvancesAccountId
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
                                   Balance = g.Sum(d => d.CreditAmount) - g.Sum(d => d.DebitAmount)
                               };

                if (journals.Any())
                {
                    Data.TrnCollectionLine newCollectionLine = new Data.TrnCollectionLine();

                    var accountId = 0;
                    foreach (var journal in journals)
                    {
                        accountId = (from d in db.MstArticles where d.Id == journal.ArticleId select d.AccountId).SingleOrDefault();

                        newCollectionLine.ORId = Convert.ToInt32(ORId);
                        newCollectionLine.BranchId = journal.BranchId;
                        newCollectionLine.AccountId = accountId;
                        newCollectionLine.ArticleId = journal.ArticleId;
                        newCollectionLine.SIId = (from d in db.TrnSalesInvoices select d.Id).FirstOrDefault();
                        newCollectionLine.Particulars = "Customer Advances";
                        newCollectionLine.Amount = journal.Balance;
                        newCollectionLine.PayTypeId = (from d in db.MstPayTypes select d.Id).FirstOrDefault();
                        newCollectionLine.CheckNumber = "NA";
                        newCollectionLine.CheckDate = DateTime.Now;
                        newCollectionLine.CheckBank = "NA";
                        newCollectionLine.DepositoryBankId = null;
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
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
