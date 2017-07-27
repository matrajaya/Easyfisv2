using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.ApiControllers
{
    public class ApiSalesSummaryReportAllFieldsController : ApiController
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
        [Route("api/salesSummaryReportAllFields/list/{startDate}/{endDate}")]
        public List<Models.TrnSalesInvoice> listSalesSummaryReport(String startDate, String endDate)
        {

            // purchase orders
            var salesInvoices = from d in db.TrnSalesInvoiceItems
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
                                    Term = d.TrnSalesInvoice.MstTerm.Term,
                                    DocumentReference = d.TrnSalesInvoice.DocumentReference,
                                    ManualSINumber = d.TrnSalesInvoice.ManualSINumber,
                                    Remarks = d.TrnSalesInvoice.Remarks,
                                    SoldBy = d.TrnSalesInvoice.MstUser4.FullName,
                                    Amount = d.TrnSalesInvoice.Amount,
                                    PaidAmount = d.TrnSalesInvoice.PaidAmount,
                                    AdjustmentAmount = d.TrnSalesInvoice.AdjustmentAmount,
                                    BalanceAmount = d.TrnSalesInvoice.BalanceAmount,
                                    PreparedBy = d.TrnSalesInvoice.MstUser3.FullName,
                                    CheckedBy = d.TrnSalesInvoice.MstUser1.FullName,
                                    ApprovedBy = d.TrnSalesInvoice.MstUser.FullName,
                                    IsLocked = d.TrnSalesInvoice.IsLocked,
                                    CreatedBy = d.TrnSalesInvoice.MstUser2.FullName,
                                    CreatedDateTime = d.TrnSalesInvoice.CreatedDateTime.ToShortDateString(),
                                    UpdatedBy = d.TrnSalesInvoice.MstUser5.FullName,
                                    UpdatedDateTime = d.TrnSalesInvoice.UpdatedDateTime.ToShortDateString(),
                                };

            return salesInvoices.ToList();
        }
    }
}
