using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiArticleUnitController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =================
        // LIST Article Unit
        // =================
        [Route("api/listArticleUnit")]
        public List<Models.MstArticleUnit> Get()
        {
            var articleUnits = from d in db.MstArticleUnits
                               select new Models.MstArticleUnit
                               {
                                   Id = d.Id,
                                   ArticleId = d.ArticleId,
                                   //Article = d.Article,
                                   UnitId = d.UnitId,
                                   Multiplier = d.Multiplier,
                                   //IsCountUnit = d.IsCountUnit
                               };
            return articleUnits.ToList();
        }
    }
}
