using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.ApiControllers
{
    public class ApiTopSellingItemsReportController : ApiController
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
        [Route("api/topSellingItemsReport/list/{startDate}/{endDate}")]
        public List<Models.TrnSalesInvoice> listSalesSummaryReport(String startDate, String endDate)
        {

            // purchase orders
            var salesInvoices = from d in db.TrnSalesInvoiceItems.OrderByDescending(q => q.Quantity)
                                where d.TrnSalesInvoice.BranchId == currentBranchId()
                                && d.TrnSalesInvoice.SIDate >= Convert.ToDateTime(startDate)
                                && d.TrnSalesInvoice.SIDate <= Convert.ToDateTime(endDate)
                                select new Models.TrnSalesInvoice
                                {
                                    Id = d.Id,
                                    Branch = d.TrnSalesInvoice.MstBranch.Branch,
                                    SINumber = d.TrnSalesInvoice.SINumber,
                                    SIDate = d.TrnSalesInvoice.SIDate.ToShortDateString(),
                                    Customer = d.TrnSalesInvoice.MstArticle.Article,
                                    Remarks = d.TrnSalesInvoice.Remarks,
                                    SoldBy = d.TrnSalesInvoice.MstUser4.FullName,
                                    Amount = d.TrnSalesInvoice.Amount
                                };

            return salesInvoices.ToList();
        }
    }
}
