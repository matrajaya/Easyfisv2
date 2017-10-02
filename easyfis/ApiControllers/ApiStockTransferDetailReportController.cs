using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiStockTransferDetailReportController : ApiController
    {
        // ============
        // Data Context 
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =================================
        // Stock Transfer Detail Report List
        // =================================
        [Authorize, HttpGet, Route("api/stockTransferDetailReport/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.TrnStockTransferItem> ListStockTransferDetailReport(String startDate, String endDate, String companyId, String branchId)
        {
            var stockTransferItems = from d in db.TrnStockTransferItems
                                     where d.TrnStockTransfer.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                     && d.TrnStockTransfer.BranchId == Convert.ToInt32(branchId)
                                     && d.TrnStockTransfer.STDate >= Convert.ToDateTime(startDate)
                                     && d.TrnStockTransfer.STDate <= Convert.ToDateTime(endDate)
                                     && d.TrnStockTransfer.IsLocked == true
                                     select new Models.TrnStockTransferItem
                                     {
                                         Id = d.Id,
                                         STId = d.STId,
                                         ST = d.TrnStockTransfer.STNumber,
                                         STDate = d.TrnStockTransfer.STDate.ToShortDateString(),
                                         ToBranch = d.TrnStockTransfer.MstBranch1.Branch,
                                         ItemId = d.ItemId,
                                         ItemCode = d.MstArticle.ManualArticleCode,
                                         Item = d.MstArticle.Article,
                                         ItemInventoryId = d.ItemInventoryId,
                                         ItemInventory = d.MstArticleInventory.InventoryCode,
                                         Particulars = d.Particulars,
                                         UnitId = d.UnitId,
                                         Unit = d.MstUnit.Unit,
                                         Quantity = d.Quantity,
                                         Cost = d.Cost,
                                         Amount = d.Amount,
                                         BaseUnitId = d.BaseUnitId,
                                         BaseUnit = d.MstUnit1.Unit,
                                         BaseQuantity = d.BaseQuantity,
                                         BaseCost = d.BaseCost,
                                     };

            return stockTransferItems.ToList();
        }
    }
}
