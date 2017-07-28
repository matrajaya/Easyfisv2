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

        [Authorize]
        [HttpGet]
        [Route("api/salesSummaryReportAllFields/list/{startDate}/{endDate}")]
        public List<Models.TrnSalesInvoice> listSalesSummaryReport(String startDate, String endDate)
        {
            var salesInvoices = from d in db.TrnSalesInvoiceItems
                                where d.TrnSalesInvoice.BranchId == currentBranchId()
                                && d.TrnSalesInvoice.SIDate >= Convert.ToDateTime(startDate)
                                && d.TrnSalesInvoice.SIDate <= Convert.ToDateTime(endDate)
                                && d.TrnSalesInvoice.IsLocked == true
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
                                    Item = d.MstArticle.Article,
                                    ItemInventory = d.MstArticleInventory.InventoryCode,
                                    Unit = d.MstUnit.Unit,
                                    Quantity = d.Quantity,
                                    SalesInvoiceItemAmount = d.Amount,
                                    Price = d.Price,
                                };

            return salesInvoices.ToList();
        }
    }
}
