using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiStockInItemController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==================
        // LIST Stock In Item
        // ==================
        [Route("api/listStockInItem")]
        public List<Models.TrnStockInItem> Get()
        {
            var stockInItem = from d in db.TrnStockInItems
                        select new Models.TrnStockInItem
                        {
                            Id = d.Id,
                            INId = d.INId,
                            //IN = d.IN,
                            ItemId = d.ItemId,
                            //Item = d.Item,
                            Particulars = d.Particulars,
                            UnitId = d.UnitId,
                            //Unit = d.Unit,
                            Quantity = d.Quantity,
                            Cost = d.Cost,
                            Amount = d.Amount,
                            BaseUnitId = d.BaseUnitId,
                            //BaseUnit = d.BaseUnit,
                            BaseQuantity = d.BaseQuantity,
                            BaseCost = d.BaseCost
                        };
            return stockInItem.ToList();
        }
    }
}
