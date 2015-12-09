using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiDiscountController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =============
        // LIST Discount
        // =============
        [Route("api/listDiscount")]
        public List<Models.MstDiscount> Get()
        {
            var discount = from d in db.MstDiscounts
                               select new Models.MstDiscount
                               {
                                   Id = d.Id,
                                   Discount = d.Discount,
                                   DiscountRate = d.DiscountRate,
                                   IsInclusive = d.IsInclusive,
                                   AccountId = d.AccountId,
                                   //Account = d.Account,
                                   IsLocked = d.IsLocked,
                                   CreatedById = d.CreatedById,
                                   //CreatedBy = d.CreatedBy,
                                   //CreatedDateTime = d.CreatedDateTime,
                                   UpdatedById = d.UpdatedById,
                                   //UpdatedBy = d.UpdatedBy,
                                   //UpdatedDateTime = d.UpdatedDateTime
                               };
            return discount.ToList();
        }
    }
}
