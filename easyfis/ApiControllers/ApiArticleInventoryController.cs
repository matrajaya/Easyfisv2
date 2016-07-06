using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiArticleInventoryController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // list article inventory
        [Authorize]
        [HttpGet]
        [Route("api/listArticleInventory")]
        public List<Models.MstArticleInventory> listArticleInventory()
        {
            var articleInventories = from d in db.MstArticleInventories
                                     select new Models.MstArticleInventory
                                     {
                                         Id = d.Id,
                                         BranchId = d.BranchId,
                                         ArticleId = d.ArticleId,
                                         InventoryCode = d.InventoryCode,
                                         Quantity = d.Quantity,
                                         Cost = d.Cost,
                                         Amount = d.Amount,
                                         Particulars = d.Particulars
                                     };

            return articleInventories.ToList();
        }

        // list article inventory by ArticleId
        [Authorize]
        [HttpGet]
        [Route("api/listArticleInventoryByArticleId/{articleId}")]
        public List<Models.MstArticleInventory> listArticleInventoryByArticleId(String articleId)
        {
            var articleInventories = from d in db.MstArticleInventories
                                     where d.ArticleId == Convert.ToInt32(articleId)
                                     select new Models.MstArticleInventory
                                     {
                                         Id = d.Id,
                                         BranchId = d.BranchId,
                                         Branch = d.MstBranch.Branch,
                                         ArticleId = d.ArticleId,
                                         Article = d.MstArticle.Article,
                                         InventoryCode = d.InventoryCode,
                                         Quantity = d.Quantity,
                                         Cost = d.Cost,
                                         Amount = d.Amount,
                                         Particulars = d.Particulars
                                     };

            return articleInventories.ToList();
        }

        // list article inventory by BranchId and ArticleId
        [Authorize]
        [HttpGet]
        [Route("api/listArticleInventoryByBranchIdAndArticleId/{branchId}/{articleId}")]
        public List<Models.MstArticleInventory> listArticleInventoryByBranchIdAndArticleId(String branchId, String articleId)
        {
            var articleInventories = from d in db.MstArticleInventories
                                     where d.BranchId == Convert.ToInt32(branchId)
                                     && d.ArticleId == Convert.ToInt32(articleId) 
                                     && d.Quantity > 0
                                     select new Models.MstArticleInventory
                                     {
                                         Id = d.Id,
                                         BranchId = d.BranchId,
                                         ArticleId = d.ArticleId,
                                         InventoryCode = d.InventoryCode,
                                         Quantity = d.Quantity,
                                         Cost = d.Cost,
                                         Amount = d.Amount,
                                         Particulars = d.Particulars
                                     };

            return articleInventories.ToList();
        }

        // get article inventory by ArticleId
        [Authorize]
        [HttpGet]
        [Route("api/getArticleInventoryByArticleId/{branchId}/{articleId}")]
        public Models.MstArticleInventory getArticleInventoryByArticleId(String branchId, String articleId)
        {
            var articleInventories = from d in db.MstArticleInventories
                                     where d.BranchId == Convert.ToInt32(branchId)
                                     && d.ArticleId == Convert.ToInt32(articleId) 
                                     && d.Quantity > 0
                                     select new Models.MstArticleInventory
                                     {
                                         Id = d.Id,
                                         BranchId = d.BranchId,
                                         ArticleId = d.ArticleId,
                                         InventoryCode = d.InventoryCode,
                                         Quantity = d.Quantity,
                                         Cost = d.Cost,
                                         Amount = d.Amount,
                                         Particulars = d.Particulars
                                     };

            return (Models.MstArticleInventory)articleInventories.FirstOrDefault();
        }

        // list article inventory by BranchId
        [Authorize]
        [HttpGet]
        [Route("api/listArticleInventoryBybranchId/{branchId}")]
        public List<Models.MstArticleInventory> listArticleInventoryBybranchId(String branchId)
        {
            var articleInventories = from d in db.MstArticleInventories
                                     where d.BranchId == Convert.ToInt32(branchId)
                                     && d.Quantity > 0
                                     && d.MstArticle.IsInventory == true
                                     select new Models.MstArticleInventory
                                     {
                                         Id = d.Id,
                                         BranchId = d.BranchId,
                                         Branch = d.MstBranch.Branch,
                                         ArticleId = d.ArticleId,
                                         Article = d.MstArticle.Article,
                                         InventoryCode = d.InventoryCode,
                                         Quantity = d.Quantity,
                                         Cost = d.Cost,
                                         Amount = d.Amount,
                                         Particulars = d.Particulars,
                                         ManualArticleCode = d.MstArticle.ManualArticleCode,
                                         UnitId = d.MstArticle.UnitId,
                                         Unit = d.MstArticle.MstUnit.Unit,
                                         Inventory = d.MstArticle.IsInventory,
                                         Price = d.MstArticle.Price
                                     };

            return articleInventories.ToList();
        }
    }
}
