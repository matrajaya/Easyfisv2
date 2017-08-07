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

        [Authorize, HttpGet, Route("api/topSellingItemsReport/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.TrnSalesInvoiceItem> listTopSellingItemsReport(String startDate, String endDate, String companyId, String branchId)
        {
            var salesInvoiceItem = from d in db.TrnSalesInvoiceItems.OrderByDescending(q => q.BaseQuantity)
                                   where d.TrnSalesInvoice.BranchId == Convert.ToInt32(branchId)
                                   && d.TrnSalesInvoice.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                   && d.TrnSalesInvoice.SIDate >= Convert.ToDateTime(startDate)
                                   && d.TrnSalesInvoice.SIDate <= Convert.ToDateTime(endDate)
                                   && d.TrnSalesInvoice.IsLocked == true
                                   group d by new
                                   {
                                       Item = d.MstArticle.Article,
                                       BaseUnit = d.MstUnit1.Unit
                                   } into g
                                   select new Models.TrnSalesInvoiceItem
                                   {
                                       Item = g.Key.Item,
                                       BaseUnit = g.Key.BaseUnit,
                                       BaseQuantity = g.Sum(d => d.BaseQuantity),
                                       Amount = g.Sum(d => d.Amount)
                                   };

            return salesInvoiceItem.ToList();
        }
    }
}
