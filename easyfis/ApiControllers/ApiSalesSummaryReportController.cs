using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Web.Http;
using System.Globalization;

namespace easyfis.ApiControllers
{
    public class ApiSalesSummaryReportController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =============================
        // Get Max Sales Item Time Stamp
        // =============================
        public String GetSalesItemMaxTimeStamp(Int32 SIId)
        {
            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                    where d.SIId == SIId
                                    select d;

            if (salesInvoiceItems.Any())
            {
                return salesInvoiceItems.Max(t => t.SalesItemTimeStamp).ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
            }
            else
            {
                return " ";
            }
        }

        // =========================
        // Sales Summary Report List
        // =========================
        [Authorize, HttpGet, Route("api/salesSummaryReport/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.TrnSalesInvoice> listSalesSummaryReport(String startDate, String endDate, String companyId, String branchId)
        {
            var salesInvoices = from d in db.TrnSalesInvoices
                                where d.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                && d.BranchId == Convert.ToInt32(branchId)
                                && d.SIDate >= Convert.ToDateTime(startDate)
                                && d.SIDate <= Convert.ToDateTime(endDate)
                                && d.IsLocked == true
                                select new Models.TrnSalesInvoice
                                {
                                    Id = d.Id,
                                    Branch = d.MstBranch.Branch,
                                    SINumber = d.SINumber,
                                    SIDate = d.SIDate.ToShortDateString(),
                                    Customer = d.MstArticle.Article,
                                    Remarks = d.Remarks,
                                    SoldBy = d.MstUser4.FullName,
                                    Amount = d.Amount,
                                    SalesTimeStamp = GetSalesItemMaxTimeStamp(d.Id)
                                };

            return salesInvoices.ToList();
        }
    }
}
