using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ModifiedApiControllers
{
    public class ApiItemInventoryController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==================
        // Get Item Inventory
        // ==================
        [Authorize, HttpGet, Route("api/itemInventory/list/{itemId}")]
        public List<Entities.MstArticleInventory> ListItemInventory(String itemId)
        {
            var itemInventory = from d in db.MstArticleInventories
                                where d.ArticleId == Convert.ToInt32(itemId)
                                && d.MstArticle.IsInventory == true
                                && d.Quantity != 0
                                select new Entities.MstArticleInventory
                                {
                                    Id = d.Id,
                                    Branch = d.MstBranch.Branch,
                                    InventoryCode = d.InventoryCode,
                                    Quantity = d.Quantity,
                                    Cost = d.Cost,
                                    Amount = d.Amount,
                                    Particulars = d.Particulars
                                };

            return itemInventory.ToList();
        }
    }
}
