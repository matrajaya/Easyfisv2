using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiPayTypeController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ============
        // LIST PayTpes
        // ============
        [Route("api/listPayType")]
        public List<Models.MstPayType> Get()
        {
            var payTypes = from d in db.MstPayTypes
                                select new Models.MstPayType
                                {
                                    Id = d.Id,
                                    PayType = d.PayType,
                                    AccountId = d.AccountId,
                                    Account = d.MstAccount.Account,
                                    IsLocked = d.IsLocked,
                                    CreatedById = d.CreatedById,
                                    CreatedBy = d.MstUser.FullName,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedById = d.UpdatedById,
                                    UpdatedBy = d.MstUser1.FullName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                };
            return payTypes.ToList();
        }
    }
}
