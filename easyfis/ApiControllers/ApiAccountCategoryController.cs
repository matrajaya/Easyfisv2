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

        // list account category
        [Authorize]
        [HttpGet]
        [Route("api/listAccountCategory")]
        public List<Models.MstAccountCategory> listAccountCategory()
        {
            var accountCategories = from d in db.MstAccountCategories.OrderBy(d => d.AccountCategory)
                                    select new Models.MstAccountCategory
                                        {
                                            Id = d.Id,
                                            AccountCategoryCode = d.AccountCategoryCode,
                                            AccountCategory = d.AccountCategory,
                                            IsLocked = d.IsLocked,
                                            CreatedById = d.CreatedById,
                                            CreatedBy = d.MstUser.FullName,
                                            CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                            UpdatedById = d.UpdatedById,
                                            UpdatedBy = d.MstUser1.FullName,
                                            UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                        };

            return accountCategories.ToList();
        }

        // add account category
        [Authorize]
        [HttpPost]
        [Route("api/addAccountCategory")]
        public Int32 insertAccountCategory(Models.MstAccountCategory accountCategory)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.MstAccountCategory newAccountCategory = new Data.MstAccountCategory();
                newAccountCategory.AccountCategoryCode = accountCategory.AccountCategoryCode;
                newAccountCategory.AccountCategory = accountCategory.AccountCategory;
                newAccountCategory.IsLocked = accountCategory.IsLocked;
                newAccountCategory.CreatedById = userId;
                newAccountCategory.CreatedDateTime = DateTime.Now;
                newAccountCategory.UpdatedById = userId;
                newAccountCategory.UpdatedDateTime = DateTime.Now;

                db.MstAccountCategories.InsertOnSubmit(newAccountCategory);
                db.SubmitChanges();

                return newAccountCategory.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update account category
        [Authorize]
        [HttpPut]
        [Route("api/updateAccountCategory/{id}")]
        public HttpResponseMessage updateAccountCategory(String id, Models.MstAccountCategory accountCategory)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var accountCategories = from d in db.MstAccountCategories where d.Id == Convert.ToInt32(id) select d;
                if (accountCategories.Any())
                {
                    var updateAccountCategory = accountCategories.FirstOrDefault();
                    updateAccountCategory.AccountCategoryCode = accountCategory.AccountCategoryCode;
                    updateAccountCategory.AccountCategory = accountCategory.AccountCategory;
                    updateAccountCategory.IsLocked = accountCategory.IsLocked;
                    updateAccountCategory.UpdatedById = userId;
                    updateAccountCategory.UpdatedDateTime = DateTime.Now;

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

        // delete account category
        [Authorize]
        [HttpDelete]
        [Route("api/deleteAccountCategory/{id}")]
        public HttpResponseMessage deleteAccountCategory(String id)
        {
            try
            {
                var accountCategories = from d in db.MstAccountCategories where d.Id == Convert.ToInt32(id) select d;
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
