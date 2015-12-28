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
                                    SalesAccount = d.MstAccount1.Account,
                                    CostAccountId = d.CostAccountId,
                                    CostAccount = d.MstAccount2.Account,
                                    AssetAccountId = d.AssetAccountId,
                                    AssetAccount = d.MstAccount3.Account,
                                    ExpenseAccountId = d.ExpenseAccountId,
                                    ExpenseAccount = d.MstAccount4.Account,
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
        // List Article Group by Article Type Id
        // =====================================
        [Route("api/listArticleGroupByArticleTypeId/{articleTypeId}")]
        public List<Models.MstArticleGroup> GetArticleGroupByArticleTypeId(String articleTypeId)
        {
            var article_articleTypeId = Convert.ToInt32(articleTypeId);
            var articleGroups = from d in db.MstArticleGroups
                                where d.ArticleTypeId == article_articleTypeId
                                select new Models.MstArticleGroup
                                {
                                    Id = d.Id,
                                    ArticleGroup = d.ArticleGroup,
                                    ArticleTypeId = d.ArticleTypeId,
                                    ArticleType = d.MstArticleType.ArticleType,
                                    AccountId = d.AccountId,
                                    Account = d.MstAccount.Account,
                                    SalesAccountId = d.SalesAccountId,
                                    SalesAccount = d.MstAccount1.Account,
                                    CostAccountId = d.CostAccountId,
                                    CostAccount = d.MstAccount2.Account,
                                    AssetAccountId = d.AssetAccountId,
                                    AssetAccount = d.MstAccount3.Account,
                                    ExpenseAccountId = d.ExpenseAccountId,
                                    ExpenseAccount = d.MstAccount4.Account,
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
