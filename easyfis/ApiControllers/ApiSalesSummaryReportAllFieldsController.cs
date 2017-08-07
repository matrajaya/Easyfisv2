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

        [Authorize, HttpGet, Route("api/salesSummaryReportAllFields/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.TrnSalesInvoice> listSalesSummaryReport(String startDate, String endDate, String companyId, String branchId)
        {
            var salesInvoices = from d in db.TrnSalesInvoiceItems
                                where d.TrnSalesInvoice.BranchId == Convert.ToInt32(branchId)
                                && d.TrnSalesInvoice.MstBranch.CompanyId == Convert.ToInt32(companyId)
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
