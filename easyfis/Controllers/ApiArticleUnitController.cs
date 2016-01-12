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
                                   Article = d.MstArticle.Article,
                                   UnitId = d.UnitId,
                                   Unit = d.MstUnit.Unit,
                                   Multiplier = d.Multiplier,
                                   IsCountUnit = d.IsCountUnit
                               };
            return articleUnits.ToList();
        }

        // ================
        // GET Article Unit
        // ================
        [Route("api/articleUnit/{id}")]
        public Models.MstArticleUnit GetArticleUnit(String id)
        {
            var articleUnitId = Convert.ToInt32(id);
            var articleUnits = from d in db.MstArticleUnits
                               where d.Id == articleUnitId
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

        // ==========================================
        // Get Article Unit By Article Id and Unit Id
        // ==========================================
        [Route("api/articleUnit/{articleId}/{unitId}")]
        public Models.MstArticleUnit GetArticleUnitByArticleId(String articleId, String unitId)
        {
            var articleUnit_articleId = Convert.ToInt32(articleId);
            var articleUnit_unitId = Convert.ToInt32(unitId);
            var articleUnits = from d in db.MstArticleUnits
                               where d.ArticleId == articleUnit_articleId
                               where d.UnitId == articleUnit_unitId
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


        // ================
        // ADD Article Unit
        // ================
        [Route("api/addArticleUnit")]
        public int Post(Models.MstArticleUnit unit)
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

        // ===================
        // UPDATE Article Unit
        // ===================
        [Route("api/updateArticleUnit/{id}")]
        public HttpResponseMessage Put(String id, Models.MstArticleUnit unit)
        {
            try
            {
                var unitId = Convert.ToInt32(id);
                var units = from d in db.MstArticleUnits where d.Id == unitId select d;

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

        // ===================
        // DELETE Article Unit
        // ===================
        [Route("api/deleteArticleUnit/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var unitId = Convert.ToInt32(id);
                var units = from d in db.MstArticleUnits where d.Id == unitId select d;

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
