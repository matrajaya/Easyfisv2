using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiCollectionDetailReportController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();
        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        public Decimal getAmount(Int32 id)
        {
            var purchaseOrderItems = from d in db.TrnPurchaseOrderItems where d.POId == id select d;
            Decimal amount = purchaseOrderItems.Sum(d => d.Amount);

            return amount;
        }

        // list account
        [Authorize]
        [HttpGet]
        [Route("api/collectionDetailReport/list/{startDate}/{endDate}")]
        public List<Models.TrnCollectionLine> listCollectionDetailReport(String startDate, String endDate)
        {
            // purchase orders
            var collectionLines = from d in db.TrnCollectionLines
                                  where d.TrnCollection.BranchId == currentBranchId()
                                  && d.TrnCollection.ORDate >= Convert.ToDateTime(startDate)
                                  && d.TrnCollection.ORDate <= Convert.ToDateTime(endDate)
                                  && d.TrnCollection.IsLocked == true
                                  select new Models.TrnCollectionLine
                                  {
                                      Id = d.Id,
                                      OR = d.TrnCollection.ORNumber,
                                      ORDate = d.TrnCollection.ORDate.ToShortDateString(),
                                      SI = d.TrnSalesInvoice.SINumber,
                                      Amount = d.Amount,
                                      DepositoryBank = d.MstArticle1.Article,
                                      PayType = d.MstPayType.PayType
                                  };

            return collectionLines.ToList();
        }
    }
}

