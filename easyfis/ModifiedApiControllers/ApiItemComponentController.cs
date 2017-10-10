using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace easyfis.ModifiedApiControllers
{
    public class ApiItemComponentController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ================
        // Get Highest Cost
        // ================
        public Decimal GetHighestCost(Int32 itemId)
        {
            var articleInventories = from d in db.MstArticleInventories.OrderByDescending(d => d.Cost)
                                     where d.ArticleId == itemId
                                     select d;

            if (articleInventories.Any())
            {
                return articleInventories.FirstOrDefault().Cost;
            }
            else
            {
                return 0;
            }
        }

        // =============================================
        // Dropdown List - Item Component - Item (Field)
        // =============================================
        [Authorize, HttpGet, Route("api/itemComponent/dropdown/list/item")]
        public List<Entities.MstArticle> DropdownListItemComponentItem()
        {
            var itemComponentItem = from d in db.MstArticles.OrderBy(d => d.Article)
                                    where d.IsLocked == true
                                    select new Entities.MstArticle
                                    {
                                        Id = d.Id,
                                        Article = d.Article
                                    };

            return itemComponentItem.ToList();
        }

        // ===================
        // List Item Component
        // ===================
        [Authorize, HttpGet, Route("api/itemComponent/list/{itemId}")]
        public List<Entities.MstArticleComponent> ListItemComponent()
        {
            var articleComponents = from d in db.MstArticleComponents
                                    select new Entities.MstArticleComponent
                                    {
                                        Id = d.Id,
                                        ComponentArticleId = d.ComponentArticleId,
                                        ComponentArticleManualCode = d.MstArticle1.ManualArticleCode,
                                        ComponentArticle = d.MstArticle1.Article,
                                        Quantity = d.Quantity,
                                        Unit = d.MstArticle1.MstUnit.Unit,
                                        Cost = GetHighestCost(d.ArticleId),
                                        Amount = GetHighestCost(d.ArticleId) * d.Quantity,
                                        Particulars = d.Particulars
                                    };

            return articleComponents.ToList();
        }

        // ==================
        // Add Item Component
        // ==================
        [Authorize, HttpPost, Route("api/itemComponent/add/{itemId}")]
        public HttpResponseMessage AddItemComponent(Entities.MstArticleComponent objItemComponent, String itemId)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;

                    var userForms = from d in db.MstUserForms
                                    where d.UserId == currentUserId
                                    && d.SysForm.FormName.Equals("ItemDetail")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanAdd)
                        {
                            var item = from d in db.MstArticles
                                       where d.Id == Convert.ToInt32(itemId)
                                       && d.ArticleTypeId == 1
                                       select d;

                            if (item.Any())
                            {
                                if (!item.FirstOrDefault().IsLocked)
                                {
                                    Data.MstArticleComponent newItemComponent = new Data.MstArticleComponent
                                    {
                                        ArticleId = Convert.ToInt32(itemId),
                                        ComponentArticleId = objItemComponent.ComponentArticleId,
                                        Quantity = objItemComponent.Quantity,
                                        Particulars = objItemComponent.Particulars
                                    };

                                    db.MstArticleComponents.InsertOnSubmit(newItemComponent);
                                    db.SubmitChanges();

                                    return Request.CreateResponse(HttpStatusCode.OK);
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot add new component if the current item detail is locked.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "These current item details are not found in the server. Please add new item first before proceeding.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to add new component in this item detail page.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this item detail page.");
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

        // =====================
        // Update Item Component
        // =====================
        [Authorize, HttpPut, Route("api/itemComponent/update/{id}/{itemId}")]
        public HttpResponseMessage UpdateItemComponent(Entities.MstArticleComponent objItemComponent, String id, String itemId)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;

                    var userForms = from d in db.MstUserForms
                                    where d.UserId == currentUserId
                                    && d.SysForm.FormName.Equals("ItemDetail")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanEdit)
                        {
                            var item = from d in db.MstArticles
                                       where d.Id == Convert.ToInt32(itemId)
                                       && d.ArticleTypeId == 1
                                       select d;

                            if (item.Any())
                            {
                                if (!item.FirstOrDefault().IsLocked)
                                {
                                    var itemComponent = from d in db.MstArticleComponents
                                                        where d.Id == Convert.ToInt32(id)
                                                        select d;

                                    if (itemComponent.Any())
                                    {
                                        var updateItemComponent = itemComponent.FirstOrDefault();
                                        updateItemComponent.ArticleId = objItemComponent.ArticleId;
                                        updateItemComponent.ComponentArticleId = objItemComponent.ComponentArticleId;
                                        updateItemComponent.Quantity = objItemComponent.Quantity;
                                        updateItemComponent.Particulars = objItemComponent.Particulars;
                                        db.SubmitChanges();

                                        return Request.CreateResponse(HttpStatusCode.OK);
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.NotFound, "This component detail is no longer exist in the server.");
                                    }
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot update component if the current item detail is locked.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "These current item details are not found in the server. Please add new item first before proceeding.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to update component in this item detail page.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this item detail page.");
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

        // =====================
        // Delete Item Component
        // =====================
        [Authorize, HttpDelete, Route("api/itemComponent/delete/{id}/{itemId}")]
        public HttpResponseMessage DeleteItemComponent(String id, String itemId)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;

                    var userForms = from d in db.MstUserForms
                                    where d.UserId == currentUserId
                                    && d.SysForm.FormName.Equals("ItemDetail")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanDelete)
                        {
                            var item = from d in db.MstArticles
                                       where d.Id == Convert.ToInt32(itemId)
                                       && d.ArticleTypeId == 1
                                       select d;

                            if (item.Any())
                            {
                                if (!item.FirstOrDefault().IsLocked)
                                {
                                    var itemComponent = from d in db.MstArticleComponents
                                                        where d.Id == Convert.ToInt32(id)
                                                        select d;

                                    if (itemComponent.Any())
                                    {
                                        db.MstArticleComponents.DeleteOnSubmit(itemComponent.First());
                                        db.SubmitChanges();

                                        return Request.CreateResponse(HttpStatusCode.OK);
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.NotFound, "This component detail is no longer exist in the server.");
                                    }
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "You cannot delete component if the current item detail is locked.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "These current item details are not found in the server. Please add new item first before proceeding.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to delete component in this item detail page.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this item detail page.");
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
