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

        // =================
        // LIST Article Type
        // =================
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
                                   CreatedBy = d.MstUser.FullName,
                                   CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                   UpdatedById = d.UpdatedById,
                                   UpdatedBy = d.MstUser1.FullName,
                                   UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                               };
            return articleTypes.ToList();
        }
    }
}
