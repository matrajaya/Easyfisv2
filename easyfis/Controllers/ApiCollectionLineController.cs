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
                                      IsClear = d.IsClear,
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
                                      IsClear = d.IsClear,
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
    }
}
