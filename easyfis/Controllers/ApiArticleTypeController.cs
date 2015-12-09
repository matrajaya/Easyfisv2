using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiArticleTypeController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==================
        // LIST Article Type
        // ==================
        [Route("api/listArticleType")]
        public List<Models.MstArticleType> Get()
        {
            var articleTypes = from d in db.MstArticleTypes
                               select new Models.MstArticleType
                               {
                                   Id = d.Id,
                                   ArticleType = d.ArticleType,
                                   IsLocked = d.IsLocked,
                                   CreatedById = d.CreatedById,
                                   //CreatedBy = d.CreatedBy,
                                   //CreatedDateTime = d.CreatedDateTime,
                                   UpdatedById = d.UpdatedById,
                                   //UpdatedBy = d.UpdatedBy,
                                   //UpdatedDateTime = d.UpdatedDateTime
                               };
            return articleTypes.ToList();
        }
    }
}
