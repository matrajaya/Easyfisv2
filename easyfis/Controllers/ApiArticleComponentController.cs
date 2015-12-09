using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiArticleComponentController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ======================
        // LIST Article Component
        // ======================
        [Route("api/listArticleComponent")]
        public List<Models.MstArticleComponent> Get()
        {
            var articleComponents = from d in db.MstArticleComponents
                               select new Models.MstArticleComponent
                               {
                                   Id = d.Id,
                                   ArticleId = d.ArticleId,
                                   ComponentArticleId = d.ComponentArticleId,
                                   Quantity = d.Quantity,
                                   Particulars = d.Particulars
                               };
            return articleComponents.ToList();
        }
    }
}
