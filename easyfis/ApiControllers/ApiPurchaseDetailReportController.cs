using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiPurchaseDetailReportController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();
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
        [Route("api/purchaseDetailReport/list/{startDate}/{endDate}")]
        public List<Models.TrnPurchaseOrderItem> listPurchaseDetailReports(String startDate, String endDate)
        {
            // purchase orders
            var PurchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                     where d.TrnPurchaseOrder.BranchId == currentBranchId()
                                     && d.TrnPurchaseOrder.PODate >= Convert.ToDateTime(startDate)
                                     && d.TrnPurchaseOrder.PODate <= Convert.ToDateTime(endDate)
                                     && d.TrnPurchaseOrder.IsLocked == true
                                     select new Models.TrnPurchaseOrderItem
                                     {
                                         Id = d.Id,
                                         PODate = d.TrnPurchaseOrder.PODate.ToShortDateString(),
                                         PO = d.TrnPurchaseOrder.PONumber,
                                         Item = d.MstArticle.Article,
                                         Price = d.MstArticle.Price,
                                         Unit = d.MstUnit.Unit,
                                         Quantity = d.Quantity,
                                         Amount = d.Amount
                                     };

            Decimal total = 0;

            return PurchaseOrderItems.ToList();
        }
    }
}
