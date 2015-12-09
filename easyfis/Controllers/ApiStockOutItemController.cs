using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiStockOutItemController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===================
        // LIST Stock Out Item
        // ===================
        [Route("api/listStockOutItem")]
        public List<Models.TrnStockOutItem> Get()
        {
            var stockOutItems = from d in db.TrnStockOutItems
                                select new Models.TrnStockOutItem
                                {
                                    Id = d.Id,
                                    //BranchId = d.BranchId,
                                    //Branch = d.Branch,
                                    OTId = d.OTId,
                                    //OT = d.OT,
                                    ExpenseAccountId = d.ExpenseAccountId,
                                    //ExpenseAccount = d.ExpenseAccount,
                                    ItemId = d.ItemId,
                                    //Item = d.Item,
                                    ItemInventoryId = d.ItemInventoryId,
                                    //ItemInventory = d.ItemInventory,
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
            return stockOutItems.ToList();
        }
    }
}
