using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiAccountCategoryController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =====================
        // LIST Account Category
        // =====================
        [Route("api/listAccountCategory")]
        public List<Models.MstAccountCategory> Get()
        {
            var accountCategories = from d in db.MstAccountCategories
                                    select new Models.MstAccountCategory
                                        {
                                            Id = d.Id,
                                            AccountCategoryCode = d.AccountCategoryCode,
                                            AccountCategory = d.AccountCategory,
                                            IsLocked = d.IsLocked,
                                            CreatedById = d.CreatedById,
                                            CreatedBy = d.MstUser.UserName,
                                            CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                            UpdatedById = d.UpdatedById,
                                            UpdatedBy = d.MstUser1.UserName,
                                            UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                        };
            return accountCategories.ToList();
        }

        // ====================
        // ADD Account Category
        // ====================
        [Route("api/addAccountCategory")]
        public int Post(Models.MstAccountCategory accountCategory)
        {
            try
            {
                var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.MstAccountCategory newAccountCategory = new Data.MstAccountCategory();

                newAccountCategory.AccountCategoryCode = accountCategory.AccountCategoryCode;
                newAccountCategory.AccountCategory = accountCategory.AccountCategory;
                newAccountCategory.IsLocked = isLocked;
                newAccountCategory.CreatedById = mstUserId;
                newAccountCategory.CreatedDateTime = date;
                newAccountCategory.UpdatedById = mstUserId;
                newAccountCategory.UpdatedDateTime = date;

                db.MstAccountCategories.InsertOnSubmit(newAccountCategory);
                db.SubmitChanges();

                return newAccountCategory.Id;
            }
            catch
            {
                return 0;
            }
        }

        // =======================
        // UPDATE Account Category
        // =======================
        [Route("api/updateAccountCategory/{id}")]
        public HttpResponseMessage Put(String id, Models.MstAccountCategory accountCategory)
        {
            try
            {
                var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var accountCategoryId = Convert.ToInt32(id);
                var accountCategories = from d in db.MstAccountCategories where d.Id == accountCategoryId select d;

                if (accountCategories.Any())
                {
                    var updateAccountCategory = accountCategories.FirstOrDefault();

                    updateAccountCategory.AccountCategoryCode = accountCategory.AccountCategoryCode;
                    updateAccountCategory.AccountCategory = accountCategory.AccountCategory;
                    updateAccountCategory.IsLocked = isLocked;
                    updateAccountCategory.UpdatedById = mstUserId;
                    updateAccountCategory.UpdatedDateTime = date;

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

        // =======================
        // DELETE Account Category
        // =======================
        [Route("api/deleteAccountCategory/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var accountCategoryId = Convert.ToInt32(id);
                var accountCategories = from d in db.MstAccountCategories where d.Id == accountCategoryId select d;

                if (accountCategories.Any())
                {
                    db.MstAccountCategories.DeleteOnSubmit(accountCategories.First());
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
