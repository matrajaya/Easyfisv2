using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.ApiControllers
{
    public class ApiChartMonthlySalesTrendController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        [Authorize, HttpGet, Route("api/chartMonthlySalesTrend/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.TrnSalesInvoiceItem> listChartMonthlySalesTrend(String startDate, String endDate, String companyId, String branchId)
        {
            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                    where d.TrnSalesInvoice.IsLocked == true
                                    && d.TrnSalesInvoice.BranchId == Convert.ToInt32(branchId)
                                    && d.TrnSalesInvoice.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                    select new Models.TrnSalesInvoiceItem
                                    {
                                        SalesItemTimeStampDateTime = new DateTime(d.SalesItemTimeStamp.Year, d.SalesItemTimeStamp.Month, d.SalesItemTimeStamp.Day),
                                        SalesItemTimeStamp = (d.SalesItemTimeStamp.Month + "/" + d.SalesItemTimeStamp.Year).ToString(),
                                        Amount = d.Amount
                                    };

            var salesInvoiceItemsLinQ = from d in salesInvoiceItems.ToList()
                                        where d.SalesItemTimeStampDateTime >= Convert.ToDateTime(startDate)
                                        && d.SalesItemTimeStampDateTime <= Convert.ToDateTime(endDate)
                                        group d by new
                                        {
                                            SalesItemTimeStamp = d.SalesItemTimeStamp
                                        }
                                        into g
                                        select new Models.TrnSalesInvoiceItem
                                        {
                                            SalesItemTimeStamp = g.Key.SalesItemTimeStamp,
                                            Amount = g.Sum(d => d.Amount)
                                        };

            return salesInvoiceItemsLinQ.ToList();
        }
    }
}
