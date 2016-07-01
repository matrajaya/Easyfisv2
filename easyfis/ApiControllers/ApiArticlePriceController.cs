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

        // list article price
        [Authorize]
        [HttpGet]
        [Route("api/listArticlePrice")]
        public List<Models.MstArticlePrice> listArticlePrice()
        {
            var articlePrices = from d in db.MstArticlePrices
                                select new Models.MstArticlePrice
                                {
                                    Id = d.Id,
                                    ArticleId = d.ArticleId,
                                    Article = d.MstArticle.Article,
                                    PriceDescription = d.PriceDescription,
                                    Price = d.Price,
                                    Remarks = d.Remarks,
                                };

            return articlePrices.ToList();
        }

        // list article price by ArticleId
        [Authorize]
        [HttpGet]
        [Route("api/listArticlePrice/{articleId}")]
        public List<Models.MstArticlePrice> listArticlePriceByArticleId(String articleId)
        {
            var articlePrices = from d in db.MstArticlePrices
                                where d.ArticleId == Convert.ToInt32(articleId)
                                select new Models.MstArticlePrice
                                {
                                    Id = d.Id,
                                    ArticleId = d.ArticleId,
                                    Article = d.MstArticle.Article,
                                    PriceDescription = d.PriceDescription,
                                    Price = d.Price,
                                    Remarks = d.Remarks,
                                };

            return articlePrices.ToList();
        }

        // get article price
        [Authorize]
        [HttpGet]
        [Route("api/articlePrice/{id}")]
        public Models.MstArticlePrice GetPrice(String id)
        {
            var articlePrices = from d in db.MstArticlePrices
                                where d.Id == Convert.ToInt32(id)
                                select new Models.MstArticlePrice
                                {
                                    Id = d.Id,
                                    ArticleId = d.ArticleId,
                                    Article = d.MstArticle.Article,
                                    PriceDescription = d.PriceDescription,
                                    Price = d.Price,
                                    Remarks = d.Remarks,
                                };

            return (Models.MstArticlePrice)articlePrices.FirstOrDefault();
        }

        // add Article Price
        [Authorize]
        [HttpPost]
        [Route("api/addArticlePrice")]
        public Int32 insertArticlePrice(Models.MstArticlePrice price)
        {
            try
            {
                Data.MstArticlePrice newPrice = new Data.MstArticlePrice();
                newPrice.ArticleId = price.ArticleId;
                newPrice.PriceDescription = price.PriceDescription;
                newPrice.Price = price.Price;
                newPrice.Remarks = price.Remarks;

                db.MstArticlePrices.InsertOnSubmit(newPrice);
                db.SubmitChanges();

                return newPrice.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update article price
        [Authorize]
        [HttpPut]
        [Route("api/updateArticlePrice/{id}")]
        public HttpResponseMessage updateArticlePrice(String id, Models.MstArticlePrice price)
        {
            try
            {
                var prices = from d in db.MstArticlePrices where d.Id == Convert.ToInt32(id) select d;
                if (prices.Any())
                {
                    var updatePrice = prices.FirstOrDefault();
                    updatePrice.ArticleId = price.ArticleId;
                    updatePrice.PriceDescription = price.PriceDescription;
                    updatePrice.Price = price.Price;
                    updatePrice.Remarks = price.Remarks;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // delete article price
        [Authorize]
        [HttpDelete]
        [Route("api/deleteArticlePrice/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var prices = from d in db.MstArticlePrices where d.Id == Convert.ToInt32(id) select d;
                if (prices.Any())
                {
                    db.MstArticlePrices.DeleteOnSubmit(prices.First());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
