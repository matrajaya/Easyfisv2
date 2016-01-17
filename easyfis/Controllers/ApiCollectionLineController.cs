using System;
using System.Collections.Generic;
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
    }
}
