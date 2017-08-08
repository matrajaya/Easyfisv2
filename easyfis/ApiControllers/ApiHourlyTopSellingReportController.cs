using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Globalization;

namespace easyfis.ApiControllers
{
    public class ApiHourlyTopSellingReportController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        public String timeStampFormat(String salesInvoiceItemTimeStamp)
        {
            CultureInfo cultureESUS = CultureInfo.CreateSpecificCulture("en-US");
            if (!salesInvoiceItemTimeStamp.Equals(""))
            {
                DateTime dateToDisplay = new DateTime(0001, 1, 1, Convert.ToInt32(salesInvoiceItemTimeStamp), 0, 0);
                return dateToDisplay.ToString("t", cultureESUS);
            }
            else
            {
                return "";
            }
        }

        [Authorize, HttpGet, Route("api/hourlyTopItemsSellingReport/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.TrnSalesInvoiceItem> listHourlyTopItemsSellingReport(String startDate, String endDate, String companyId, String branchId)
        {
            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems.OrderByDescending(d => d.Quantity)
                                    where d.TrnSalesInvoice.BranchId == Convert.ToInt32(branchId)
                                    && d.TrnSalesInvoice.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                    && d.TrnSalesInvoice.SIDate >= Convert.ToDateTime(startDate)
                                    && d.TrnSalesInvoice.SIDate <= Convert.ToDateTime(endDate)
                                    && d.TrnSalesInvoice.IsLocked == true
                                    group d by new
                                    {
                                        Item = d.MstArticle.Article,
                                        BasePrice = d.MstArticle.Price,
                                        BaseUnit = d.MstArticle.MstUnit.Unit,
                                        SalesItemTimeStamp = d.SalesItemTimeStamp.Hour,
                                    } into g
                                    select new Models.TrnSalesInvoiceItem
                                    {
                                        Item = g.Key.Item,
                                        BaseUnit = g.Key.BaseUnit,
                                        BaseQuantity = g.Sum(d => d.BaseQuantity),
                                        BasePrice = g.Key.BasePrice,
                                        SalesItemTimeStamp = timeStampFormat(g.Key.SalesItemTimeStamp.ToString())
                                    };

            return salesInvoiceItems.ToList();
        }
    }
}
