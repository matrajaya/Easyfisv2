using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiPurchaseSummaryReportController : ApiController
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
            Decimal amount = 0;

            var purchaseOrderItems = from d in db.TrnPurchaseOrderItems where d.POId == id select d;
            if (purchaseOrderItems.Any())
            {
                amount = purchaseOrderItems.Sum(d => d.Amount);
            }

            return amount;
        }

        // list account
        [Authorize]
        [HttpGet]
        [Route("api/purchaseSummaryReport/list/{startDate}/{endDate}")]
        public List<Models.TrnPurchaseOrder> listPurchaseSummaryReport(String startDate, String endDate)
        {
            // purchase orders
            var purchaseOrders = from d in db.TrnPurchaseOrders
                                 where d.BranchId == currentBranchId()
                                 && d.PODate >= Convert.ToDateTime(startDate)
                                 && d.PODate <= Convert.ToDateTime(endDate)
                                 && d.IsLocked == true
                                 select new Models.TrnPurchaseOrder
                                 {
                                     Id = d.Id,
                                     Branch = d.MstBranch.Branch,
                                     PONumber = d.PONumber,
                                     PODate = d.PODate.ToShortDateString(),
                                     Supplier = d.MstArticle.Article,
                                     Remarks = d.Remarks,
                                     IsClose = d.IsClose,
                                     Amount = getAmount(d.Id)
                                 };

            return purchaseOrders.ToList();
        }
    }
}
