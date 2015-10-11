using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiAccountTypeController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =================
        // LIST Account Type
        // =================
        [Route("api/listAccountType")]
        public List<Models.MstAccountType> Get()
        {
            var accountTypes = from d in db.MstAccountTypes select new Models.MstAccountType
                           {
                               Id = d.Id,
                               AccountTypeCode = d.AccountTypeCode,
                               AccountType = d.AccountType,
                               AccountCategoryId = d.AccountCategoryId,
                               AccountCategory = d.MstAccountCategory.AccountCategory,
                               SubCategoryDescription = d.SubCategoryDescription,
                               IsLocked = d.IsLocked,
                               CreatedById = d.CreatedById,
                               CreatedBy = d.MstUser.UserName,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedById = d.UpdatedById,
                               UpdatedBy = d.MstUser1.UserName,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };
            return accountTypes.ToList();
        }
    }
}
