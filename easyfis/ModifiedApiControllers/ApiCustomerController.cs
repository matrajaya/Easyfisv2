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
    public class ApiCustomerController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =============
        // List Customer
        // =============
        [Authorize, HttpGet, Route("api/customer/list")]
        public List<Entities.MstArticle> ListCustomer()
        {
            var customers = from d in db.MstArticles.OrderByDescending(d => d.ArticleCode)
                            where d.ArticleTypeId == 2
                            select new Entities.MstArticle
                            {
                                Id = d.Id,
                                ArticleCode = d.ArticleCode,
                                ManualArticleCode = d.ManualArticleCode,
                                Article = d.Article,
                                ArticleGroupId = d.ArticleGroupId,
                                ArticleGroup = d.MstArticleGroup.ArticleGroup,
                                ContactNumber = d.ContactNumber,
                                IsLocked = d.IsLocked,
                                CreatedById = d.CreatedById,
                                CreatedBy = d.MstUser.FullName,
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedById = d.UpdatedById,
                                UpdatedBy = d.MstUser1.FullName,
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                            };

            return customers.ToList();
        }

        // ======================================
        // Dropdown List - Customer Group (Field)
        // ======================================
        [Authorize, HttpGet, Route("api/customer/dropdown/list/customerGroup")]
        public List<Entities.MstArticleGroup> DropdownListCustomerGroup()
        {
            var customerGroups = from d in db.MstArticleGroups.OrderBy(d => d.ArticleGroup)
                                 where d.ArticleTypeId == 2
                                 && d.IsLocked == true
                                 select new Entities.MstArticleGroup
                                 {
                                     Id = d.Id,
                                     ArticleGroup = d.ArticleGroup,
                                     AccountId = d.AccountId,
                                     SalesAccountId = d.SalesAccountId,
                                     CostAccountId = d.CostAccountId,
                                     AssetAccountId = d.AssetAccountId,
                                     ExpenseAccountId = d.ExpenseAccountId
                                 };

            return customerGroups.ToList();
        }

        // ==============================================
        // Dropdown List - Customer Group Account (Field)
        // ==============================================
        [Authorize, HttpGet, Route("api/customer/dropdown/list/customerGroup/account/{accountId}")]
        public List<Entities.MstAccount> DropdownListCustomerGroupAccount(String accountId)
        {
            var customerGroupAccounts = from d in db.MstAccounts.OrderBy(d => d.Account)
                                        where d.Id == Convert.ToInt32(accountId)
                                        && d.IsLocked == true
                                        select new Entities.MstAccount
                                        {
                                            Id = d.Id,
                                            AccountCode = d.AccountCode,
                                            Account = d.Account
                                        };

            return customerGroupAccounts.ToList();
        }

        // =====================================
        // Dropdown List - Customer Term (Field)
        // =====================================
        [Authorize, HttpGet, Route("api/customer/dropdown/list/term")]
        public List<Entities.MstTerm> ListCustomerTerm()
        {
            var customerTerms = from d in db.MstTerms.OrderBy(d => d.Term)
                                select new Entities.MstTerm
                                {
                                    Id = d.Id,
                                    Term = d.Term
                                };

            return customerTerms.ToList();
        }

        // ===============
        // Detail Customer
        // ===============
        [Authorize, HttpGet, Route("api/customer/detail/{id}")]
        public Entities.MstArticle DetailCustomer(String id)
        {
            var customer = from d in db.MstArticles.OrderBy(d => d.ArticleCode)
                           where d.Id == Convert.ToInt32(id)
                           && d.ArticleTypeId == 2
                           select new Entities.MstArticle
                           {
                               Id = d.Id,
                               ArticleCode = d.ArticleCode,
                               ManualArticleCode = d.ManualArticleCode,
                               Article = d.Article,
                               Category = d.Category,
                               ArticleTypeId = d.ArticleTypeId,
                               ArticleGroupId = d.ArticleGroupId,
                               AccountId = d.AccountId,
                               SalesAccountId = d.SalesAccountId,
                               CostAccountId = d.CostAccountId,
                               AssetAccountId = d.AssetAccountId,
                               ExpenseAccountId = d.ExpenseAccountId,
                               Particulars = d.Particulars,
                               Address = d.Address,
                               TermId = d.TermId,
                               ContactNumber = d.ContactNumber,
                               ContactPerson = d.ContactPerson,
                               EmailAddress = d.EmailAddress,
                               TaxNumber = d.TaxNumber,
                               IsLocked = d.IsLocked,
                               CreatedById = d.CreatedById,
                               CreatedBy = d.MstUser.FullName,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedById = d.UpdatedById,
                               UpdatedBy = d.MstUser1.FullName,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };

            return customer.FirstOrDefault();
        }

        // ===================
        // Fill Leading Zeroes
        // ===================
        public String FillLeadingZeroes(Int32 number, Int32 length)
        {
            var result = number.ToString();
            var pad = length - result.Length;
            while (pad > 0)
            {
                result = '0' + result;
                pad--;
            }

            return result;
        }

        // ============
        // Add Customer
        // ============
        [Authorize, HttpPost, Route("api/customer/add")]
        public HttpResponseMessage AddCustomer()
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
                                    && d.SysForm.FormName.Equals("CustomerList")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanAdd)
                        {
                            var defaultCustomerCode = "0000000001";
                            var lastCustomer = from d in db.MstArticles.OrderByDescending(d => d.Id)
                                               where d.ArticleTypeId == 2
                                               select d;

                            if (lastCustomer.Any())
                            {
                                var customerCode = Convert.ToInt32(lastCustomer.FirstOrDefault().ArticleCode) + 0000000001;
                                defaultCustomerCode = FillLeadingZeroes(customerCode, 10);
                            }

                            var articleGroups = from d in db.MstArticleGroups
                                                where d.ArticleTypeId == 2
                                                select d;

                            if (articleGroups.Any())
                            {
                                var units = from d in db.MstUnits
                                            select d;

                                if (units.Any())
                                {
                                    var taxTypes = from d in db.MstTaxTypes
                                                   select d;

                                    if (taxTypes.Any())
                                    {
                                        var terms = from d in db.MstTerms
                                                    select d;

                                        if (terms.Any())
                                        {
                                            Data.MstArticle newCustomer = new Data.MstArticle
                                            {
                                                ArticleCode = defaultCustomerCode,
                                                ManualArticleCode = "NA",
                                                Article = "NA",
                                                Category = "NA",
                                                ArticleTypeId = 2,
                                                ArticleGroupId = articleGroups.FirstOrDefault().Id,
                                                AccountId = articleGroups.FirstOrDefault().AccountId,
                                                SalesAccountId = articleGroups.FirstOrDefault().SalesAccountId,
                                                CostAccountId = articleGroups.FirstOrDefault().CostAccountId,
                                                AssetAccountId = articleGroups.FirstOrDefault().AssetAccountId,
                                                ExpenseAccountId = articleGroups.FirstOrDefault().ExpenseAccountId,
                                                UnitId = units.FirstOrDefault().Id,
                                                OutputTaxId = db.MstTaxTypes.FirstOrDefault().Id,
                                                InputTaxId = db.MstTaxTypes.FirstOrDefault().Id,
                                                WTaxTypeId = db.MstTaxTypes.FirstOrDefault().Id,
                                                Price = 0,
                                                Cost = 0,
                                                IsInventory = false,
                                                Particulars = "NA",
                                                Address = "NA",
                                                TermId = terms.FirstOrDefault().Id,
                                                ContactNumber = "NA",
                                                ContactPerson = "NA",
                                                EmailAddress = "NA",
                                                TaxNumber = "NA",
                                                CreditLimit = 0,
                                                DateAcquired = DateTime.Now,
                                                UsefulLife = 0,
                                                SalvageValue = 0,
                                                ManualArticleOldCode = "NA",
                                                Kitting = 0,
                                                IsLocked = false,
                                                CreatedById = currentUserId,
                                                CreatedDateTime = DateTime.Now,
                                                UpdatedById = currentUserId,
                                                UpdatedDateTime = DateTime.Now
                                            };

                                            db.MstArticles.InsertOnSubmit(newCustomer);
                                            db.SubmitChanges();

                                            return Request.CreateResponse(HttpStatusCode.OK, newCustomer.Id);
                                        }
                                        else
                                        {
                                            return Request.CreateResponse(HttpStatusCode.NotFound, "No term found. Please setup more terms for all master tables.");
                                        }
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.NotFound, "No tax type found. Please setup more tax types for all master tables.");
                                    }
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.NotFound, "No unit found. Please setup more units for all master tables.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "No article group found. Please setup at least one article group for customers.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to add customer.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access for this customer page.");
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

        // =============
        // Lock Customer
        // =============
        [Authorize, HttpPut, Route("api/customer/lock/{id}")]
        public HttpResponseMessage LockCustomer(Entities.MstArticle objCustomer, String id)
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
                                    && d.SysForm.FormName.Equals("CustomerDetail")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanLock)
                        {
                            var customer = from d in db.MstArticles
                                           where d.Id == Convert.ToInt32(id)
                                           && d.ArticleTypeId == 2
                                           select d;

                            if (customer.Any())
                            {
                                if (!customer.FirstOrDefault().IsLocked)
                                {
                                    var lockCustomer = customer.FirstOrDefault();
                                    lockCustomer.ManualArticleCode = objCustomer.ManualArticleCode;
                                    lockCustomer.Article = objCustomer.Article;
                                    lockCustomer.ArticleGroupId = objCustomer.ArticleGroupId;
                                    lockCustomer.AccountId = objCustomer.AccountId;
                                    lockCustomer.SalesAccountId = objCustomer.SalesAccountId;
                                    lockCustomer.CostAccountId = objCustomer.CostAccountId;
                                    lockCustomer.AssetAccountId = objCustomer.AssetAccountId;
                                    lockCustomer.ExpenseAccountId = objCustomer.ExpenseAccountId;
                                    lockCustomer.TermId = objCustomer.TermId;
                                    lockCustomer.Address = objCustomer.Address;
                                    lockCustomer.ContactNumber = objCustomer.ContactNumber;
                                    lockCustomer.ContactPerson = objCustomer.ContactPerson;
                                    lockCustomer.TaxNumber = objCustomer.TaxNumber;
                                    lockCustomer.Particulars = objCustomer.Particulars;
                                    lockCustomer.EmailAddress = objCustomer.EmailAddress;
                                    lockCustomer.IsLocked = true;
                                    lockCustomer.UpdatedById = currentUserId;
                                    lockCustomer.UpdatedDateTime = DateTime.Now;

                                    db.SubmitChanges();

                                    return Request.CreateResponse(HttpStatusCode.OK);
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Locking Error. These customer details are already locked.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "Data not found. These customer details are not found in the server.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to lock customer.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access for this customer detail page.");
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

        // ===============
        // Unlock Customer
        // ===============
        [Authorize, HttpPut, Route("api/customer/unlock/{id}")]
        public HttpResponseMessage UnlockCustomer(String id)
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
                                    && d.SysForm.FormName.Equals("CustomerDetail")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanUnlock)
                        {
                            var customer = from d in db.MstArticles
                                           where d.Id == Convert.ToInt32(id)
                                           && d.ArticleTypeId == 2
                                           select d;

                            if (customer.Any())
                            {
                                if (customer.FirstOrDefault().IsLocked)
                                {
                                    var unlockCustomer = customer.FirstOrDefault();
                                    unlockCustomer.IsLocked = false;
                                    unlockCustomer.UpdatedById = currentUserId;
                                    unlockCustomer.UpdatedDateTime = DateTime.Now;

                                    db.SubmitChanges();

                                    return Request.CreateResponse(HttpStatusCode.OK);
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Unlocking Error. These customer details are already unlocked.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "Data not found. These customer details are not found in the server.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to unlock customer.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access for this customer detail page.");
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

        // ===============
        // Delete Customer
        // ===============
        [Authorize, HttpDelete, Route("api/customer/delete/{id}")]
        public HttpResponseMessage DeleteCustomer(String id)
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
                                    && d.SysForm.FormName.Equals("CustomerList")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanDelete)
                        {
                            var customer = from d in db.MstArticles
                                           where d.Id == Convert.ToInt32(id)
                                           && d.ArticleTypeId == 2
                                           select d;

                            if (customer.Any())
                            {
                                db.MstArticles.DeleteOnSubmit(customer.First());
                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "Data not found. This selected customer is not found in the server.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to delete customer.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access for this customer page.");
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
