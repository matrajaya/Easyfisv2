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

        // list article unit
        [Authorize]
        [HttpGet]
        [Route("api/listArticleUnit")]
        public List<Models.MstArticleUnit> listArticleUnit()
        {
            var articleUnits = from d in db.MstArticleUnits
                               select new Models.MstArticleUnit
                               {
                                   Id = d.Id,
                                   ArticleId = d.ArticleId,
                                   Article = d.MstArticle.Article,
                                   UnitId = d.UnitId,
                                   Unit = d.MstUnit.Unit,
                                   Multiplier = d.Multiplier,
                                   IsCountUnit = d.IsCountUnit
                               };

            return articleUnits.ToList();
        }

        // list article unit by ArticleId
        [Authorize]
        [HttpGet]
        [Route("api/listArticleUnitByArticleId/{articleId}")]
        public List<Models.MstArticleUnit> listArticleUnitByArticleId(String articleId)
        {
            var articleUnits = from d in db.MstArticleUnits
                               where d.ArticleId == Convert.ToInt32(articleId)
                               select new Models.MstArticleUnit
                               {
                                   Id = d.Id,
                                   ArticleId = d.ArticleId,
                                   Article = d.MstArticle.Article,
                                   UnitId = d.UnitId,
                                   Unit = d.MstUnit.Unit,
                                   Multiplier = d.Multiplier,
                                   IsCountUnit = d.IsCountUnit
                               };

            return articleUnits.ToList();
        }

        // get article unit
        [Authorize]
        [HttpGet]
        [Route("api/articleUnit/{id}")]
        public Models.MstArticleUnit getArticleUnit(String id)
        {
            var articleUnits = from d in db.MstArticleUnits
                               where d.Id == Convert.ToInt32(id)
                               select new Models.MstArticleUnit
                               {
                                   Id = d.Id,
                                   ArticleId = d.ArticleId,
                                   Article = d.MstArticle.Article,
                                   UnitId = d.UnitId,
                                   Unit = d.MstUnit.Unit,
                                   Multiplier = d.Multiplier,
                                   IsCountUnit = d.IsCountUnit
                               };

            return (Models.MstArticleUnit)articleUnits.FirstOrDefault();
        }

        // get article unit by ArticleId and UnitId
        [Authorize]
        [HttpGet]
        [Route("api/articleUnit/{articleId}/{unitId}")]
        public Models.MstArticleUnit getArticleUnitByArticleIdByUnitId(String articleId, String unitId)
        {
            var articleUnits = from d in db.MstArticleUnits
                               where d.ArticleId == Convert.ToInt32(articleId)
                               && d.UnitId == Convert.ToInt32(unitId)
                               select new Models.MstArticleUnit
                               {
                                   Id = d.Id,
                                   ArticleId = d.ArticleId,
                                   Article = d.MstArticle.Article,
                                   UnitId = d.UnitId,
                                   Unit = d.MstUnit.Unit,
                                   Multiplier = d.Multiplier,
                                   IsCountUnit = d.IsCountUnit
                               };

            return (Models.MstArticleUnit)articleUnits.FirstOrDefault();
        }

        // add article unit
        [Authorize]
        [HttpPost]
        [Route("api/addArticleUnit")]
        public Int32 insertArticleUnit(Models.MstArticleUnit unit)
        {
            try
            {
                Data.MstArticleUnit newUnit = new Data.MstArticleUnit();
                newUnit.ArticleId = unit.ArticleId;
                newUnit.UnitId = unit.UnitId;
                newUnit.Multiplier = unit.Multiplier;
                newUnit.IsCountUnit = unit.IsCountUnit;

                db.MstArticleUnits.InsertOnSubmit(newUnit);
                db.SubmitChanges();

                return newUnit.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update article unit
        [Authorize]
        [HttpPut]
        [Route("api/updateArticleUnit/{id}")]
        public HttpResponseMessage updateArticleUnit(String id, Models.MstArticleUnit unit)
        {
            try
            {
                var units = from d in db.MstArticleUnits where d.Id == Convert.ToInt32(id) select d;
                if (units.Any())
                {
                    var updateUnit = units.FirstOrDefault();
                    updateUnit.ArticleId = unit.ArticleId;
                    updateUnit.UnitId = unit.UnitId;
                    updateUnit.Multiplier = unit.Multiplier;
                    updateUnit.IsCountUnit = unit.IsCountUnit;

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

        // delete article unit
        [Authorize]
        [HttpDelete]
        [Route("api/deleteArticleUnit/{id}")]
        public HttpResponseMessage deleteArticleUnit(String id)
        {
            try
            {
                var units = from d in db.MstArticleUnits where d.Id == Convert.ToInt32(id) select d;
                if (units.Any())
                {
                    db.MstArticleUnits.DeleteOnSubmit(units.First());
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
