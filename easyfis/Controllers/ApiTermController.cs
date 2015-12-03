using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiTermController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =========
        // LIST Term
        // =========
        [Route("api/listTerm")]
        public List<Models.MstTerm> Get()
        {
            var terms = from d in db.MstTerms
                           select new Models.MstTerm
                           {
                               Id = d.Id,
                               Term = d.Term,
                               NumberOfDays = d.NumberOfDays,
                               IsLocked = d.IsLocked,
                               CreatedById = d.CreatedById,
                               CreatedBy = d.MstUser.FullName,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedById = d.UpdatedById,
                               UpdatedBy = d.MstUser1.FullName,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };
            return terms.ToList();
        }
    }
}
