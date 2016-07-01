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

        // list account article type
        [Authorize]
        [HttpGet]
        [Route("api/listAccountArticleType")]
        public List<Models.MstAccountArticleType> listAccountArticleType()
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

        // list account article type By AccountId
        [Authorize]
        [HttpGet]
        [Route("api/listAccountArticleTypeByAccountId/{accountId}")]
        public List<Models.MstAccountArticleType> listAccountArticleTypeByAccountId(String accountId)
        {
            var accountArticleTypes = from d in db.MstAccountArticleTypes
                                      where d.AccountId == Convert.ToInt32(accountId)
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

        // add account article type 
        [Authorize]
        [HttpPost]
        [Route("api/addAccountArticleType")]
        public Int32 insertAccountArticleType(Models.MstAccountArticleType accountArticleType)
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

        // update account article type
        [Authorize]
        [HttpPut]
        [Route("api/updateAccountArticleType/{id}")]
        public HttpResponseMessage updateAccountArticleType(String id, Models.MstAccountArticleType accountArticleType)
        {
            try
            {
                var accountArticleTypes = from d in db.MstAccountArticleTypes where d.Id == Convert.ToInt32(id) select d;
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

        // delete account article type
        [Authorize]
        [HttpDelete]
        [Route("api/deleteAccountArticleType/{id}")]
        public HttpResponseMessage deleteAccountArticleType(String id)
        {
            try
            {
                var accountArticleTypes = from d in db.MstAccountArticleTypes where d.Id == Convert.ToInt32(id) select d;
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
