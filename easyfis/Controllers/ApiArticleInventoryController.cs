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

        // ======================
        // LIST Article Inventory
        // ======================
        [Route("api/listArticleInventory")]
        public List<Models.MstArticleInventory> Get()
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

        // ====================================
        // LIST Article Inventory By Article Id
        // ====================================
        [Route("api/listArticleInventoryByArticleId/{articleId}")]
        public List<Models.MstArticleInventory> GetArticleInventoryByArticleId(String articleId)
        {
            var articleInventory_articleId = Convert.ToInt32(articleId);
            var articleInventories = from d in db.MstArticleInventories
                                     where d.ArticleId == articleInventory_articleId
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

        // ====================================
        // LIST Article Inventory By Article Id
        // ====================================
        [Route("api/listArticleInventoryByBranchIdAndArticleId/{branchId}/{articleId}")]
        public List<Models.MstArticleInventory> GetArticleInventoryByArticleId(String branchId, String articleId)
        {
            var articleInventory_branchId = Convert.ToInt32(branchId);
            var articleInventory_articleId = Convert.ToInt32(articleId);
            var articleInventories = from d in db.MstArticleInventories
                                     where d.BranchId == articleInventory_branchId && d.ArticleId == articleInventory_articleId
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

        // ===================================
        // GET Article Inventory By Article Id
        // ===================================
        [Route("api/getArticleInventoryByArticleId/{branchId}/{articleId}")]
        public Models.MstArticleInventory GetArticleInventoryByBranchIdAndArticleId(String branchId, String articleId)
        {
            var articleInventory_branchId = Convert.ToInt32(branchId);
            var articleInventory_articleId = Convert.ToInt32(articleId);
            var articleInventories = from d in db.MstArticleInventories
                                     where d.BranchId == articleInventory_branchId && d.ArticleId == articleInventory_articleId && d.Quantity > 0
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
    }
}
