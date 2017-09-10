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

        // list article group account
        [Authorize]
        [HttpGet]
        [Route("api/articleGroup/account/list/{articleTypeId}")]
        public List<Models.MstArticleGroup> listArticleGroupAccount(String articleTypeId)
        {
            var articleGroups = from d in db.MstArticleGroups.OrderBy(d => d.ArticleGroup)
                                where d.ArticleTypeId == Convert.ToInt32(articleTypeId)
                                group d by new
                                {
                                    AccountId = d.AccountId,
                                    AccountCode = d.MstAccount.AccountCode,
                                    Account = d.MstAccount.Account
                                } into g
                                select new Models.MstArticleGroup
                                {
                                    AccountId = g.Key.AccountId,
                                    AccountCode = g.Key.AccountCode,
                                    Account = g.Key.Account
                                };

            return articleGroups.ToList();
        }


        // list article group
        [Authorize]
        [HttpGet]
        [Route("api/listArticleGroup")]
        public List<Models.MstArticleGroup> listArticleGroup()
        {
            var articleGroups = from d in db.MstArticleGroups.OrderBy(d => d.ArticleGroup)
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
                                    SalesAccountCode = d.MstAccount1.AccountCode,
                                    SalesAccount = d.MstAccount1.Account,
                                    CostAccountId = d.CostAccountId,
                                    CostAccountCode = d.MstAccount2.AccountCode,
                                    CostAccount = d.MstAccount2.Account,
                                    AssetAccountId = d.AssetAccountId,
                                    AssetAccountCode = d.MstAccount3.AccountCode,
                                    AssetAccount = d.MstAccount3.Account,
                                    ExpenseAccountId = d.ExpenseAccountId,
                                    ExpenseAccountCode = d.MstAccount4.AccountCode,
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

        // list article group by ArticleTypeId
        [Authorize]
        [HttpGet]
        [Route("api/listArticleGroupByArticleTypeId/{articleTypeId}")]
        public List<Models.MstArticleGroup> listArticleGroupByArticleTypeId(String articleTypeId)
        {
            var articleGroups = from d in db.MstArticleGroups.OrderBy(d => d.ArticleGroup)
                                where d.ArticleTypeId == Convert.ToInt32(articleTypeId)
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
                                    SalesAccountCode = d.MstAccount1.AccountCode,
                                    SalesAccount = d.MstAccount1.Account,
                                    CostAccountId = d.CostAccountId,
                                    CostAccountCode = d.MstAccount2.AccountCode,
                                    CostAccount = d.MstAccount2.Account,
                                    AssetAccountId = d.AssetAccountId,
                                    AssetAccountCode = d.MstAccount3.AccountCode,
                                    AssetAccount = d.MstAccount3.Account,
                                    ExpenseAccountId = d.ExpenseAccountId,
                                    ExpenseAccountCode = d.MstAccount4.AccountCode,
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

        // add Article Group
        [Authorize]
        [HttpPost]
        [Route("api/addArticleGroup")]
        public Int32 insertArticleGroup(Models.MstArticleGroup articleGroup)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.MstArticleGroup newArticleGroup = new Data.MstArticleGroup();
                newArticleGroup.ArticleGroup = articleGroup.ArticleGroup;
                newArticleGroup.ArticleTypeId = articleGroup.ArticleTypeId;
                newArticleGroup.AccountId = articleGroup.AccountId;
                newArticleGroup.SalesAccountId = articleGroup.SalesAccountId;
                newArticleGroup.CostAccountId = articleGroup.CostAccountId;
                newArticleGroup.AssetAccountId = articleGroup.AssetAccountId;
                newArticleGroup.ExpenseAccountId = articleGroup.ExpenseAccountId;
                newArticleGroup.IsLocked = articleGroup.IsLocked;
                newArticleGroup.CreatedById = userId;
                newArticleGroup.CreatedDateTime = DateTime.Now;
                newArticleGroup.UpdatedById = userId;
                newArticleGroup.UpdatedDateTime = DateTime.Now;

                db.MstArticleGroups.InsertOnSubmit(newArticleGroup);
                db.SubmitChanges();

                return newArticleGroup.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update Article Group
        [Authorize]
        [HttpPut]
        [Route("api/updateArticleGroup/{id}")]
        public HttpResponseMessage updateArticleGroup(String id, Models.MstArticleGroup articleGroup)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var articleGroups = from d in db.MstArticleGroups where d.Id == Convert.ToInt32(id) select d;
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
                    updateArticleGroup.UpdatedById = userId;
                    updateArticleGroup.UpdatedDateTime = DateTime.Now;

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

        // delete article group
        [Authorize]
        [HttpDelete]
        [Route("api/deleteArticleGroup/{id}")]
        public HttpResponseMessage deleteArticleGroup(String id)
        {
            try
            {
                var articleGroups = from d in db.MstArticleGroups where d.Id == Convert.ToInt32(id) select d;
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
