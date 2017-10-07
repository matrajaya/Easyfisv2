using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.ModifiedApiControllers
{
    public class ApiMoreContactInformationController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =============================
        // List More Contact Information
        // =============================
        [Authorize, HttpGet, Route("api/moreContactInformation/list/{articleId}")]
        public List<Entities.MstArticleContact> ListMoreContactInformation(String articleId)
        {
            var moreContactInformations = from d in db.MstArticleContacts
                                          where d.ArticleId == Convert.ToInt32(articleId)
                                          select new Entities.MstArticleContact
                                          {
                                              Id = d.Id,
                                              ContactPerson = d.ContactPerson,
                                              ContactNumber = d.ContactNumber,
                                              Remarks = d.Remarks
                                          };

            return moreContactInformations.ToList();
        }

        // ============================
        // Add More Contact Information
        // ============================
        [Authorize, HttpPost, Route("api/moreContactInformation/add/{articleId}")]
        public HttpResponseMessage AddMoreContactInformation(Entities.MstArticleContact objContactInformation, String articleId)
        {
            try
            {
                String supplierDetailURL = "/Software/SupplierDetail";
                String customerDetailURL = "/Software/CustomerDetail";

                String currentAbsolutePath = HttpContext.Current.Request.UrlReferrer.AbsolutePath;

                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;

                    IQueryable<Data.MstUserForm> userForms = null;
                    if (currentAbsolutePath.Equals(supplierDetailURL))
                    {
                        userForms = from d in db.MstUserForms
                                    where d.UserId == currentUserId
                                    && d.SysForm.FormName.Equals("SupplierDetail")
                                    select d;
                    }
                    else
                    {
                        if (currentAbsolutePath.Equals(customerDetailURL))
                        {
                            userForms = from d in db.MstUserForms
                                        where d.UserId == currentUserId
                                        && d.SysForm.FormName.Equals("CustomerDetail")
                                        select d;
                        }
                        else
                        {
                            userForms = null;
                        }
                    }

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanAdd)
                        {

                            var article = from d in db.MstArticles
                                          where d.Id == Convert.ToInt32(articleId)
                                          select d;

                            if (article.Any())
                            {
                                if (!article.FirstOrDefault().IsLocked)
                                {
                                    Data.MstArticleContact newContact = new Data.MstArticleContact
                                    {
                                        ArticleId = Convert.ToInt32(articleId),
                                        ContactPerson = objContactInformation.ContactPerson,
                                        ContactNumber = objContactInformation.ContactNumber,
                                        Remarks = objContactInformation.Remarks
                                    };

                                    db.MstArticleContacts.InsertOnSubmit(newContact);
                                    db.SubmitChanges();

                                    return Request.CreateResponse(HttpStatusCode.OK);
                                }
                                else
                                {
                                    if (currentAbsolutePath.Equals(supplierDetailURL))
                                    {
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot add new contact information if the current supplier detail is locked.");
                                    }
                                    else
                                    {
                                        if (currentAbsolutePath.Equals(customerDetailURL))
                                        {
                                            return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot add new contact information if the current customer detail is locked.");
                                        }
                                        else
                                        {
                                            return Request.CreateResponse(HttpStatusCode.NotFound, "URL Not Found.");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (currentAbsolutePath.Equals(supplierDetailURL))
                                {
                                    return Request.CreateResponse(HttpStatusCode.NotFound, "These current supplier details are not found in the server. Please add new supplier first before proceeding.");
                                }
                                else
                                {
                                    if (currentAbsolutePath.Equals(customerDetailURL))
                                    {
                                        return Request.CreateResponse(HttpStatusCode.NotFound, "These current supplier details are not found in the server. Please add new supplier first before proceeding.");
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.NotFound, "URL Not Found.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (currentAbsolutePath.Equals(supplierDetailURL))
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to add new contact information in this supplier detail page.");
                            }
                            else
                            {
                                if (currentAbsolutePath.Equals(customerDetailURL))
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to add new contact information in this customer detail page.");
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.NotFound, "URL Not Found.");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (currentAbsolutePath.Equals(supplierDetailURL))
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this supplier detail page.");
                        }
                        else
                        {
                            if (currentAbsolutePath.Equals(customerDetailURL))
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this customer detail page.");
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "URL Not Found.");
                            }
                        }
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Theres no current user logged in.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }

        // ===============================
        // Update More Contact Information
        // ===============================
        [Authorize, HttpPut, Route("api/moreContactInformation/update/{id}/{articleId}")]
        public HttpResponseMessage UpdateMoreContactInformation(Entities.MstArticleContact objContactInformation, String id, String articleId)
        {
            try
            {
                String supplierDetailURL = "/Software/SupplierDetail";
                String customerDetailURL = "/Software/CustomerDetail";

                String currentAbsolutePath = HttpContext.Current.Request.UrlReferrer.AbsolutePath;

                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;

                    IQueryable<Data.MstUserForm> userForms = null;
                    if (currentAbsolutePath.Equals(supplierDetailURL))
                    {
                        userForms = from d in db.MstUserForms
                                    where d.UserId == currentUserId
                                    && d.SysForm.FormName.Equals("SupplierDetail")
                                    select d;
                    }
                    else
                    {
                        if (currentAbsolutePath.Equals(customerDetailURL))
                        {
                            userForms = from d in db.MstUserForms
                                        where d.UserId == currentUserId
                                        && d.SysForm.FormName.Equals("CustomerDetail")
                                        select d;
                        }
                        else
                        {
                            userForms = null;
                        }
                    }

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanEdit)
                        {
                            var article = from d in db.MstArticles
                                          where d.Id == Convert.ToInt32(articleId)
                                          select d;

                            if (article.Any())
                            {
                                if (!article.FirstOrDefault().IsLocked)
                                {
                                    var moreContactInformation = from d in db.MstArticleContacts
                                                                 where d.Id == Convert.ToInt32(id)
                                                                 && d.ArticleId == Convert.ToInt32(articleId)
                                                                 select d;

                                    if (moreContactInformation.Any())
                                    {
                                        var updateMoreContactInformation = moreContactInformation.FirstOrDefault();
                                        updateMoreContactInformation.ContactPerson = objContactInformation.ContactPerson;
                                        updateMoreContactInformation.ContactNumber = objContactInformation.ContactNumber;
                                        updateMoreContactInformation.Remarks = objContactInformation.Remarks;
                                        db.SubmitChanges();

                                        return Request.CreateResponse(HttpStatusCode.OK);
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.NotFound, "This contact information detail is no longer exist in the server.");
                                    }

                                }
                                else
                                {
                                    if (currentAbsolutePath.Equals(supplierDetailURL))
                                    {
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot edit and update a contact information detail if the current supplier detail is locked.");
                                    }
                                    else
                                    {
                                        if (currentAbsolutePath.Equals(customerDetailURL))
                                        {
                                            return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot edit and update a contact information detail if the current customer detail is locked.");
                                        }
                                        else
                                        {
                                            return Request.CreateResponse(HttpStatusCode.NotFound, "URL Not Found.");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (currentAbsolutePath.Equals(supplierDetailURL))
                                {
                                    return Request.CreateResponse(HttpStatusCode.NotFound, "These current supplier details are not found in the server. Please add new supplier first before proceeding.");
                                }
                                else
                                {
                                    if (currentAbsolutePath.Equals(customerDetailURL))
                                    {
                                        return Request.CreateResponse(HttpStatusCode.NotFound, "These current supplier details are not found in the server. Please add new supplier first before proceeding.");
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.NotFound, "URL Not Found.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (currentAbsolutePath.Equals(supplierDetailURL))
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to edit and update contact information in this supplier detail page.");
                            }
                            else
                            {
                                if (currentAbsolutePath.Equals(customerDetailURL))
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to edit and update contact information in this customer detail page.");
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.NotFound, "URL Not Found.");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (currentAbsolutePath.Equals(supplierDetailURL))
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this supplier detail page.");
                        }
                        else
                        {
                            if (currentAbsolutePath.Equals(customerDetailURL))
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this customer detail page.");
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "URL Not Found.");
                            }
                        }
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Theres no current user logged in.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }

        // ===============================
        // Delete More Contact Information
        // ===============================
        [Authorize, HttpDelete, Route("api/moreContactInformation/delete/{id}/{articleId}")]
        public HttpResponseMessage DeleteMoreContactInformation(String id, String articleId)
        {
            try
            {
                String supplierDetailURL = "/Software/SupplierDetail";
                String customerDetailURL = "/Software/CustomerDetail";

                String currentAbsolutePath = HttpContext.Current.Request.UrlReferrer.AbsolutePath;

                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;

                    IQueryable<Data.MstUserForm> userForms = null;
                    if (currentAbsolutePath.Equals(supplierDetailURL))
                    {
                        userForms = from d in db.MstUserForms
                                    where d.UserId == currentUserId
                                    && d.SysForm.FormName.Equals("SupplierDetail")
                                    select d;
                    }
                    else
                    {
                        if (currentAbsolutePath.Equals(customerDetailURL))
                        {
                            userForms = from d in db.MstUserForms
                                        where d.UserId == currentUserId
                                        && d.SysForm.FormName.Equals("CustomerDetail")
                                        select d;
                        }
                        else
                        {
                            userForms = null;
                        }
                    }

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanDelete)
                        {
                            var article = from d in db.MstArticles
                                          where d.Id == Convert.ToInt32(articleId)
                                          select d;

                            if (article.Any())
                            {
                                if (!article.FirstOrDefault().IsLocked)
                                {
                                    var moreContactInformation = from d in db.MstArticleContacts
                                                                 where d.Id == Convert.ToInt32(id)
                                                                 && d.ArticleId == Convert.ToInt32(articleId)
                                                                 select d;

                                    if (moreContactInformation.Any())
                                    {
                                        db.MstArticleContacts.DeleteOnSubmit(moreContactInformation.First());
                                        db.SubmitChanges();

                                        return Request.CreateResponse(HttpStatusCode.OK);
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.NotFound, "This contact information detail is no longer exist in the server.");
                                    }

                                }
                                else
                                {
                                    if (currentAbsolutePath.Equals(supplierDetailURL))
                                    {
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot delete a contact information detail if the current supplier detail is locked.");
                                    }
                                    else
                                    {
                                        if (currentAbsolutePath.Equals(customerDetailURL))
                                        {
                                            return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot delete a contact information detail if the current customer detail is locked.");
                                        }
                                        else
                                        {
                                            return Request.CreateResponse(HttpStatusCode.NotFound, "URL Not Found.");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (currentAbsolutePath.Equals(supplierDetailURL))
                                {
                                    return Request.CreateResponse(HttpStatusCode.NotFound, "These current supplier details are not found in the server. Please add new supplier first before proceeding.");
                                }
                                else
                                {
                                    if (currentAbsolutePath.Equals(customerDetailURL))
                                    {
                                        return Request.CreateResponse(HttpStatusCode.NotFound, "These current supplier details are not found in the server. Please add new supplier first before proceeding.");
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.NotFound, "URL Not Found.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (currentAbsolutePath.Equals(supplierDetailURL))
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to delete contact information in this supplier detail page.");
                            }
                            else
                            {
                                if (currentAbsolutePath.Equals(customerDetailURL))
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to delete contact information in this customer detail page.");
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.NotFound, "URL Not Found.");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (currentAbsolutePath.Equals(supplierDetailURL))
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this supplier detail page.");
                        }
                        else
                        {
                            if (currentAbsolutePath.Equals(customerDetailURL))
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this customer detail page.");
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "URL Not Found.");
                            }
                        }
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Theres no current user logged in.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }
    }
}
