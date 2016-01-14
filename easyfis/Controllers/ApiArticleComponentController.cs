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
                                        Article = d.MstArticle.Article,
                                        ComponentArticleId = d.ComponentArticleId,
                                        ComponentArticle = d.MstArticle1.Article,
                                        Quantity = d.Quantity,
                                        Unit = d.MstArticle.MstUnit.Unit,
                                        Cost = Convert.ToDecimal(d.MstArticle.Cost),
                                        Particulars = d.MstArticle.Particulars,
                                    };
            return articleComponents.ToList();
        }

        // ===================================
        // LIST Article Component by ArticleId
        // ===================================
        [Route("api/listArticleComponent/{articleId}")]
        public List<Models.MstArticleComponent> GetArticleComponentByArticleTypeId(String articleId)
        {
            var articleComponents_articleId = Convert.ToInt32(articleId);
            var articleComponents = from d in db.MstArticleComponents
                                    where d.ArticleId == articleComponents_articleId
                                    select new Models.MstArticleComponent
                                    {
                                        Id = d.Id,
                                        ArticleId = d.ArticleId,
                                        Article = d.MstArticle.Article,
                                        ComponentArticleId = d.ComponentArticleId,
                                        ComponentArticle = d.MstArticle1.Article,
                                        Quantity = d.Quantity,
                                        Unit = d.MstArticle.MstUnit.Unit,
                                        Cost = Convert.ToDecimal(d.MstArticle.Cost),
                                        Particulars = d.MstArticle.Particulars,
                                    };
            return articleComponents.ToList();
        }

        // =====================
        // GET Article Component
        // =====================
        [Route("api/articleComponent/{id}")]
        public Models.MstArticleComponent GetArticleComponent(String id)
        {
            var articleComponentId = Convert.ToInt32(id);
            var articleComponents = from d in db.MstArticleComponents
                                    where d.Id == articleComponentId
                                    select new Models.MstArticleComponent
                                    {
                                        Id = d.Id,
                                        ArticleId = d.ArticleId,
                                        Article = d.MstArticle.Article,
                                        ComponentArticleId = d.ComponentArticleId,
                                        ComponentArticle = d.MstArticle1.Article,
                                        Quantity = d.Quantity,
                                        Unit = d.MstArticle.MstUnit.Unit,
                                        Cost = Convert.ToDecimal(d.MstArticle.Cost),
                                        Particulars = d.MstArticle.Particulars,
                                    };
            return (Models.MstArticleComponent)articleComponents.FirstOrDefault();
        }

        // =====================
        // ADD Article Component
        // =====================
        [Route("api/addArticleComponent")]
        public int Post(Models.MstArticleComponent component)
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

        // ========================
        // UPDATE Article Component
        // ========================
        [Route("api/updateArticleComponent/{id}")]
        public HttpResponseMessage Put(String id, Models.MstArticleComponent component)
        {
            try
            {
                var componentId = Convert.ToInt32(id);
                var components = from d in db.MstArticleComponents where d.Id == componentId select d;

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

        // ========================
        // DELETE Articke Component
        // ========================
        [Route("api/deleteArticleComponent/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var componentId = Convert.ToInt32(id);
                var components = from d in db.MstArticleComponents where d.Id == componentId select d;

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
