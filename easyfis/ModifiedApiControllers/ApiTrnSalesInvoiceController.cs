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
    public class ApiTrnSalesInvoiceController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==================
        // List Sales Invoice
        // ==================
        [Authorize, HttpGet, Route("api/salesInvoice/list/{startDate}/{endDate}")]
        public List<Entities.TrnSalesInvoice> ListSalesInvoice(String startDate, String endDate)
        {
            var currentUser = from d in db.MstUsers
                              where d.UserId == User.Identity.GetUserId()
                              select d;

            var branchId = currentUser.FirstOrDefault().BranchId;

            var salesInvoices = from d in db.TrnSalesInvoices.OrderByDescending(d => d.Id)
                                where d.BranchId == branchId
                                && d.SIDate >= Convert.ToDateTime(startDate)
                                && d.SIDate <= Convert.ToDateTime(endDate)
                                select new Entities.TrnSalesInvoice
                                {
                                    Id = d.Id,
                                    SINumber = d.SINumber,
                                    SIDate = d.SIDate.ToShortDateString(),
                                    Customer = d.MstArticle.Article,
                                    Remarks = d.Remarks,
                                    DocumentReference = d.DocumentReference,
                                    Amount = d.Amount,
                                    IsLocked = d.IsLocked,
                                    CreatedBy = d.MstUser2.FullName,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedBy = d.MstUser5.FullName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                };

            return salesInvoices.ToList();
        }

        // ====================
        // Detail Sales Invoice
        // ====================
        [Authorize, HttpGet, Route("api/salesInvoice/detail/{id}")]
        public Entities.TrnSalesInvoice DetailSalesInvoice(String id)
        {
            var currentUser = from d in db.MstUsers
                              where d.UserId == User.Identity.GetUserId()
                              select d;

            var branchId = currentUser.FirstOrDefault().BranchId;

            var salesInvoice = from d in db.TrnSalesInvoices.OrderByDescending(d => d.Id)
                               where d.BranchId == branchId
                               && d.Id >= Convert.ToInt32(id)
                               select new Entities.TrnSalesInvoice
                               {
                                   Id = d.Id,
                                   BranchId = d.BranchId,
                                   SINumber = d.SINumber,
                                   SIDate = d.SIDate.ToShortDateString(),
                                   DocumentReference = d.DocumentReference,
                                   Customer = d.MstArticle.Article,
                                   TermId = d.TermId,
                                   Remarks = d.Remarks,
                                   ManualSINumber = d.ManualSINumber,
                                   SoldById = d.SoldById,
                                   PreparedById = d.PreparedById,
                                   CheckedById = d.CheckedById,
                                   ApprovedById = d.ApprovedById,
                                   IsLocked = d.IsLocked,
                                   CreatedBy = d.MstUser2.FullName,
                                   CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                   UpdatedBy = d.MstUser5.FullName,
                                   UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                               };

            if (salesInvoice.Any())
            {
                return salesInvoice.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        // ==============================
        // Dropdown List - Branch (Field)
        // ==============================
        [Authorize, HttpGet, Route("api/salesInvoice/dropdown/list/branch")]
        public List<Entities.MstBranch> DropdownListSalesInvoiceBranch()
        {
            var branches = from d in db.MstBranches.OrderBy(d => d.Branch)
                           select new Entities.MstBranch
                           {
                               Id = d.Id,
                               Branch = d.Branch
                           };

            return branches.ToList();
        }

        // ================================
        // Dropdown List - Customer (Field)
        // ================================
        [Authorize, HttpGet, Route("api/salesInvoice/dropdown/list/customer")]
        public List<Entities.MstArticle> DropdownListSalesInvoiceCustomer()
        {
            var customers = from d in db.MstArticles.OrderBy(d => d.Article)
                            where d.ArticleTypeId == 2
                            && d.IsLocked == true
                            select new Entities.MstArticle
                            {
                                Id = d.Id,
                                Article = d.Article
                            };

            return customers.ToList();
        }

        // ============================
        // Dropdown List - Term (Field)
        // ============================
        [Authorize, HttpGet, Route("api/salesInvoice/dropdown/list/term")]
        public List<Entities.MstTerm> DropdownListSalesInvoiceTerm()
        {
            var terms = from d in db.MstTerms.OrderBy(d => d.Term)
                        where d.IsLocked == true
                        select new Entities.MstTerm
                        {
                            Id = d.Id,
                            Term = d.Term
                        };

            return terms.ToList();
        }

        // ============================
        // Dropdown List - User (Field)
        // ============================
        [Authorize, HttpGet, Route("api/salesInvoice/dropdown/list/users")]
        public List<Entities.MstUser> DropdownListSalesInvoiceUsers()
        {
            var users = from d in db.MstUsers.OrderBy(d => d.FullName)
                        where d.IsLocked == true
                        select new Entities.MstUser
                        {
                            Id = d.Id,
                            FullName = d.FullName
                        };

            return users.ToList();
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

        // =================
        // Add Sales Invoice
        // =================
        [Authorize, HttpPost, Route("api/salesInvoice/add")]
        public HttpResponseMessage AddSalesInvoice()
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;
                    var currentBranchId = currentUser.FirstOrDefault().BranchId;

                    var userForms = from d in db.MstUserForms
                                    where d.UserId == currentUserId
                                    && d.SysForm.FormName.Equals("SalesInvoiceList")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanAdd)
                        {
                            var defaultSINumber = "0000000001";
                            var lastSalesInvoice = from d in db.TrnSalesInvoices.OrderByDescending(d => d.Id)
                                                   where d.BranchId == currentBranchId
                                                   select d;

                            if (lastSalesInvoice.Any())
                            {
                                var SINumber = Convert.ToInt32(lastSalesInvoice.FirstOrDefault().SINumber) + 0000000001;
                                defaultSINumber = FillLeadingZeroes(SINumber, 10);
                            }

                            var customers = from d in db.MstArticles.OrderBy(d => d.Article)
                                            where d.ArticleTypeId == 2
                                            && d.IsLocked == true
                                            select d;

                            if (customers.Any())
                            {
                                var terms = from d in db.MstTerms.OrderBy(d => d.Term)
                                            where d.IsLocked == true
                                            select d;

                                if (terms.Any())
                                {
                                    var users = from d in db.MstUsers.OrderBy(d => d.FullName)
                                                where d.IsLocked == true
                                                select d;

                                    if (users.Any())
                                    {
                                        Data.TrnSalesInvoice newSalesInvoice = new Data.TrnSalesInvoice
                                        {
                                            BranchId = currentBranchId,
                                            SINumber = defaultSINumber,
                                            SIDate = DateTime.Today,
                                            DocumentReference = "NA",
                                            CustomerId = customers.FirstOrDefault().Id,
                                            TermId = terms.FirstOrDefault().Id,
                                            Remarks = "NA",
                                            ManualSINumber = "NA",
                                            Amount = 0,
                                            PaidAmount = 0,
                                            AdjustmentAmount = 0,
                                            BalanceAmount = 0,
                                            SoldById = currentUserId,
                                            PreparedById = currentUserId,
                                            CheckedById = currentUserId,
                                            ApprovedById = currentUserId,
                                            IsLocked = false,
                                            CreatedById = currentUserId,
                                            CreatedDateTime = DateTime.Now,
                                            UpdatedById = currentUserId,
                                            UpdatedDateTime = DateTime.Now
                                        };

                                        db.TrnSalesInvoices.InsertOnSubmit(newSalesInvoice);
                                        db.SubmitChanges();

                                        return Request.CreateResponse(HttpStatusCode.OK, newSalesInvoice.Id);
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.NotFound, "No user found. Please setup more users for all transactions.");
                                    }
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.NotFound, "No term found. Please setup more terms for all transactions.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "No supplier found. Please setup more suppliers for all transactions.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to add sales invoice.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access for this sales invoice page.");
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

        // ========================
        // Get Sales Invoice Amount
        // ========================
        public Decimal GetSalesInvoiceAmount(Int32 SIId)
        {
            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                    where d.SIId == SIId
                                    select d;

            if (salesInvoiceItems.Any())
            {
                return salesInvoiceItems.Sum(d => d.Amount);
            }
            else
            {
                return 0;
            }
        }

        // ==================
        // Lock Sales Invoice
        // ==================
        [Authorize, HttpPut, Route("api/salesInvoice/lock/{id}")]
        public HttpResponseMessage LockSalesInvoice(Entities.TrnSalesInvoice objSalesInvoice, String id)
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
                                    && d.SysForm.FormName.Equals("SalesInvoiceDetail")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanLock)
                        {
                            var salesInvoice = from d in db.TrnSalesInvoices
                                               where d.Id == Convert.ToInt32(id)
                                               select d;

                            if (salesInvoice.Any())
                            {
                                if (!salesInvoice.FirstOrDefault().IsLocked)
                                {
                                    Decimal paidAmount = 0;

                                    var collectionLines = from d in db.TrnCollectionLines
                                                          where d.SIId == Convert.ToInt32(id)
                                                          && d.TrnCollection.IsLocked == true
                                                          select d;

                                    if (collectionLines.Any())
                                    {
                                        paidAmount = collectionLines.Sum(d => d.Amount);
                                    }

                                    var lockSalesInvoice = salesInvoice.FirstOrDefault();
                                    lockSalesInvoice.SIDate = Convert.ToDateTime(objSalesInvoice.SIDate);
                                    lockSalesInvoice.CustomerId = objSalesInvoice.CustomerId;
                                    lockSalesInvoice.TermId = objSalesInvoice.TermId;
                                    lockSalesInvoice.DocumentReference = objSalesInvoice.DocumentReference;
                                    lockSalesInvoice.ManualSINumber = objSalesInvoice.ManualSINumber;
                                    lockSalesInvoice.Remarks = objSalesInvoice.Remarks;
                                    lockSalesInvoice.Amount = GetSalesInvoiceAmount(Convert.ToInt32(id));
                                    lockSalesInvoice.PaidAmount = paidAmount;
                                    lockSalesInvoice.AdjustmentAmount = 0;
                                    lockSalesInvoice.BalanceAmount = GetSalesInvoiceAmount(Convert.ToInt32(id)) - paidAmount;
                                    lockSalesInvoice.SoldById = objSalesInvoice.SoldById;
                                    lockSalesInvoice.CheckedById = objSalesInvoice.CheckedById;
                                    lockSalesInvoice.ApprovedById = objSalesInvoice.ApprovedById;
                                    lockSalesInvoice.IsLocked = true;
                                    lockSalesInvoice.UpdatedById = currentUserId;
                                    lockSalesInvoice.UpdatedDateTime = DateTime.Now;

                                    db.SubmitChanges();

                                    // =====================
                                    // Inventory and Journal
                                    // =====================
                                    Business.Inventory inventory = new Business.Inventory();
                                    Business.Journal journal = new Business.Journal();

                                    if (lockSalesInvoice.IsLocked)
                                    {
                                        inventory.InsertSIInventory(Convert.ToInt32(id));
                                        journal.insertSIJournal(Convert.ToInt32(id));
                                    }

                                    return Request.CreateResponse(HttpStatusCode.OK);
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Locking Error. These sales invoice details are already locked.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "Data not found. These sales invoice details are not found in the server.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to lock sales invoice.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access for this sales invoice page.");
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

        // ====================
        // Unlock Sales Invoice
        // ====================
        [Authorize, HttpPut, Route("api/salesInvoice/unlock/{id}")]
        public HttpResponseMessage UnlockSalesInvoice(String id)
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
                                    && d.SysForm.FormName.Equals("SalesInvoiceDetail")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanUnlock)
                        {
                            var salesInvoice = from d in db.TrnSalesInvoices
                                               where d.Id == Convert.ToInt32(id)
                                               select d;

                            if (salesInvoice.Any())
                            {
                                if (salesInvoice.FirstOrDefault().IsLocked)
                                {
                                    var unlockSalesInvoice = salesInvoice.FirstOrDefault();
                                    unlockSalesInvoice.IsLocked = false;
                                    unlockSalesInvoice.UpdatedById = currentUserId;
                                    unlockSalesInvoice.UpdatedDateTime = DateTime.Now;

                                    db.SubmitChanges();

                                    // =====================
                                    // Inventory and Journal
                                    // =====================
                                    Business.Inventory inventory = new Business.Inventory();
                                    Business.Journal journal = new Business.Journal();

                                    if (!unlockSalesInvoice.IsLocked)
                                    {
                                        inventory.deleteSIInventory(Convert.ToInt32(id));
                                        journal.deleteSIJournal(Convert.ToInt32(id));
                                    }

                                    return Request.CreateResponse(HttpStatusCode.OK);
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Unlocking Error. These sales invoice details are already unlocked.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "Data not found. These sales invoice details are not found in the server.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to unlock sales invoice.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access for this sales invoice page.");
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

        // ====================
        // Delete Sales Invoice
        // ====================
        [Authorize, HttpDelete, Route("api/salesInvoice/delete/{id}")]
        public HttpResponseMessage DeleteSalesInvoice(String id)
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
                                    && d.SysForm.FormName.Equals("SalesInvoiceList")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanDelete)
                        {
                            var salesInvoice = from d in db.TrnSalesInvoices
                                               where d.Id == Convert.ToInt32(id)
                                               select d;

                            if (salesInvoice.Any())
                            {
                                if (!salesInvoice.FirstOrDefault().IsLocked)
                                {
                                    db.TrnSalesInvoices.DeleteOnSubmit(salesInvoice.First());
                                    db.SubmitChanges();

                                    return Request.CreateResponse(HttpStatusCode.OK);
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Delete Error. You cannot delete sales invoice if the current sales invoice record is locked.");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "Data not found. These sales invoice details are not found in the server.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to delete sales invoice.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access for this sales invoice page.");
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
