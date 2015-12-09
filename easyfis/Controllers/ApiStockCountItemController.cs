using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiStockCountItemController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =====================
        // LIST Stock Count Item
        // =====================
        [Route("api/listStockCountItem")]
        public List<Models.TrnStockCountItem> Get()
        {
            var stockCountItems = from d in db.TrnStockCountItems
                                  select new Models.TrnStockCountItem
                                  {
                                      Id = d.Id,
                                      SCId = d.SCId,
                                      //SC = d.SC,
                                      ItemId = d.ItemId,
                                      //Item = d.Item,
                                      Particulars = d.Particulars,
                                      Quantity = d.Quantity
                                  };
            return stockCountItems.ToList();
        }
    }
}
