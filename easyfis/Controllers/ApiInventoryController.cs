using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiInventoryController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==============
        // LIST Inventory
        // ==============
        [Route("api/listInventory")]
        public List<Models.TrnInventory> Get()
        {
            var inventories = from d in db.TrnInventories
                        select new Models.TrnInventory
                        {
                            Id = d.Id,
                            BranchId = d.BranchId,
                            Branch = d.MstBranch.Branch,
                            InventoryDate = d.InventoryDate.ToShortDateString(),
                            ArticleId = d.ArticleId,
                            ArticleInventoryId = d.ArticleInventoryId,
                            RRId = d.RRId,
                            SIId = d.SIId,
                            INId = d.INId,
                            OTId = d.OTId,
                            STId = d.STId,
                            QuantityIn = d.QuantityIn,
                            Quantity = d.Quantity,
                            QuantityOut = d.QuantityOut,
                            Amount = d.Amount,
                            Particulars = d.Particulars
                        };
            return inventories.ToList();
        }

        // ======================
        // LIST Inventory by RRId
        // ======================
        [Route("api/listInventoryByRRId/{RRId}")]
        public List<Models.TrnInventory> GetInventoryByRRId(String RRId)
        {
            var inventories_RRId = Convert.ToUInt32(RRId);
            var inventories = from d in db.TrnInventories
                              where d.RRId == inventories_RRId
                              select new Models.TrnInventory
                              {
                                  Id = d.Id,
                                  BranchId = d.BranchId,
                                  Branch = d.MstBranch.Branch,
                                  InventoryDate = d.InventoryDate.ToShortDateString(),
                                  ArticleId = d.ArticleId,
                                  Article = d.MstArticle.Article,
                                  ArticleInventoryId = d.ArticleInventoryId,
                                  RRId = d.RRId,
                                  SIId = d.SIId,
                                  INId = d.INId,
                                  OTId = d.OTId,
                                  STId = d.STId,
                                  QuantityIn = d.QuantityIn,
                                  Quantity = d.Quantity,
                                  QuantityOut = d.QuantityOut,
                                  Amount = d.Amount,
                                  Particulars = d.Particulars,
                                  Code = d.MstArticleInventory.InventoryCode,
                                  Unit = d.MstArticle.MstUnit.Unit
                              };
            return inventories.ToList();
        }

        // ======================
        // LIST Inventory by SIId
        // ======================
        [Route("api/listInventoryBySIId/{SIId}")]
        public List<Models.TrnInventory> GetInventoryBySIId(String SIId)
        {
            var inventories_SIId = Convert.ToUInt32(SIId);
            var inventories = from d in db.TrnInventories
                              where d.SIId == inventories_SIId
                              select new Models.TrnInventory
                              {
                                  Id = d.Id,
                                  BranchId = d.BranchId,
                                  Branch = d.MstBranch.Branch,
                                  InventoryDate = d.InventoryDate.ToShortDateString(),
                                  ArticleId = d.ArticleId,
                                  Article = d.MstArticle.Article,
                                  ArticleInventoryId = d.ArticleInventoryId,
                                  RRId = d.RRId,
                                  SIId = d.SIId,
                                  INId = d.INId,
                                  OTId = d.OTId,
                                  STId = d.STId,
                                  QuantityIn = d.QuantityIn,
                                  Quantity = d.Quantity,
                                  QuantityOut = d.QuantityOut,
                                  Amount = d.Amount,
                                  Particulars = d.Particulars,
                                  Code = d.MstArticleInventory.InventoryCode,
                                  Unit = d.MstArticle.MstUnit.Unit
                              };
            return inventories.ToList();
        }

        // ======================
        // LIST Inventory by INId
        // ======================
        [Route("api/listInventoryByINId/{INId}")]
        public List<Models.TrnInventory> GetInventoryByINId(String INId)
        {
            var inventories_INId = Convert.ToUInt32(INId);
            var inventories = from d in db.TrnInventories
                              where d.INId == inventories_INId
                              select new Models.TrnInventory
                              {
                                  Id = d.Id,
                                  BranchId = d.BranchId,
                                  Branch = d.MstBranch.Branch,
                                  InventoryDate = d.InventoryDate.ToShortDateString(),
                                  ArticleId = d.ArticleId,
                                  Article = d.MstArticle.Article,
                                  ArticleInventoryId = d.ArticleInventoryId,
                                  RRId = d.RRId,
                                  SIId = d.SIId,
                                  INId = d.INId,
                                  OTId = d.OTId,
                                  STId = d.STId,
                                  QuantityIn = d.QuantityIn,
                                  Quantity = d.Quantity,
                                  QuantityOut = d.QuantityOut,
                                  Amount = d.Amount,
                                  Particulars = d.Particulars,
                                  Code = d.MstArticleInventory.InventoryCode,
                                  Unit = d.MstArticle.MstUnit.Unit
                              };
            return inventories.ToList();
        }
    }
}
