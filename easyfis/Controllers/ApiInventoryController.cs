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
            var invent = from d in db.TrnInventories
                        select new Models.TrnInventory
                        {
                            Id = d.Id,
                            BranchId = d.BranchId,
                            //Branch = d.Branch,
                            //InventoryDate = d.InventoryDate,
                            ArticleId = d.ArticleId,
                            ArticleInventoryId = d.ArticleInventoryId,
                            //RRId = d.RRId,
                            //SIId = d.SIId,
                            //INId = d.INId,
                            //OTId = d.OTId,
                            //STId = d.STId,
                            QuantityIn = d.QuantityIn,
                            Quantity = d.Quantity,
                            QuantityOut = d.QuantityOut,
                            Amount = d.Amount,
                            Particulars = d.Particulars
                        };
            return invent.ToList();
        }
    }
}
