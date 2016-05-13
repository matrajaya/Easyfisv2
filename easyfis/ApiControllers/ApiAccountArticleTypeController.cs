using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiAccountArticleTypeController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =========================
        // LIST Account Article Type
        // =========================
        [Route("api/listAccountArticleType")]
        public List<Models.MstAccountArticleType> Get()
        {
            var accountArticleTypes = from d in db.MstAccountArticleTypes
                                      select new Models.MstAccountArticleType
                                     {
                                         Id = d.Id,
                                         AccountId = d.AccountId,
                                         Account = d.MstAccount.Account,
                                         ArticleTypeId = d.ArticleTypeId,
                                         ArticleType = d.MstArticleType.ArticleType
                                     };
            return accountArticleTypes.ToList();
        }

        // =======================================
        // LIST Account Article Type By Account Id
        // =======================================
        [Route("api/listAccountArticleTypeByAccountId/{accountId}")]
        public List<Models.MstAccountArticleType> GetAccountArticleTypeByAccountId(String accountId)
        {
            var accountArticleType_accountId = Convert.ToInt32(accountId);
            var accountArticleTypes = from d in db.MstAccountArticleTypes
                                      where d.AccountId == accountArticleType_accountId
                                      select new Models.MstAccountArticleType
                                      {
                                          Id = d.Id,
                                          AccountId = d.AccountId,
                                          Account = d.MstAccount.Account,
                                          ArticleTypeId = d.ArticleTypeId,
                                          ArticleType = d.MstArticleType.ArticleType
                                      };
            return accountArticleTypes.ToList();
        }

        // ========================
        // ADD Account Article Type 
        // ========================
        [Route("api/addAccountArticleType")]
        public int Post(Models.MstAccountArticleType accountArticleType)
        {
            try
            {
                Data.MstAccountArticleType newAccountArticleType = new Data.MstAccountArticleType();

                newAccountArticleType.AccountId = accountArticleType.AccountId;
                newAccountArticleType.ArticleTypeId = accountArticleType.ArticleTypeId;

                db.MstAccountArticleTypes.InsertOnSubmit(newAccountArticleType);
                db.SubmitChanges();

                return newAccountArticleType.Id;

            }
            catch
            {
                return 0;
            }
        }

        // ===========================
        // UPDATE Account Article Type 
        // ===========================
        [Route("api/updateAccountArticleType/{id}")]
        public HttpResponseMessage Put(String id, Models.MstAccountArticleType accountArticleType)
        {
            try
            {
                var accountArticleTypeId = Convert.ToInt32(id);
                var accountArticleTypes = from d in db.MstAccountArticleTypes where d.Id == accountArticleTypeId select d;

                if (accountArticleTypes.Any())
                {
                    var updateAccountArticleTypes= accountArticleTypes.FirstOrDefault();

                    updateAccountArticleTypes.AccountId = accountArticleType.AccountId;
                    updateAccountArticleTypes.ArticleTypeId = accountArticleType.ArticleTypeId;

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

        // ===========================
        // DELETE Account Article Type 
        // ===========================
        [Route("api/deleteAccountArticleType/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var accountArticleTypeId = Convert.ToInt32(id);
                var accountArticleTypes = from d in db.MstAccountArticleTypes where d.Id == accountArticleTypeId select d;

                if (accountArticleTypes.Any())
                {
                    db.MstAccountArticleTypes.DeleteOnSubmit(accountArticleTypes.First());
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
