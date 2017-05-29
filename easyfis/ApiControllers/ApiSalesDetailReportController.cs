using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiSalesDetailReportController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();
        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

       

        // list account
        [Authorize]
        [HttpGet]
        [Route("api/salesDetailReport/list/{startDate}/{endDate}")]
        public List<Models.TrnSalesInvoiceItem> listSalesDetailReport(String startDate, String endDate)
        {
            // purchase orders
            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                    where d.TrnSalesInvoice.BranchId == currentBranchId()
                                    && d.TrnSalesInvoice.SIDate >= Convert.ToDateTime(startDate)
                                    && d.TrnSalesInvoice.SIDate <= Convert.ToDateTime(endDate)
                                    && d.TrnSalesInvoice.IsLocked == true
                                    select new Models.TrnSalesInvoiceItem
                                    {
                                        SIId = d.SIId,
                                        Id = d.Id,
                                        SI = d.TrnSalesInvoice.SINumber,
                                        SIDate = d.TrnSalesInvoice.SIDate.ToShortDateString(),
                                        Item = d.MstArticle.Article,
                                        ItemInventory = d.MstArticleInventory.InventoryCode,
                                        Unit = d.MstUnit.Unit,
                                        Quantity = d.Quantity,
                                        Amount = d.Amount,
                                        Price = d.Price,
                                        Customer = d.TrnSalesInvoice.MstArticle.Article
                                    };

            return salesInvoiceItems.ToList();
        }
    }
}
