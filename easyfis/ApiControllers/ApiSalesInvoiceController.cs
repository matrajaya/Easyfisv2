using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace easyfis.Controllers
{
    public class ApiSalesInvoiceController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        private Business.Inventory inventory = new Business.Inventory();
        private Business.Journal journal = new Business.Journal();

        // current branch Id
        public Int32 currentBranchId()
        {
            return (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.BranchId).SingleOrDefault();
        }

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

        // get amount in sales
        public Decimal getAmountSalesInvoiceItem(Int32 SIId)
        {
            Decimal amount = 0;

            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems where d.SIId == SIId select d;
            if (salesInvoiceItems.Any())
            {
                amount = salesInvoiceItems.Sum(d => d.Amount);
            }

            return amount;
        }

        // list sales invoice
        [Authorize]
        [HttpGet]
        [Route("api/listSalesInvoice")]
        public List<Models.TrnSalesInvoice> listSalesInvoice()
        {
            var salesInvoices = from d in db.TrnSalesInvoices.OrderByDescending(d => d.Id)
                                where d.BranchId == currentBranchId()
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

        //// get sales invoice by id
        //[Authorize]
        //[HttpGet]
        //[Route("api/salesInvoice/{id}")]
        //public Models.TrnSalesInvoice getSalesInvoiceById(String id)
        //{
        //    var salesInvoices = from d in db.TrnSalesInvoices
        //                        where d.Id == Convert.ToInt32(id)
        //                        select new Models.TrnSalesInvoice
        //                        {
        //                            Id = d.Id,
        //                            BranchId = d.BranchId,
        //                            Branch = d.MstBranch.Branch,
        //                            SINumber = d.SINumber,
        //                            SIDate = d.SIDate.ToShortDateString(),
        //                            CustomerId = d.CustomerId,
        //                            Customer = d.MstArticle.Article,
        //                            TermId = d.TermId,
        //                            Term = d.MstTerm.Term,
        //                            DocumentReference = d.DocumentReference,
        //                            ManualSINumber = d.ManualSINumber,
        //                            Remarks = d.Remarks,
        //                            Amount = d.Amount,
        //                            PaidAmount = d.PaidAmount,
        //                            AdjustmentAmount = d.AdjustmentAmount,
        //                            BalanceAmount = d.BalanceAmount,
        //                            SoldById = d.SoldById,
        //                            SoldBy = d.MstUser4.FullName,
        //                            PreparedById = d.PreparedById,
        //                            PreparedBy = d.MstUser3.FullName,
        //                            CheckedById = d.CheckedById,
        //                            CheckedBy = d.MstUser1.FullName,
        //                            ApprovedById = d.ApprovedById,
        //                            ApprovedBy = d.MstUser.FullName,
        //                            IsLocked = d.IsLocked,
        //                            CreatedById = d.CreatedById,
        //                            CreatedBy = d.MstUser2.FullName,
        //                            CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
        //                            UpdatedById = d.UpdatedById,
        //                            UpdatedBy = d.MstUser5.FullName,
        //                            UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
        //                        };

        //    return (Models.TrnSalesInvoice)salesInvoices.FirstOrDefault();
        //}

        // list sales invoice by customerId 
        [Authorize]
        [HttpGet]
        [Route("api/salesInvoiceByCustomerId/{customerId}")]
        public List<Models.TrnSalesInvoice> listSalesInvoiceByCustomerId(String customerId)
        {
            var salesInvoices = from d in db.TrnSalesInvoices
                                where d.CustomerId == Convert.ToInt32(customerId)
                                && d.BranchId == currentBranchId()
                                && d.IsLocked == true
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

        // list sales invoice by customer Id and by Balance > 0 for collection accounts receivable
        [Authorize]
        [HttpGet]
        [Route("api/salesInvoiceByCustomerIdByBalance/{customerId}")]
        public List<Models.TrnSalesInvoice> listSalesInvoiceByCustomerIdByBalance(String customerId)
        {
            var salesInvoices = from d in db.TrnSalesInvoices
                                where d.CustomerId == Convert.ToInt32(customerId)
                                && d.IsLocked == true
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
                                    AccountId = d.MstArticle.AccountId,
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

        // list sales invoice by SIDate
        [Authorize]
        [HttpGet]
        [Route("api/listSalesInvoiceFilterBySIDate/{SIStartDate}/{SISEndDate}")]
        public List<Models.TrnSalesInvoice> listSalesInvoiceBySIDate(String SIStartDate, String SISEndDate)
        {
            var salesInvoices = from d in db.TrnSalesInvoices.OrderByDescending(d => d.Id)
                                where d.SIDate >= Convert.ToDateTime(SIStartDate)
                                && d.SIDate <= Convert.ToDateTime(SISEndDate)
                                && d.BranchId == currentBranchId()
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

        // get sales invoice last SINumber
        [Authorize]
        [HttpGet]
        [Route("api/salesInvoiceLastSINumber")]
        public Models.TrnSalesInvoice getSalesInvoiceLastSINumber()
        {
            var salesInvoices = from d in db.TrnSalesInvoices.OrderByDescending(d => d.SINumber)
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

        // add sales invoice
        [Authorize]
        [HttpPost]
        [Route("api/addSales")]
        public Int32 insertSalesInvoice(Models.TrnSalesInvoice sales)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var lastSINumber = from d in db.TrnSalesInvoices.OrderByDescending(d => d.Id) where d.BranchId == currentBranchId() select d;
                var SINumberResult = "0000000001";

                if (lastSINumber.Any())
                {
                    var SINumber = Convert.ToInt32(lastSINumber.FirstOrDefault().SINumber) + 0000000001;
                    SINumberResult = zeroFill(SINumber, 10);
                }

                Data.TrnSalesInvoice newSalesInvoice = new Data.TrnSalesInvoice();
                newSalesInvoice.BranchId = currentBranchId();
                newSalesInvoice.SINumber = SINumberResult;
                newSalesInvoice.SIDate = DateTime.Today;
                newSalesInvoice.CustomerId = (from d in db.MstArticles where d.ArticleTypeId == 2 select d.Id).FirstOrDefault();
                newSalesInvoice.TermId = (from d in db.MstTerms select d.Id).FirstOrDefault();
                newSalesInvoice.DocumentReference = "NA";
                newSalesInvoice.ManualSINumber = "NA";
                newSalesInvoice.Remarks = "NA";
                newSalesInvoice.Amount = 0;
                newSalesInvoice.PaidAmount = 0;
                newSalesInvoice.AdjustmentAmount = 0;
                newSalesInvoice.BalanceAmount = 0;
                newSalesInvoice.SoldById = userId;
                newSalesInvoice.PreparedById = userId;
                newSalesInvoice.CheckedById = userId;
                newSalesInvoice.ApprovedById = userId;
                newSalesInvoice.IsLocked = false;
                newSalesInvoice.CreatedById = userId;
                newSalesInvoice.CreatedDateTime = DateTime.Now;
                newSalesInvoice.UpdatedById = userId;
                newSalesInvoice.UpdatedDateTime = DateTime.Now;

                db.TrnSalesInvoices.InsertOnSubmit(newSalesInvoice);
                db.SubmitChanges();

                return newSalesInvoice.Id;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        // update sales invoice
        [Authorize]
        [HttpPut]
        [Route("api/updateSales/{id}")]
        public HttpResponseMessage updateSalesInvoice(String id, Models.TrnSalesInvoice sales)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();
                var salesInvoices = from d in db.TrnSalesInvoices where d.Id == Convert.ToInt32(id) select d;

                if (salesInvoices.Any())
                {
                    Decimal PaidAmount = 0;

                    var collectionLinesORId = from d in db.TrnCollectionLines where d.SIId == Convert.ToInt32(id) select d;
                    if (collectionLinesORId.Any())
                    {
                        Boolean collectionHeaderIsLocked = (from d in db.TrnCollections where d.Id == collectionLinesORId.First().ORId select d.IsLocked).SingleOrDefault();
                        var collectionLines = from d in db.TrnCollectionLines where d.SIId == Convert.ToInt32(id) select d;
                        if (collectionLines.Any())
                        {
                            if (collectionHeaderIsLocked == true)
                            {
                                PaidAmount = collectionLines.Sum(d => d.Amount);
                            }
                        }
                    }

                    var updateSalesInvoice = salesInvoices.FirstOrDefault();
                    updateSalesInvoice.BranchId = sales.BranchId;
                    updateSalesInvoice.SINumber = sales.SINumber;
                    updateSalesInvoice.SIDate = Convert.ToDateTime(sales.SIDate);
                    updateSalesInvoice.CustomerId = sales.CustomerId;
                    updateSalesInvoice.TermId = sales.TermId;
                    updateSalesInvoice.DocumentReference = sales.DocumentReference;
                    updateSalesInvoice.ManualSINumber = sales.ManualSINumber;
                    updateSalesInvoice.Remarks = sales.Remarks;
                    updateSalesInvoice.Amount = getAmountSalesInvoiceItem(Convert.ToInt32(id));
                    updateSalesInvoice.PaidAmount = PaidAmount;
                    updateSalesInvoice.AdjustmentAmount = 0;
                    updateSalesInvoice.BalanceAmount = getAmountSalesInvoiceItem(Convert.ToInt32(id)) - PaidAmount;
                    updateSalesInvoice.SoldById = sales.SoldById;
                    updateSalesInvoice.PreparedById = sales.PreparedById;
                    updateSalesInvoice.CheckedById = sales.CheckedById;
                    updateSalesInvoice.ApprovedById = sales.ApprovedById;
                    updateSalesInvoice.IsLocked = true;
                    updateSalesInvoice.UpdatedById = userId;
                    updateSalesInvoice.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    inventory.InsertSIInventory(Convert.ToInt32(id));
                    journal.insertSIJournal(Convert.ToInt32(id));

                    // Check for negative inventory
                    bool foundNegativeQuantity = false;
                    if (updateSalesInvoice.TrnSalesInvoiceItems.Any())
                    {
                        foreach (var salesInvoiceItem in updateSalesInvoice.TrnSalesInvoiceItems)
                        {
                            if (salesInvoiceItem.MstArticle.IsInventory)
                            {
                                var mstArticleInventory = from d in db.MstArticleInventories
                                                          where d.TrnSalesInvoiceItems.Contains(salesInvoiceItem)
                                                          select d;

                                if (mstArticleInventory.Any())
                                {
                                    if (salesInvoiceItem.MstArticleInventory.Quantity < 0)
                                    {
                                        foundNegativeQuantity = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (!foundNegativeQuantity)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        inventory.deleteSIInventory(Convert.ToInt32(id));
                        journal.deleteSIJournal(Convert.ToInt32(id));

                        updateSalesInvoice.IsLocked = false;
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Negative Inventory Found!");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Sales Invoice Found!");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Internal Server Error!");
            }
        }

        // unlock sales invoice
        [Authorize]
        [HttpPut]
        [Route("api/updateSalesIsLocked/{id}")]
        public HttpResponseMessage unlockSalesInvoice(String id, Models.TrnSalesInvoice sales)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();
                var salesInvoces = from d in db.TrnSalesInvoices where d.Id == Convert.ToInt32(id) select d;

                if (salesInvoces.Any())
                {
                    var updateSalesInvoice = salesInvoces.FirstOrDefault();

                    updateSalesInvoice.IsLocked = false;
                    updateSalesInvoice.UpdatedById = userId;
                    updateSalesInvoice.UpdatedDateTime = DateTime.Now;

                    db.SubmitChanges();

                    if (updateSalesInvoice.IsLocked == true)
                    {
                        inventory.InsertSIInventory(Convert.ToInt32(id));
                        journal.insertSIJournal(Convert.ToInt32(id));
                    }
                    else
                    {
                        inventory.deleteSIInventory(Convert.ToInt32(id));
                        journal.deleteSIJournal(Convert.ToInt32(id));
                    }

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

        // delete sales
        [Authorize]
        [HttpDelete]
        [Route("api/deleteSales/{id}")]
        public HttpResponseMessage deleteSalesInvoice(String id)
        {
            try
            {
                var sales = from d in db.TrnSalesInvoices where d.Id == Convert.ToInt32(id) select d;
                if (sales.Any())
                {
                    db.TrnSalesInvoices.DeleteOnSubmit(sales.First());
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
