using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiPurchaseDetailReportController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =================================
        // Purchase Order Detail Report List
        // =================================
        [Authorize]
        [HttpGet]
        [Route("api/purchaseDetailReport/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.TrnPurchaseOrderItem> ListPurchaseDetailReports(String startDate, String endDate, String companyId, String branchId)
        {
            var purchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                     where d.TrnPurchaseOrder.PODate >= Convert.ToDateTime(startDate)
                                     && d.TrnPurchaseOrder.PODate <= Convert.ToDateTime(endDate)
                                     && d.TrnPurchaseOrder.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                     && d.TrnPurchaseOrder.BranchId == Convert.ToInt32(branchId)
                                     && d.TrnPurchaseOrder.IsLocked == true
                                     select new Models.TrnPurchaseOrderItem
                                     {
                                         Id = d.Id,
                                         POId = d.POId,
                                         Branch = d.TrnPurchaseOrder.MstBranch.Branch,
                                         PO = d.TrnPurchaseOrder.PONumber,
                                         PODate = d.TrnPurchaseOrder.PODate.ToShortDateString(),
                                         Supplier = d.TrnPurchaseOrder.MstArticle.Article,
                                         Item = d.MstArticle.Article,
                                         Price = d.MstArticle.Price,
                                         Unit = d.MstUnit.Unit,
                                         Quantity = d.Quantity,
                                         Amount = d.Amount
                                     };

            return purchaseOrderItems.ToList();
        }
    }
}
