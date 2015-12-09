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
                                      //OR = d.OR,
                                      BranchId = d.BranchId,
                                      //Branch = d.Branch,
                                      AccountId = d.AccountId,
                                      //Account = d.Account,
                                      ArticleId = d.ArticleId,
                                      //Article = d.Article,
                                      //SIId = d.SIId,
                                      //SI = d.SI,
                                      Particulars = d.Particulars,
                                      Amount = d.Amount,
                                      PayTypeId = d.PayTypeId,
                                      //PayType = d.PayType,
                                      CheckNumber = d.CheckNumber,
                                      //CheckDate = d.CheckDate,
                                      CheckBank = d.CheckBank,
                                      //DepositoryBankId = d.DepositoryBankId,
                                      //DepositoryBank = d.DepositoryBank,
                                      IsClear = d.IsClear,
                                  };
            return collectionLines.ToList();
        }
    }
}
