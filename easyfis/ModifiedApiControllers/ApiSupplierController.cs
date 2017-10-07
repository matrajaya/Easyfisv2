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
    public class ApiSupplierController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =============
        // List Supplier
        // =============
        [Authorize, HttpGet, Route("api/supplier/list")]
        public List<Entities.MstArticle> ListSupplier()
        {
            var suppliers = from d in db.MstArticles.OrderByDescending(d => d.ArticleCode)
                            where d.ArticleTypeId == 3
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

            return suppliers.ToList();
        }

        // ======================================
        // Dropdown List - Supplier Group (Field)
        // ======================================
        [Authorize, HttpGet, Route("api/supplier/dropdown/list/supplierGroup")]
        public List<Entities.MstArticleGroup> DropdownListSupplierGroup()
        {
            var supplierGroups = from d in db.MstArticleGroups.OrderBy(d => d.ArticleGroup)
                                 where d.ArticleTypeId == 3
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

            return supplierGroups.ToList();
        }

        // ==============================================
        // Dropdown List - Supplier Group Account (Field)
        // ==============================================
        [Authorize, HttpGet, Route("api/supplier/dropdown/list/supplierGroup/account/{accountId}")]
        public List<Entities.MstAccount> DropdownListSupplierGroupAccount(String accountId)
        {
            var supplierGroupAccounts = from d in db.MstAccounts.OrderBy(d => d.Account)
                                        where d.Id == Convert.ToInt32(accountId)
                                        && d.IsLocked == true
                                        select new Entities.MstAccount
                                        {
                                            Id = d.Id,
                                            AccountCode = d.AccountCode,
                                            Account = d.Account
                                        };

            return supplierGroupAccounts.ToList();
        }

        // =====================================
        // Dropdown List - Supplier Term (Field)
        // =====================================
        [Authorize, HttpGet, Route("api/supplier/dropdown/list/term")]
        public List<Entities.MstTerm> ListSupplierTerm()
        {
            var supplierTerms = from d in db.MstTerms.OrderBy(d => d.Term)
                                select new Entities.MstTerm
                                {
                                    Id = d.Id,
                                    Term = d.Term
                                };

            return supplierTerms.ToList();
        }

        // ===============
        // Detail Supplier
        // ===============
        [Authorize, HttpGet, Route("api/supplier/detail/{id}")]
        public Entities.MstArticle DetailSupplier(String id)
        {
            var supplier = from d in db.MstArticles
                           where d.Id == Convert.ToInt32(id)
                           && d.ArticleTypeId == 3
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

            return supplier.FirstOrDefault();
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
        // Add Supplier
        // ============
        [Authorize, HttpPost, Route("api/supplier/add")]
        public HttpResponseMessage AddSupplier()
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
                                    && d.SysForm.FormName.Equals("SupplierList")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanAdd)
                        {
                            var defaultSupplierCode = "0000000001";
                            var lastSupplier = from d in db.MstArticles.OrderByDescending(d => d.Id)
                                               where d.ArticleTypeId == 3
                                               select d;

                            if (lastSupplier.Any())
                            {
                                var supplierCode = Convert.ToInt32(lastSupplier.FirstOrDefault().ArticleCode) + 0000000001;
                                defaultSupplierCode = FillLeadingZeroes(supplierCode, 10);
                            }

                            var articleGroups = from d in db.MstArticleGroups
                                                where d.ArticleTypeId == 3
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
                                            Data.MstArticle newSupplier = new Data.MstArticle
                                            {
                                                ArticleCode = defaultSupplierCode,
                                                ManualArticleCode = "NA",
                                                Article = "NA",
                                                Category = "NA",
                                                ArticleTypeId = 3,
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

                                            db.MstArticles.InsertOnSubmit(newSupplier);
                                            db.SubmitChanges();

                                            return Request.CreateResponse(HttpStatusCode.OK, newSupplier.Id);
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
                                return Request.CreateResponse(HttpStatusCode.NotFound, "No article group found. Please setup at least one article group for suppliers.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to add supplier.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access for this supplier page.");
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
        // Lock Supplier
        // =============
        [Authorize, HttpPut, Route("api/supplier/lock/{id}")]
        public HttpResponseMessage LockSupplier(Entities.MstArticle objSupplier, String id)
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
                                    && d.SysForm.FormName.Equals("SupplierDetail")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanLock)
                        {
                            var supplier = from d in db.MstArticles
                                           where d.Id == Convert.ToInt32(id)
                                           && d.ArticleTypeId == 3
                                           select d;

                            if (supplier.Any())
                            {
                                if (!supplier.FirstOrDefault().IsLocked)
                                {
                                    var lockSupplier = supplier.FirstOrDefault();
                                    lockSupplier.ManualArticleCode = objSupplier.ManualArticleCode;
                                    lockSupplier.Article = objSupplier.Article;
                                    lockSupplier.ArticleGroupId = objSupplier.ArticleGroupId;
                                    lockSupplier.AccountId = objSupplier.AccountId;
                                    lockSupplier.SalesAccountId = objSupplier.SalesAccountId;
                                    lockSupplier.CostAccountId = objSupplier.CostAccountId;
                                    lockSupplier.AssetAccountId = objSupplier.AssetAccountId;
                                    lockSupplier.ExpenseAccountId = objSupplier.ExpenseAccountId;
                                    lockSupplier.TermId = objSupplier.TermId;
                                    lockSupplier.Address = objSupplier.Address;
                                    lockSupplier.ContactNumber = objSupplier.ContactNumber;
                                    lockSupplier.ContactPerson = objSupplier.ContactPerson;
                                    lockSupplier.TaxNumber = objSupplier.TaxNumber;
                                    lockSupplier.Particulars = objSupplier.Particulars;
                                    lockSupplier.EmailAddress = objSupplier.EmailAddress;
                                    lockSupplier.IsLocked = true;
                                    lockSupplier.UpdatedById = currentUserId;
                                    lockSupplier.UpdatedDateTime = DateTime.Now;

                                    db.SubmitChanges();

                                    return Request.CreateResponse(HttpStatusCode.OK);
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Locking Error. These supplier details are already locked.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "Data not found. These supplier details are not found in the server.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to lock supplier.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access for this supplier detail page.");
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
        // Unlock Supplier
        // ===============
        [Authorize, HttpPut, Route("api/supplier/unlock/{id}")]
        public HttpResponseMessage UnlockSupplier(String id)
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
                                    && d.SysForm.FormName.Equals("SupplierDetail")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanUnlock)
                        {
                            var supplier = from d in db.MstArticles
                                           where d.Id == Convert.ToInt32(id)
                                           && d.ArticleTypeId == 3
                                           select d;

                            if (supplier.Any())
                            {
                                if (supplier.FirstOrDefault().IsLocked)
                                {
                                    var unlockSupplier = supplier.FirstOrDefault();
                                    unlockSupplier.IsLocked = false;
                                    unlockSupplier.UpdatedById = currentUserId;
                                    unlockSupplier.UpdatedDateTime = DateTime.Now;

                                    db.SubmitChanges();

                                    return Request.CreateResponse(HttpStatusCode.OK);
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Unlocking Error. These supplier details are already unlocked.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "Data not found. These supplier details are not found in the server.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to unlock supplier.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access for this supplier detail page.");
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
        // Delete Supplier
        // ===============
        [Authorize, HttpDelete, Route("api/supplier/delete/{id}")]
        public HttpResponseMessage DeleteSupplier(String id)
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
                                    && d.SysForm.FormName.Equals("SupplierList")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanDelete)
                        {
                            var supplier = from d in db.MstArticles
                                           where d.Id == Convert.ToInt32(id)
                                           && d.ArticleTypeId == 3
                                           select d;

                            if (supplier.Any())
                            {
                                db.MstArticles.DeleteOnSubmit(supplier.First());
                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "Data not found. This selected supplier is not found in the server.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to delete supplier.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access for this supplier page.");
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
