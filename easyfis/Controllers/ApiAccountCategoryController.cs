using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiAccountCategoryController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =====================
        // LIST Account Category
        // =====================
        [Route("api/listAccountCategory")]
        public List<Models.MstAccountCategory> Get()
        {
            var accountCategories = from d in db.MstAccountCategories select new Models.MstAccountCategory
                               {
                                   Id = d.Id,
                                   AccountCategoryCode = d.AccountCategoryCode,
                                   AccountCategory = d.AccountCategory,
                                   IsLocked = d.IsLocked,
                                   CreatedById = d.CreatedById,
                                   CreatedBy = d.MstUser.UserName,
                                   CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                   UpdatedById = d.UpdatedById,
                                   UpdatedBy = d.MstUser1.UserName,
                                   UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                               };
            return accountCategories.ToList();
        }
    }
}
