using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiTaxTypeController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =============
        // LIST Tax Type
        // =============
        [Route("api/listTaxType")]
        public List<Models.MstTaxType> Get()
        {
            var taxTypes = from d in db.MstTaxTypes
                        select new Models.MstTaxType
                        {
                            Id = d.Id,
                            TaxType = d.TaxType,
                            TaxRate = d.TaxRate,
                            IsInclusive = d.IsInclusive,
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
            return taxTypes.ToList();
        }
    }
}
