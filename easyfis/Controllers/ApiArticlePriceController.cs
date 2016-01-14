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

        // ================================
        // LIST Article Price By Article Id
        // ===========================-=====
        [Route("api/listArticlePrice/{articleId}")]
        public List<Models.MstArticlePrice> GetByArticleId(String articleId)
        {
            var articlePrices_articleId = Convert.ToInt32(articleId);
            var articlePrices = from d in db.MstArticlePrices
                                where d.ArticleId == articlePrices_articleId
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

        // =================
        // GET Article Price
        // =================
        [Route("api/articlePrice/{id}")]
        public Models.MstArticlePrice GetPrice(String id)
        {
            var articlePriceId = Convert.ToInt32(id);
            var articlePrices = from d in db.MstArticlePrices
                                where d.Id == articlePriceId
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

        // ==================
        // ADD Article Price
        // ==================
        [Route("api/addArticlePrice")]
        public int Post(Models.MstArticlePrice price)
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

        // ====================
        // UPDATE Article Price
        // ====================
        [Route("api/updateArticlePrice/{id}")]
        public HttpResponseMessage Put(String id, Models.MstArticlePrice price)
        {
            try
            {
                var priceId = Convert.ToInt32(id);
                var prices = from d in db.MstArticlePrices where d.Id == priceId select d;

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

        // ====================
        // DELETE Article Price
        // ====================
        [Route("api/deleteArticlePrice/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var priceId = Convert.ToInt32(id);
                var prices = from d in db.MstArticlePrices where d.Id == priceId select d;

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
