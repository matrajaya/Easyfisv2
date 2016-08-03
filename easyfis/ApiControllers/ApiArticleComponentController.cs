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

        // list article component
        [Authorize]
        [HttpGet]
        [Route("api/listArticleComponent")]
        public List<Models.MstArticleComponent> listArticleComponent()
        {
            var articleComponents = from d in db.MstArticleComponents
                                    select new Models.MstArticleComponent
                                    {
                                        Id = d.Id,
                                        ArticleId = d.ArticleId,
                                        Article = d.MstArticle.ManualArticleCode,
                                        ComponentArticleId = d.ComponentArticleId,
                                        ComponentArticle = d.MstArticle1.Article,
                                        Quantity = d.Quantity,
                                        Unit = d.MstArticle.MstUnit.Unit,
                                        Cost = Convert.ToDecimal(d.MstArticle.Cost),
                                        Particulars = d.MstArticle.Particulars,
                                    };

            return articleComponents.ToList();
        }

        // list Article Component by ArticleId
        [Authorize]
        [HttpGet]
        [Route("api/listArticleComponent/{articleId}")]
        public List<Models.MstArticleComponent> listArticleComponentByArticleId(String articleId)
        {
            var articleComponents = from d in db.MstArticleComponents
                                    where d.ArticleId == Convert.ToInt32(articleId)
                                    && d.ComponentArticleId != Convert.ToInt32(articleId)
                                    && d.MstArticle1.Kitting == 2
                                    select new Models.MstArticleComponent
                                    {
                                        Id = d.Id,
                                        ArticleId = d.ArticleId,
                                        Article = d.MstArticle.ManualArticleCode,
                                        ComponentArticleId = d.ComponentArticleId,
                                        ComponentArticle = d.MstArticle1.Article,
                                        Quantity = d.Quantity,
                                        Unit = d.MstArticle.MstUnit.Unit,
                                        Cost = Convert.ToDecimal(d.MstArticle.Cost),
                                        Particulars = d.MstArticle.Particulars,
                                    };

            return articleComponents.ToList();
        }

        // get Article Component
        [Authorize]
        [HttpGet]
        [Route("api/articleComponent/{id}")]
        public Models.MstArticleComponent getArticleComponent(String id)
        {
            var articleComponents = from d in db.MstArticleComponents
                                    where d.Id == Convert.ToInt32(id)
                                    select new Models.MstArticleComponent
                                    {
                                        Id = d.Id,
                                        ArticleId = d.ArticleId,
                                        Article = d.MstArticle.ManualArticleCode,
                                        ComponentArticleId = d.ComponentArticleId,
                                        ComponentArticle = d.MstArticle1.Article,
                                        Quantity = d.Quantity,
                                        Unit = d.MstArticle.MstUnit.Unit,
                                        Cost = Convert.ToDecimal(d.MstArticle.Cost),
                                        Particulars = d.MstArticle.Particulars,
                                    };

            return (Models.MstArticleComponent)articleComponents.FirstOrDefault();
        }

        // add Article Component
        [Authorize]
        [HttpPost]
        [Route("api/addArticleComponent")]
        public Int32 insertArticleComponent(Models.MstArticleComponent component)
        {
            try
            {
                Data.MstArticleComponent newComponent = new Data.MstArticleComponent();
                newComponent.ArticleId = component.ArticleId;
                newComponent.ComponentArticleId = component.ComponentArticleId;
                newComponent.Quantity = component.Quantity;
                newComponent.Particulars = component.Particulars;

                db.MstArticleComponents.InsertOnSubmit(newComponent);
                db.SubmitChanges();

                return newComponent.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update Article Component
        [Authorize]
        [HttpPut]
        [Route("api/updateArticleComponent/{id}")]
        public HttpResponseMessage updateArticleComponent(String id, Models.MstArticleComponent component)
        {
            try
            {
                var components = from d in db.MstArticleComponents where d.Id == Convert.ToInt32(id) select d;
                if (components.Any())
                {
                    var updateComponent = components.FirstOrDefault();
                    updateComponent.ArticleId = component.ArticleId;
                    updateComponent.ComponentArticleId = component.ComponentArticleId;
                    updateComponent.Quantity = component.Quantity;
                    updateComponent.Particulars = component.Particulars;

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

        // delete Articke Component
        [Authorize]
        [HttpDelete]
        [Route("api/deleteArticleComponent/{id}")]
        public HttpResponseMessage deleteArticleComponent(String id)
        {
            try
            {
                var components = from d in db.MstArticleComponents where d.Id == Convert.ToInt32(id) select d;
                if (components.Any())
                {
                    db.MstArticleComponents.DeleteOnSubmit(components.First());
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
