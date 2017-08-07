using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.ApiControllers
{
    public class ApiSalesSummaryReportSalesNoController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        [Authorize, HttpGet, Route("api/salesSummaryReportSalesNo/list/{startSalesNo}/{endSalesNo}/{companyId}/{branchId}")]
        public List<Models.TrnSalesInvoice> listSalesSummaryReport(String startSalesNo, String endSalesNo, String companyId, String branchId)
        {
            var salesInvoices = from d in db.TrnSalesInvoices
                                where d.BranchId == Convert.ToInt32(branchId)
                                && d.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                && Convert.ToInt32(d.SINumber) >= Convert.ToInt32(startSalesNo)
                                && Convert.ToInt32(d.SINumber) <= Convert.ToInt32(endSalesNo)
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
                                    Amount = d.Amount
                                };

            return salesInvoices.ToList();
        }
    }
}
