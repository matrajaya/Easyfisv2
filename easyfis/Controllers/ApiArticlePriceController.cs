using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiArticlePriceController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==================
        // LIST Article Price
        // ==================
        [Route("api/listArticlePrice")]
        public List<Models.MstArticlePrice> Get()
        {
            var articlePrice = from d in db.MstArticlePrices
                                   select new Models.MstArticlePrice
                                   {
                                       Id = d.Id,
                                       ArticleId = d.ArticleId,
                                       //Article = d.Article,
                                       PriceDescription = d.PriceDescription,
                                       Price = d.Price,
                                       Remarks = d.Remarks,
                                     
                                   };
            return articlePrice.ToList();
        }
    }
}
