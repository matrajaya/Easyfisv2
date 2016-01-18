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
                                         ArticleId = d.ArticleId,
                                         InventoryCode = d.InventoryCode,
                                         Quantity = d.Quantity,
                                         Cost = d.Cost,
                                         Amount = d.Amount,
                                         Particulars = d.Particulars
                                     };
            return articleInventories.ToList();
        }
    }
}
