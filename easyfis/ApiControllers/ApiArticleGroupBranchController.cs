using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiArticleGroupBranchController : ApiController
    {
        // ====================
        // Easyfis Data Context
        // ====================
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =========================
        // List Article Group Branch
        // =========================
        [Authorize, HttpGet, Route("api/articleGroupBranch/list/{articleGroupId}")]
        public List<Models.MstArticleGroupBranch> listArticleGroupBranch(String articleGroupId)
        {
            var articleGroupBranch = from d in db.MstArticleGroupBranches
                                     where d.ArticleGroupId == Convert.ToInt32(articleGroupId)
                                     select new Models.MstArticleGroupBranch
                                     {
                                         Id = d.Id,
                                         ArticleGroupId = d.ArticleGroupId,
                                         ArticleGroup = d.MstArticleGroup.ArticleGroup,
                                         BranchId = d.BranchId,
                                         Branch = d.MstBranch.Branch,
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
                                         ExpenseAccount = d.MstAccount4.Account
                                     };

            return articleGroupBranch.ToList();
        }

        // ========================
        // Add Article Group Branch
        // ========================
        [Authorize, HttpPost, Route("api/articleGroupBranch/add")]
        public HttpResponseMessage addArticleGroupBranch(Models.MstArticleGroupBranch articleGroupBranch)
        {
            try
            {
                Data.MstArticleGroupBranch newArticleGroupBranch = new Data.MstArticleGroupBranch();
                newArticleGroupBranch.ArticleGroupId = articleGroupBranch.ArticleGroupId;
                newArticleGroupBranch.BranchId = articleGroupBranch.BranchId;
                newArticleGroupBranch.AccountId = articleGroupBranch.AccountId;
                newArticleGroupBranch.SalesAccountId = articleGroupBranch.SalesAccountId;
                newArticleGroupBranch.CostAccountId = articleGroupBranch.CostAccountId;
                newArticleGroupBranch.AssetAccountId = articleGroupBranch.AssetAccountId;
                newArticleGroupBranch.ExpenseAccountId = articleGroupBranch.ExpenseAccountId;
                db.MstArticleGroupBranches.InsertOnSubmit(newArticleGroupBranch);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // ===========================
        // Update Article Group Branch
        // ===========================
        [Authorize, HttpPut, Route("api/articleGroupBranch/update/{id}")]
        public HttpResponseMessage updateArticleGroupBranch(String id, Models.MstArticleGroupBranch articleGroupBranch)
        {
            try
            {
                var articleGroupBranches = from d in db.MstArticleGroupBranches
                                           where d.Id == Convert.ToInt32(id)
                                           select d;

                if (articleGroupBranches.Any())
                {
                    var updateArticleGroupBranch = articleGroupBranches.FirstOrDefault();
                    updateArticleGroupBranch.BranchId = articleGroupBranch.BranchId;
                    updateArticleGroupBranch.AccountId = articleGroupBranch.AccountId;
                    updateArticleGroupBranch.SalesAccountId = articleGroupBranch.SalesAccountId;
                    updateArticleGroupBranch.CostAccountId = articleGroupBranch.CostAccountId;
                    updateArticleGroupBranch.AssetAccountId = articleGroupBranch.AssetAccountId;
                    updateArticleGroupBranch.ExpenseAccountId = articleGroupBranch.ExpenseAccountId;
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // ===========================
        // Delete Article Group Branch
        // ===========================
        [Authorize, HttpDelete, Route("api/articleGroupBranch/delete/{id}")]
        public HttpResponseMessage deleteArticleGroupBranch(String id)
        {
            try
            {
                var articleGroupBranches = from d in db.MstArticleGroupBranches
                                           where d.Id == Convert.ToInt32(id)
                                           select d;

                if (articleGroupBranches.Any())
                {
                    db.MstArticleGroupBranches.DeleteOnSubmit(articleGroupBranches.First());
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
