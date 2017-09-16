using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiInventoryReportController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =====================
        // Inventory Report List
        // =====================
        [Authorize]
        [HttpGet]
        [Route("api/inventoryReport/list/{startDate}/{endDate}/{companyId}/{branchId}")]
        public List<Models.MstArticleInventory> ListInventoryReport(String startDate, String endDate, String companyId, String branchId)
        {
            try
            {
                var unionInventories = (from d in db.TrnInventories
                                        where d.InventoryDate < Convert.ToDateTime(startDate)
                                        && d.MstArticleInventory.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                        && d.MstArticleInventory.BranchId == Convert.ToInt32(branchId)
                                        && d.MstArticleInventory.MstArticle.IsInventory == true
                                        select new Models.MstArticleInventory
                                        {
                                            Id = d.Id,
                                            Document = "Beginning Balance",
                                            BranchId = d.BranchId,
                                            Branch = d.MstBranch.Branch,
                                            ArticleId = d.MstArticleInventory.ArticleId,
                                            Article = d.MstArticleInventory.MstArticle.Article,
                                            InventoryCode = d.MstArticleInventory.InventoryCode,
                                            Quantity = d.MstArticleInventory.Quantity,
                                            Cost = d.MstArticleInventory.Cost,
                                            Amount = d.MstArticleInventory.Amount,
                                            UnitId = d.MstArticleInventory.MstArticle.MstUnit.Id,
                                            Unit = d.MstArticleInventory.MstArticle.MstUnit.Unit,
                                            BegQuantity = d.Quantity,
                                            InQuantity = d.QuantityIn,
                                            OutQuantity = d.QuantityOut,
                                            EndQuantity = d.Quantity
                                        }).Union(from d in db.TrnInventories
                                                 where d.InventoryDate >= Convert.ToDateTime(startDate)
                                                 && d.InventoryDate <= Convert.ToDateTime(endDate)
                                                 && d.MstArticleInventory.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                                 && d.MstArticleInventory.BranchId == Convert.ToInt32(branchId)
                                                 && d.MstArticleInventory.MstArticle.IsInventory == true
                                                 select new Models.MstArticleInventory
                                                 {
                                                     Id = d.Id,
                                                     Document = "Current",
                                                     BranchId = d.BranchId,
                                                     Branch = d.MstBranch.Branch,
                                                     ArticleId = d.MstArticleInventory.ArticleId,
                                                     Article = d.MstArticleInventory.MstArticle.Article,
                                                     InventoryCode = d.MstArticleInventory.InventoryCode,
                                                     Quantity = d.MstArticleInventory.Quantity,
                                                     Cost = d.MstArticleInventory.Cost,
                                                     Amount = d.MstArticleInventory.Amount,
                                                     UnitId = d.MstArticleInventory.MstArticle.MstUnit.Id,
                                                     Unit = d.MstArticleInventory.MstArticle.MstUnit.Unit,
                                                     BegQuantity = d.Quantity,
                                                     InQuantity = d.QuantityIn,
                                                     OutQuantity = d.QuantityOut,
                                                     EndQuantity = d.Quantity
                                                 });

                if (unionInventories.Any())
                {
                    var inventories = from d in unionInventories
                                      group d by new
                                      {
                                          BranchId = d.BranchId,
                                          Branch = d.Branch,
                                          ArticleId = d.ArticleId,
                                          Article = d.Article,
                                          InventoryCode = d.InventoryCode,
                                          Cost = d.Cost,
                                          UnitId = d.UnitId,
                                          Unit = d.Unit
                                      } into g
                                      select new Models.MstArticleInventory
                                      {
                                          BranchId = g.Key.BranchId,
                                          Branch = g.Key.Branch,
                                          ArticleId = g.Key.ArticleId,
                                          Article = g.Key.Article,
                                          InventoryCode = g.Key.InventoryCode,
                                          Cost = g.Key.Cost,
                                          UnitId = g.Key.UnitId,
                                          Unit = g.Key.Unit,
                                          BegQuantity = g.Sum(d => d.Document == "Current" ? 0 : d.BegQuantity),
                                          InQuantity = g.Sum(d => d.Document == "Beginning Balance" ? 0 : d.InQuantity),
                                          OutQuantity = g.Sum(d => d.Document == "Beginning Balance" ? 0 : d.OutQuantity),
                                          EndQuantity = g.Sum(d => d.EndQuantity),
                                          Amount = g.Sum(d => d.Quantity * d.Cost)
                                      };

                    return inventories.ToList();
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
