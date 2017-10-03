using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiItemComponentListController : ApiController
    {
        // ============
        // Data Context 
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ================
        // Get Highest Cost
        // ================
        public Decimal GetHighestCost(Int32 articleId)
        {
            var articleInventories = from d in db.MstArticleInventories.OrderByDescending(d => d.Cost)
                                     where d.ArticleId == articleId
                                     select d;

            if (articleInventories.Any())
            {
                return articleInventories.FirstOrDefault().Cost;
            }
            else
            {
                return 0;
            }
        }

        // ===================
        // Item Component List
        // ===================
        [Authorize, HttpGet, Route("api/itemComponentList/list/{itemGroupId}")]
        public List<Models.MstArticleComponent> ListStockInDetailReport(String itemGroupId)
        {
            var articleComponents = from d in db.MstArticleComponents
                                    where d.MstArticle.ArticleGroupId == Convert.ToInt32(itemGroupId)
                                    select new Models.MstArticleComponent
                                    {
                                        Id = d.Id,
                                        ArticleId = d.ArticleId,
                                        Article = d.MstArticle.Article,
                                        ManualArticleCode = d.MstArticle1.ManualArticleCode,
                                        ComponentArticleId = d.ComponentArticleId,
                                        ComponentArticle = d.MstArticle1.Article,
                                        Quantity = d.Quantity,
                                        Unit = d.MstArticle1.MstUnit.Unit,
                                        Cost = GetHighestCost(d.ComponentArticleId),
                                        Particulars = d.MstArticle.Particulars,
                                    };
            return articleComponents.ToList();
        }
    }
}
