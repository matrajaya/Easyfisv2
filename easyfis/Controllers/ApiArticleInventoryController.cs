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
            var articleInventory = from d in db.MstArticleInventories
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
            return articleInventory.ToList();
        }
    }
}
