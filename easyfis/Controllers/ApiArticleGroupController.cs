using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiArticleGroupController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==================
        // LIST Article Group
        // ==================
        [Route("api/listArticleGroup")]
        public List<Models.MstArticleGroup> Get()
        {
            var articleGroups = from d in db.MstArticleGroups
                                select new Models.MstArticleGroup
                                {
                                    Id = d.Id,

                                    ArticleGroup = d.ArticleGroup,
                                    ArticleTypeId = d.ArticleTypeId,
                                    ArticleType = d.MstArticleType.ArticleType,
                                    AccountId = d.AccountId,
                                    Account = d.MstAccount.Account,
                                    SalesAccountId = d.SalesAccountId,
                                    CostAccountId = d.CostAccountId,
                                    AssetAccountId = d.AssetAccountId,
                                    ExpenseAccountId = d.ExpenseAccountId,

                                    IsLocked = d.IsLocked,
                                    CreatedById = d.CreatedById,
                                    CreatedBy = d.MstUser.FullName,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedById = d.UpdatedById,
                                    UpdatedBy = d.MstUser1.FullName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                };
            return articleGroups.ToList();
        }

        // =====================================
        // LIST Article Group by Article Type Id
        // =====================================
        [Route("api/listArticleGroupByArticleTypeId/{id}")]
        public List<Models.MstArticleGroup> GetArticleGroupByArticleTypeId(String id)
        {
            var articleTypeId = Convert.ToInt32(id);
            var articleGroups = from d in db.MstArticleGroups
                                where d.ArticleTypeId == articleTypeId
                                select new Models.MstArticleGroup
                                {
                                    Id = d.Id,
                                    ArticleGroup = d.ArticleGroup,
                                    ArticleTypeId = d.ArticleTypeId,
                                    ArticleType = d.MstArticleType.ArticleType,
                                    AccountId = d.AccountId,
                                    Account = d.MstAccount.Account,
                                    SalesAccountId = d.SalesAccountId,
                                    CostAccountId = d.CostAccountId,
                                    AssetAccountId = d.AssetAccountId,
                                    ExpenseAccountId = d.ExpenseAccountId,

                                    IsLocked = d.IsLocked,
                                    CreatedById = d.CreatedById,
                                    CreatedBy = d.MstUser.FullName,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedById = d.UpdatedById,
                                    UpdatedBy = d.MstUser1.FullName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                };
            return articleGroups.ToList();
        }
    }
}
