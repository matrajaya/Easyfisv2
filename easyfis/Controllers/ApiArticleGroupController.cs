using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

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
                                    AccountCode = d.MstAccount.AccountCode,
                                    Account = d.MstAccount.Account,
                                    SalesAccountId = d.SalesAccountId,
                                    SalesAccountCode = d.MstAccount4.AccountCode,
                                    SalesAccount = d.MstAccount4.Account,
                                    CostAccountId = d.CostAccountId,
                                    CostAccountCode = d.MstAccount2.AccountCode,
                                    CostAccount = d.MstAccount2.Account,
                                    AssetAccountId = d.AssetAccountId,
                                    AssetAccountCode = d.MstAccount1.AccountCode,
                                    AssetAccount = d.MstAccount1.Account,
                                    ExpenseAccountId = d.ExpenseAccountId,
                                    ExpenseAccountCode = d.MstAccount3.AccountCode,
                                    ExpenseAccount = d.MstAccount3.Account,
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
                                    AccountCode = d.MstAccount.AccountCode,
                                    Account = d.MstAccount.Account,
                                    SalesAccountId = d.SalesAccountId,
                                    SalesAccountCode = d.MstAccount4.AccountCode,
                                    SalesAccount = d.MstAccount4.Account,
                                    CostAccountId = d.CostAccountId,
                                    CostAccountCode = d.MstAccount2.AccountCode,
                                    CostAccount = d.MstAccount2.Account,
                                    AssetAccountId = d.AssetAccountId,
                                    AssetAccountCode = d.MstAccount1.AccountCode,
                                    AssetAccount = d.MstAccount1.Account,
                                    ExpenseAccountId = d.ExpenseAccountId,
                                    ExpenseAccountCode = d.MstAccount3.AccountCode,
                                    ExpenseAccount = d.MstAccount3.Account,
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

        // =================
        // ADD Article Group
        // =================
        [Route("api/addArticleGroup")]
        public int Post(Models.MstArticleGroup articleGroup)
        {
            try
            {
                //var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.MstArticleGroup newArticleGroup = new Data.MstArticleGroup();

                newArticleGroup.ArticleGroup = articleGroup.ArticleGroup;
                newArticleGroup.ArticleTypeId = articleGroup.ArticleTypeId;
                newArticleGroup.AccountId = articleGroup.AccountId;
                newArticleGroup.SalesAccountId = articleGroup.SalesAccountId;
                newArticleGroup.CostAccountId = articleGroup.CostAccountId;
                newArticleGroup.AssetAccountId = articleGroup.AssetAccountId;
                newArticleGroup.ExpenseAccountId = articleGroup.ExpenseAccountId;
                newArticleGroup.IsLocked = articleGroup.IsLocked;
                newArticleGroup.CreatedById = mstUserId;
                newArticleGroup.CreatedDateTime = date;
                newArticleGroup.UpdatedById = mstUserId;
                newArticleGroup.UpdatedDateTime = date;

                db.MstArticleGroups.InsertOnSubmit(newArticleGroup);
                db.SubmitChanges();

                return newArticleGroup.Id;
            }
            catch
            {
                return 0;
            }
        }

        // ====================
        // UPDATE Article Group
        // ====================
        [Route("api/updateArticleGroup/{id}")]
        public HttpResponseMessage Put(String id, Models.MstArticleGroup articleGroup)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var articleGroup_Id = Convert.ToInt32(id);
                var articleGroups = from d in db.MstArticleGroups where d.Id == articleGroup_Id select d;

                if (articleGroups.Any())
                {
                    var updateArticleGroup = articleGroups.FirstOrDefault();

                    updateArticleGroup.ArticleGroup = articleGroup.ArticleGroup;
                    updateArticleGroup.ArticleTypeId = articleGroup.ArticleTypeId;
                    updateArticleGroup.AccountId = articleGroup.AccountId;
                    updateArticleGroup.SalesAccountId = articleGroup.SalesAccountId;
                    updateArticleGroup.CostAccountId = articleGroup.CostAccountId;
                    updateArticleGroup.AssetAccountId = articleGroup.AssetAccountId;
                    updateArticleGroup.ExpenseAccountId = articleGroup.ExpenseAccountId;
                    updateArticleGroup.IsLocked = articleGroup.IsLocked;
                    updateArticleGroup.UpdatedById = mstUserId;
                    updateArticleGroup.UpdatedDateTime = date;

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
        // DELETE Article Group
        // ====================
        [Route("api/deleteArticleGroup/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var articleGroup_Id = Convert.ToInt32(id);
                var articleGroups = from d in db.MstArticleGroups where d.Id == articleGroup_Id select d;

                if (articleGroups.Any())
                {
                    db.MstArticleGroups.DeleteOnSubmit(articleGroups.First());
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
