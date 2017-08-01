using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.ApiControllers
{
    public class ApiHourlyTopSellingReportController : ApiController
    {
        // data
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // list hourly top items selling report
        [Authorize]
        [HttpGet]
        [Route("api/hourlyTopItemsSellingReport/list/{startDate}/{endDate}")]
        public List<Models.TrnSalesInvoiceItem> listHourlyTopItemsSellingReport(String startDate, String endDate)
        {
            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems.OrderByDescending(d => d.Quantity)
                                    where d.TrnSalesInvoice.BranchId == currentBranchId()
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
                                        Amount = g.Sum(d => d.Amount),
                                        SalesItemTimeStampHour = g.Average(d => Convert.ToInt64(Convert.ToDateTime(d.SalesItemTimeStamp).Hour)).ToString(),
                                        SalesItemTimeStampMinutes = g.Average(d => Convert.ToInt64(Convert.ToDateTime(d.SalesItemTimeStamp).Minute)).ToString(),
                                        SalesItemTimeStampSeconds = g.Average(d => Convert.ToInt64(Convert.ToDateTime(d.SalesItemTimeStamp).Second)).ToString(),
                                        SalesItemTimeStamp = String.Format("{0:T}", Convert.ToDateTime(g.Average(d => Convert.ToInt64(Convert.ToDateTime(d.SalesItemTimeStamp).Hour)) + ":" + g.Average(d => Convert.ToInt64(Convert.ToDateTime(d.SalesItemTimeStamp).Minute)) + ":" + g.Average(d => Convert.ToInt64(Convert.ToDateTime(d.SalesItemTimeStamp).Second))))
                                    };

            return salesInvoiceItems.ToList();
        }
    }
}
