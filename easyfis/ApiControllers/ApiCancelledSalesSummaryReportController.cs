using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.ApiControllers
{
    public class ApiCancelledSalesSummaryReportController : ApiController
    {
                // list account
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
        [Route("api/cancelledSalesSummaryReport/list/{startDate}/{endDate}")]
        public List<Models.TrnSalesInvoice> listSalesSummaryReport(String startDate, String endDate)
        {

            // purchase orders
            var salesInvoices = from d in db.TrnSalesInvoices
                                where d.BranchId == currentBranchId()
                                && d.SIDate >= Convert.ToDateTime(startDate)
                                && d.SIDate <= Convert.ToDateTime(endDate)
                                && d.IsLocked != true 
                                select new Models.TrnSalesInvoice
                                {
                                    Id = d.Id,
                                    Branch = d.MstBranch.Branch,
                                    SINumber = d.SINumber,
                                    SIDate = d.SIDate.ToShortDateString(),
                                    Customer = d.MstArticle.Article,
                                    Remarks = d.Remarks,
                                    SoldBy = d.MstUser4.FullName,
                                    Amount = d.Amount
                                };

            return salesInvoices.ToList();
        }
    }
}
