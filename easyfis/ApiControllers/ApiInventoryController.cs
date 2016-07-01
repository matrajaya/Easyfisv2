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

        // list inventory
        [Authorize]
        [HttpGet]
        [Route("api/listInventory")]
        public List<Models.TrnInventory> listInventory()
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

        // list inventory by RRId
        [Authorize]
        [HttpGet]
        [Route("api/listInventoryByRRId/{RRId}")]
        public List<Models.TrnInventory> listInventoryByRRId(String RRId)
        {
            var inventories = from d in db.TrnInventories
                              where d.RRId == Convert.ToUInt32(RRId)
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

        // list Inventory by SIId
        [Authorize]
        [HttpGet]
        [Route("api/listInventoryBySIId/{SIId}")]
        public List<Models.TrnInventory> listInventoryBySIId(String SIId)
        {
            var inventories = from d in db.TrnInventories
                              where d.SIId == Convert.ToUInt32(SIId)
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

        // list Inventory by INId
        [Authorize]
        [HttpGet]
        [Route("api/listInventoryByINId/{INId}")]
        public List<Models.TrnInventory> listInventoryByINId(String INId)
        {
            var inventories = from d in db.TrnInventories
                              where d.INId == Convert.ToUInt32(INId)
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

        // list Inventory by OTId
        [Authorize]
        [HttpGet]
        [Route("api/listInventoryByOTId/{OTId}")]
        public List<Models.TrnInventory> listInventoryByOTId(String OTId)
        {
            var inventories = from d in db.TrnInventories
                              where d.OTId == Convert.ToUInt32(OTId)
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

        // list Inventory by STId
        [Authorize]
        [HttpGet]
        [Route("api/listInventoryBySTId/{STId}")]
        public List<Models.TrnInventory> listInventoryBySTId(String STId)
        {
            var inventories = from d in db.TrnInventories
                              where d.STId == Convert.ToUInt32(STId)
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
