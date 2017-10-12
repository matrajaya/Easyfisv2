using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiStockCardController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ======================
        // Stock Card Report List
        // ======================
        [Authorize, HttpGet, Route("api/stockCard/list/{startDate}/{endDate}/{companyId}/{branchId}/{itemId}")]
        public List<Models.TrnInventory> ListStockCard(String startDate, String endDate, String companyId, String branchId, String itemId)
        {
            var unionInventories = (from d in db.TrnInventories
                                    where d.InventoryDate < Convert.ToDateTime(startDate)
                                    && d.MstArticleInventory.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                    && d.MstArticleInventory.BranchId == Convert.ToInt32(branchId)
                                    && d.MstArticleInventory.MstArticle.IsInventory == true 
                                    && d.ArticleId == Convert.ToInt32(itemId)
                                    select new
                                    {
                                        Id = d.Id,
                                        Document = "Beginning Balance",
                                        InventoryDate = d.InventoryDate,
                                        BranchId = d.BranchId,
                                        Branch = d.MstBranch.Branch,
                                        BranchCode = d.MstBranch.BranchCode,
                                        ArticleId = d.MstArticleInventory.ArticleId,
                                        Article = d.MstArticleInventory.MstArticle.Article,
                                        InventoryCode = d.MstArticleInventory.InventoryCode,
                                        Quantity = d.MstArticleInventory.Quantity,
                                        BegQuantity = d.Quantity,
                                        InQuantity = d.QuantityIn,
                                        OutQuantity = d.QuantityOut,
                                        EndQuantity = d.Quantity,
                                        UnitId = d.MstArticleInventory.MstArticle.MstUnit.Id,
                                        Unit = d.MstArticleInventory.MstArticle.MstUnit.Unit,
                                        Cost = d.MstArticleInventory.Cost,
                                        Amount = d.MstArticleInventory.Amount,
                                        RRId = d.RRId,
                                        RRNumber = d.TrnReceivingReceipt.RRNumber,
                                        SIId = d.SIId,
                                        SINumber = d.TrnSalesInvoice.SINumber,
                                        INId = d.INId,
                                        INNumber = d.TrnStockIn.INNumber,
                                        OTId = d.OTId,
                                        OTNumber = d.TrnStockOut.OTNumber,
                                        STId = d.STId,
                                        STNumber = d.TrnStockTransfer.STNumber,
                                    }).Union(from d in db.TrnInventories
                                             where d.InventoryDate >= Convert.ToDateTime(startDate)
                                             && d.InventoryDate <= Convert.ToDateTime(endDate)
                                             && d.MstArticleInventory.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                             && d.MstArticleInventory.BranchId == Convert.ToInt32(branchId)
                                             && d.MstArticleInventory.MstArticle.IsInventory == true
                                             && d.ArticleId == Convert.ToInt32(itemId)
                                             select new
                                             {
                                                 Id = d.Id,
                                                 Document = "Current",
                                                 InventoryDate = d.InventoryDate,
                                                 BranchId = d.BranchId,
                                                 Branch = d.MstBranch.Branch,
                                                 BranchCode = d.MstBranch.BranchCode,
                                                 ArticleId = d.MstArticleInventory.ArticleId,
                                                 Article = d.MstArticleInventory.MstArticle.Article,
                                                 InventoryCode = d.MstArticleInventory.InventoryCode,
                                                 Quantity = d.MstArticleInventory.Quantity,
                                                 BegQuantity = d.Quantity,
                                                 InQuantity = d.QuantityIn,
                                                 OutQuantity = d.QuantityOut,
                                                 EndQuantity = d.Quantity,
                                                 UnitId = d.MstArticleInventory.MstArticle.MstUnit.Id,
                                                 Unit = d.MstArticleInventory.MstArticle.MstUnit.Unit,
                                                 Cost = d.MstArticleInventory.Cost,
                                                 Amount = d.MstArticleInventory.Amount,
                                                 RRId = d.RRId,
                                                 RRNumber = d.TrnReceivingReceipt.RRNumber,
                                                 SIId = d.SIId,
                                                 SINumber = d.TrnSalesInvoice.SINumber,
                                                 INId = d.INId,
                                                 INNumber = d.TrnStockIn.INNumber,
                                                 OTId = d.OTId,
                                                 OTNumber = d.TrnStockOut.OTNumber,
                                                 STId = d.STId,
                                                 STNumber = d.TrnStockTransfer.STNumber,
                                             });

            if (unionInventories.Any())
            {
                var inventories = from d in unionInventories
                                  group d by new
                                  {
                                      InventoryDate = d.InventoryDate,
                                      BranchId = d.BranchId,
                                      Branch = d.Branch,
                                      BranchCode = d.BranchCode,
                                      ArticleId = d.ArticleId,
                                      Article = d.Article,
                                      InventoryCode = d.InventoryCode,
                                      Cost = d.Cost,
                                      UnitId = d.UnitId,
                                      Unit = d.Unit,
                                      RRId = d.RRId,
                                      RRNumber = d.RRNumber,
                                      SIId = d.SIId,
                                      SINumber = d.SINumber,
                                      INId = d.INId,
                                      INNumber = d.INNumber,
                                      OTId = d.OTId,
                                      OTNumber = d.OTNumber,
                                      STId = d.STId,
                                      STNumber = d.STNumber,
                                  } into g
                                  select new Models.TrnInventory
                                  {
                                      InventoryDate = g.Key.InventoryDate.ToShortDateString(),
                                      BranchId = g.Key.BranchId,
                                      Branch = g.Key.Branch,
                                      BranchCode = g.Key.BranchCode,
                                      ArticleId = g.Key.ArticleId,
                                      Article = g.Key.Article,
                                      ArticleInventoryCode = g.Key.InventoryCode,
                                      Cost = g.Key.Cost,
                                      UnitId = g.Key.UnitId,
                                      Unit = g.Key.Unit,
                                      BegQuantity = g.Sum(d => d.Document == "Current" ? 0 : d.BegQuantity),
                                      InQuantity = g.Sum(d => d.Document == "Beginning Balance" ? 0 : d.InQuantity),
                                      OutQuantity = g.Sum(d => d.Document == "Beginning Balance" ? 0 : d.OutQuantity),
                                      EndQuantity = g.Sum(d => d.EndQuantity),
                                      Amount = g.Sum(d => d.Quantity * d.Cost),
                                      RRId = g.Key.RRId,
                                      RRNumber = g.Key.RRNumber,
                                      SIId = g.Key.SIId,
                                      SINumber = g.Key.SINumber,
                                      INId = g.Key.INId,
                                      INNumber = g.Key.INNumber,
                                      OTId = g.Key.OTId,
                                      OTNumber = g.Key.OTNumber,
                                      STId = g.Key.STId,
                                      STNumber = g.Key.STNumber,
                                  };

                return inventories.ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
