using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiStockInDetailReportController : ApiController
    {
        // ============
        // Data Context 
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===========================
        // Stock In Detail Report List
        // ===========================
        [Authorize, HttpGet, Route("api/stockInDetailReport/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.TrnStockInItem> ListStockInDetailReport(String startDate, String endDate, String companyId, String branchId)
        {
            var stockInItems = from d in db.TrnStockInItems
                               where d.TrnStockIn.MstBranch.CompanyId == Convert.ToInt32(companyId)
                               && d.TrnStockIn.BranchId == Convert.ToInt32(branchId)
                               && d.TrnStockIn.INDate >= Convert.ToDateTime(startDate)
                               && d.TrnStockIn.INDate <= Convert.ToDateTime(endDate)
                               && d.TrnStockIn.IsLocked == true
                               select new Models.TrnStockInItem
                               {
                                   Id = d.Id,
                                   INId = d.INId,
                                   IN = d.TrnStockIn.INNumber,
                                   INDate = d.TrnStockIn.INDate.ToShortDateString(),
                                   ItemId = d.ItemId,
                                   ItemCode = d.MstArticle.ManualArticleCode,
                                   Item = d.MstArticle.Article,
                                   Particulars = d.Particulars,
                                   UnitId = d.UnitId,
                                   Unit = d.MstUnit.Unit,
                                   Quantity = d.Quantity,
                                   Cost = d.Cost,
                                   Amount = d.Amount,
                                   BaseUnitId = d.BaseUnitId,
                                   BaseUnit = d.MstUnit1.Unit,
                                   BaseQuantity = d.BaseQuantity,
                                   BaseCost = d.BaseCost
                               };
            return stockInItems.ToList();
        }
    }
}
