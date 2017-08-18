using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiArticleInventoryController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            return (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.BranchId).SingleOrDefault();
        }

        // list article inventory
        [Authorize]
        [HttpGet]
        [Route("api/listArticleInventory")]
        public List<Models.MstArticleInventory> listArticleInventory()
        {
            var articleInventories = from d in db.MstArticleInventories
                                     where d.BranchId == currentBranchId()
                                     && d.MstArticle.IsLocked == true
                                     && d.MstArticle.IsInventory == true
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

        // list article inventory
        [Authorize]
        [HttpGet]
        [Route("api/listArticleInventoryLocked")]
        public List<Models.MstArticleInventory> listArticleInventoryLocked()
        {
            var articleInventories = from d in db.MstArticleInventories.OrderBy(d => d.MstArticle.Article)
                                     where d.BranchId == currentBranchId()
                                     && d.MstArticle.IsLocked == true
                                     && d.MstArticle.IsInventory == true
                                     && d.Quantity > 0
                                     select new Models.MstArticleInventory
                                     {
                                         Id = d.Id,
                                         BranchId = d.BranchId,
                                         ArticleId = d.ArticleId,
                                         ManualArticleCode = d.MstArticle.ManualArticleCode,
                                         Article = d.MstArticle.Article,
                                         InventoryCode = d.InventoryCode,
                                         Price = d.MstArticle.Price,
                                         Quantity = d.Quantity,
                                         UnitId = d.MstArticle.UnitId,
                                         Unit = d.MstArticle.MstUnit.Unit,
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
                                     where d.BranchId == currentBranchId()
                                     && d.ArticleId == Convert.ToInt32(articleId)
                                     && d.MstArticle.IsLocked == true
                                     && d.MstArticle.IsInventory == true
                                     && d.Quantity > 0
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

        // list article inventory by ArticleId
        [Authorize]
        [HttpGet]
        [Route("api/listArticleInventoryByArticleId/items/{articleId}")]
        public List<Models.MstArticleInventory> listArticleInventoryByArticleIdItems(String articleId)
        {
            var articleInventories = from d in db.MstArticleInventories
                                     where d.ArticleId == Convert.ToInt32(articleId)
                                     && d.MstArticle.IsInventory == true
                                     && d.Quantity != 0
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
                                     where d.BranchId == currentBranchId()
                                     && d.ArticleId == Convert.ToInt32(articleId)
                                     && d.MstArticle.IsLocked == true
                                     && d.MstArticle.IsInventory == true
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
                                     where d.BranchId == currentBranchId()
                                     && d.ArticleId == Convert.ToInt32(articleId) 
                                     && d.MstArticle.IsLocked == true
                                     && d.MstArticle.IsInventory == true
                                     && d.Quantity > 0
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

            return (Models.MstArticleInventory)articleInventories.FirstOrDefault();
        }

        // list article inventory by BranchId
        [Authorize]
        [HttpGet]
        [Route("api/listArticleInventoryBybranchId/{branchId}")]
        public List<Models.MstArticleInventory> listArticleInventoryBybranchId(String branchId)
        {
            var articleInventories = from d in db.MstArticleInventories.OrderBy(d => d.MstArticle.Article)
                                     where d.BranchId == currentBranchId()
                                     && d.MstArticle.IsLocked == true
                                     && d.MstArticle.IsInventory == true
                                     && d.Quantity > 0
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
