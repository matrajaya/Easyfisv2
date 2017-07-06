using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.POSIntegrationApiControllers
{
    public class POSIntegrationApiSalesInvoiceController : ApiController
    {
        // data
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // business folders
        private Business.Inventory inventory = new Business.Inventory();
        private Business.PostJournal journal = new Business.PostJournal();

        // get current user's branch
        public Int32 getCurrentUserBranchId()
        {
            var mstUser = from d in db.MstUsers
                          where d.UserId == User.Identity.GetUserId()
                          select d;

            if (mstUser.Any())
            {
                return mstUser.FirstOrDefault().BranchId;
            }
            else
            {
                return 0;
            }
        }

        // zero padding for auto increment and auto generated code in every table
        public String zeroFill(Int32 number, Int32 length)
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

        // list of sales invoice for POS Integration
        [HttpGet]
        [Route("api/list/POSIntegration/salesInvoice")]
        public List<Models.TrnSalesInvoice> listSalesInvoicePOSIntegration()
        {
            var salesInvoices = from d in db.TrnSalesInvoices.OrderByDescending(d => d.Id)
                                where d.BranchId == getCurrentUserBranchId()
                                select new Models.TrnSalesInvoice
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    SINumber = d.SINumber,
                                    SIDate = d.SIDate.ToShortDateString(),
                                    CustomerId = d.CustomerId,
                                    Customer = d.MstArticle.Article,
                                    TermId = d.TermId,
                                    Term = d.MstTerm.Term,
                                    DocumentReference = d.DocumentReference,
                                    ManualSINumber = d.ManualSINumber,
                                    Remarks = d.Remarks,
                                    Amount = d.Amount,
                                    PaidAmount = d.PaidAmount,
                                    AdjustmentAmount = d.AdjustmentAmount,
                                    BalanceAmount = d.BalanceAmount,
                                    SoldById = d.SoldById,
                                    SoldBy = d.MstUser4.FullName,
                                    PreparedById = d.PreparedById,
                                    PreparedBy = d.MstUser3.FullName,
                                    CheckedById = d.CheckedById,
                                    CheckedBy = d.MstUser1.FullName,
                                    ApprovedById = d.ApprovedById,
                                    ApprovedBy = d.MstUser.FullName,
                                    IsLocked = d.IsLocked,
                                    CreatedById = d.CreatedById,
                                    CreatedBy = d.MstUser2.FullName,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedById = d.UpdatedById,
                                    UpdatedBy = d.MstUser5.FullName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                };

            return salesInvoices.ToList();
        }

        // get sales invoice by id for POS Integration
        [HttpGet]
        [Route("api/get/POSIntegration/salesInvoice/{id}")]
        public Models.TrnSalesInvoice getSalesInvoicePOSIntegration(String id)
        {
            var salesInvoices = from d in db.TrnSalesInvoices
                                where d.Id == Convert.ToInt32(id)
                                select new Models.TrnSalesInvoice
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    SINumber = d.SINumber,
                                    SIDate = d.SIDate.ToShortDateString(),
                                    CustomerId = d.CustomerId,
                                    Customer = d.MstArticle.Article,
                                    TermId = d.TermId,
                                    Term = d.MstTerm.Term,
                                    DocumentReference = d.DocumentReference,
                                    ManualSINumber = d.ManualSINumber,
                                    Remarks = d.Remarks,
                                    Amount = d.Amount,
                                    PaidAmount = d.PaidAmount,
                                    AdjustmentAmount = d.AdjustmentAmount,
                                    BalanceAmount = d.BalanceAmount,
                                    SoldById = d.SoldById,
                                    SoldBy = d.MstUser4.FullName,
                                    PreparedById = d.PreparedById,
                                    PreparedBy = d.MstUser3.FullName,
                                    CheckedById = d.CheckedById,
                                    CheckedBy = d.MstUser1.FullName,
                                    ApprovedById = d.ApprovedById,
                                    ApprovedBy = d.MstUser.FullName,
                                    IsLocked = d.IsLocked,
                                    CreatedById = d.CreatedById,
                                    CreatedBy = d.MstUser2.FullName,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedById = d.UpdatedById,
                                    UpdatedBy = d.MstUser5.FullName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                };

            return (Models.TrnSalesInvoice)salesInvoices.FirstOrDefault();
        }

        // add sales invoice for POS Integration
        [HttpPost]
        [Route("api/add/POSIntegration/salesInvoice")]
        public Int32 addSalesInvoicePOSIntegration(Models.TrnSalesInvoice sales)
        {
            try
            {
                // current user  id
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d).FirstOrDefault().Id;

                // get last SI Number
                var lastSINumber = from d in db.TrnSalesInvoices.OrderByDescending(d => d.Id) select d;
                var SINumberResult = "0000000001";
                if (lastSINumber.Any())
                {
                    var SINumber = Convert.ToInt32(lastSINumber.FirstOrDefault().SINumber) + 0000000001;
                    SINumberResult = zeroFill(SINumber, 10);
                }

                // customers and terms
                var customers = from d in db.MstArticles where d.ArticleTypeId == 2 select d;
                var terms = from d in db.MstTerms select d;

                // add Sales invoice
                Data.TrnSalesInvoice addSalesInvoice = new Data.TrnSalesInvoice();
                addSalesInvoice.BranchId = getCurrentUserBranchId();
                addSalesInvoice.SINumber = SINumberResult;
                addSalesInvoice.SIDate = DateTime.Today;
                addSalesInvoice.CustomerId = customers.FirstOrDefault().Id;
                addSalesInvoice.TermId = customers.FirstOrDefault().Id;
                addSalesInvoice.DocumentReference = "NA";
                addSalesInvoice.ManualSINumber = "NA";
                addSalesInvoice.Remarks = "NA";
                addSalesInvoice.Amount = 0;
                addSalesInvoice.PaidAmount = 0;
                addSalesInvoice.AdjustmentAmount = 0;
                addSalesInvoice.BalanceAmount = 0;
                addSalesInvoice.SoldById = userId;
                addSalesInvoice.PreparedById = userId;
                addSalesInvoice.CheckedById = userId;
                addSalesInvoice.ApprovedById = userId;
                addSalesInvoice.IsLocked = false;
                addSalesInvoice.CreatedById = userId;
                addSalesInvoice.CreatedDateTime = DateTime.Now;
                addSalesInvoice.UpdatedById = userId;
                addSalesInvoice.UpdatedDateTime = DateTime.Now;
                db.TrnSalesInvoices.InsertOnSubmit(addSalesInvoice);
                db.SubmitChanges();

                return addSalesInvoice.Id;
            }
            catch
            {
                return 0;
            }
        }

        // lock sales invoice for POS Integration
        [HttpPut]
        [Route("api/lock/POSIntegration/salesInvoice/{id}")]
        public HttpResponseMessage lockSalesInvoicePOSIntegration(String id, Models.TrnSalesInvoice sales)
        {
            try
            {
                // sales invoice by Id
                var salesInvoice = from d in db.TrnSalesInvoices
                                   where d.Id == Convert.ToInt32(id)
                                   select d;

                // check if exist
                if (salesInvoice.Any())
                {
                    // check if unlock
                    if (!salesInvoice.FirstOrDefault().IsLocked)
                    {

                        // get collection line
                        Decimal collectionLinesTotalAmount = 0;
                        var collectionLines = from d in db.TrnCollectionLines
                                              where d.SIId == Convert.ToInt32(id)
                                              && d.TrnCollection.IsLocked == true
                                              select d;

                        // check if exist
                        if (collectionLines.Any())
                        {
                            collectionLinesTotalAmount = collectionLines.Sum(d => d.Amount);
                        }

                        // get total amount in sales invoice items
                        Decimal salesInvoiceItemTotalAmount = 0;
                        var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                                where d.SIId == Convert.ToInt32(id)
                                                select d;

                        if (salesInvoiceItems.Any())
                        {
                            salesInvoiceItemTotalAmount = salesInvoiceItems.Sum(d => d.Amount);
                        }

                        // current user  id
                        var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d).FirstOrDefault().Id;

                        // lock and update sales invoice item
                        var lockSalesInvoice = salesInvoice.FirstOrDefault();
                        lockSalesInvoice.BranchId = sales.BranchId;
                        lockSalesInvoice.SINumber = sales.SINumber;
                        lockSalesInvoice.SIDate = Convert.ToDateTime(sales.SIDate);
                        lockSalesInvoice.CustomerId = sales.CustomerId;
                        lockSalesInvoice.TermId = sales.TermId;
                        lockSalesInvoice.DocumentReference = sales.DocumentReference;
                        lockSalesInvoice.ManualSINumber = sales.ManualSINumber;
                        lockSalesInvoice.Remarks = sales.Remarks;
                        lockSalesInvoice.Amount = salesInvoiceItemTotalAmount;
                        lockSalesInvoice.PaidAmount = collectionLinesTotalAmount;
                        lockSalesInvoice.AdjustmentAmount = 0;
                        lockSalesInvoice.BalanceAmount = salesInvoiceItemTotalAmount - collectionLinesTotalAmount;
                        lockSalesInvoice.SoldById = sales.SoldById;
                        lockSalesInvoice.PreparedById = sales.PreparedById;
                        lockSalesInvoice.CheckedById = sales.CheckedById;
                        lockSalesInvoice.ApprovedById = sales.ApprovedById;
                        lockSalesInvoice.IsLocked = true;
                        lockSalesInvoice.UpdatedById = userId;
                        lockSalesInvoice.UpdatedDateTime = DateTime.Now;
                        db.SubmitChanges();

                        // business
                        inventory.InsertSIInventory(Convert.ToInt32(id));
                        journal.insertSIJournal(Convert.ToInt32(id));

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "The sales invoice record you're trying to lock was already locked.");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No sales invoice record found.");
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }

        // unlock sales invoice for POS Integration 
        [HttpPut]
        [Route("api/unlock/POSIntegration/salesInvoice/{id}")]
        public HttpResponseMessage unlockSalesInvoicePOSIntegration(String id)
        {
            try
            {
                // current sales invoice
                var salesInvoce = from d in db.TrnSalesInvoices
                                  where d.Id == Convert.ToInt32(id)
                                  select d;

                // check if exist
                if (salesInvoce.Any())
                {
                    // check if lock
                    if (salesInvoce.FirstOrDefault().IsLocked)
                    {
                        // current user  id
                        var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d).FirstOrDefault().Id;

                        // unlock and update sales invoice
                        var unlockSalesInvoice = salesInvoce.FirstOrDefault();
                        unlockSalesInvoice.IsLocked = false;
                        unlockSalesInvoice.UpdatedById = userId;
                        unlockSalesInvoice.UpdatedDateTime = DateTime.Now;
                        db.SubmitChanges();

                        // business
                        inventory.deleteSIInventory(Convert.ToInt32(id));
                        journal.deleteSIJournal(Convert.ToInt32(id));

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "The sales invoice record you're trying to unlock was already unlocked.");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No sales invoice record found.");
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }

        // delete sales invoice for POS Integration 
        [HttpDelete]
        [Route("api/delete/POSIntegration/salesInvoice/{id}")]
        public HttpResponseMessage deleteSalesInvoicePOSIntegration(String id)
        {
            try
            {
                // current sales invoice
                var salesInvoce = from d in db.TrnSalesInvoices
                                  where d.Id == Convert.ToInt32(id)
                                  select d;

                // check if exist
                if (salesInvoce.Any())
                {
                    // check if unlock
                    if (!salesInvoce.FirstOrDefault().IsLocked)
                    {
                        // delete the selected record
                        db.TrnSalesInvoices.DeleteOnSubmit(salesInvoce.First());
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Delete is not allowed when the sales invoice record is locked. Unlock the record first then try to delete again.");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No sales invoice record found.");
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }
    }
}
