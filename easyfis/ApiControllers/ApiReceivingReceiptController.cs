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
    public class ApiReceivingReceiptController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        private Business.Inventory inventory = new Business.Inventory();
        private Business.PostJournal journal = new Business.PostJournal();

        // current branch Id
        public Int32 currentBranchId()
        {
            return (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.BranchId).SingleOrDefault();
        }

        // get amount in receiving items
        public Decimal getAmountReceivingReceiptItem(Int32 RRId)
        {
            Decimal totalAmount = 0;

            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems where d.RRId == RRId select d;
            if (receivingReceiptItems.Any())
            {
                totalAmount = receivingReceiptItems.Sum(d => d.Amount);
            }

            return totalAmount;
        }

        // list receiving receipt
        [Authorize]
        [HttpGet]
        [Route("api/listReceivingReceipt")]
        public List<Models.TrnReceivingReceipt> listReceivingReceipt()
        {
            var receivingReceipts = from d in db.TrnReceivingReceipts.OrderByDescending(d => d.Id)
                                    where d.BranchId == currentBranchId()
                                    select new Models.TrnReceivingReceipt
                                    {
                                        Id = d.Id,
                                        BranchId = d.BranchId,
                                        Branch = d.MstBranch.Branch,
                                        RRDate = d.RRDate.ToShortDateString(),
                                        RRNumber = d.RRNumber,
                                        SupplierId = d.SupplierId,
                                        Supplier = d.MstArticle.Article,
                                        TermId = d.TermId,
                                        Term = d.MstTerm.Term,
                                        DocumentReference = d.DocumentReference,
                                        ManualRRNumber = d.ManualRRNumber,
                                        Remarks = d.Remarks,
                                        Amount = d.Amount,
                                        WTaxAmount = d.WTaxAmount,
                                        PaidAmount = d.PaidAmount,
                                        AdjustmentAmount = d.AdjustmentAmount,
                                        BalanceAmount = d.BalanceAmount,
                                        ReceivedById = d.ReceivedById,
                                        ReceivedBy = d.MstUser4.FullName,
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

            return receivingReceipts.ToList();
        }

        // get receiving receipt by Id
        [Authorize]
        [HttpGet]
        [Route("api/receivingReceipt/{id}")]
        public Models.TrnReceivingReceipt getReceivingReceiptById(String id)
        {
            var receivingReceipt = from d in db.TrnReceivingReceipts
                                   where d.Id == Convert.ToInt32(id)
                                   select new Models.TrnReceivingReceipt
                                   {
                                       Id = d.Id,
                                       BranchId = d.BranchId,
                                       Branch = d.MstBranch.Branch,
                                       RRDate = d.RRDate.ToShortDateString(),
                                       RRNumber = d.RRNumber,
                                       SupplierId = d.SupplierId,
                                       Supplier = d.MstArticle.Article,
                                       TermId = d.TermId,
                                       Term = d.MstTerm.Term,
                                       DocumentReference = d.DocumentReference,
                                       ManualRRNumber = d.ManualRRNumber,
                                       Remarks = d.Remarks,
                                       Amount = d.Amount,
                                       WTaxAmount = d.WTaxAmount,
                                       PaidAmount = d.PaidAmount,
                                       AdjustmentAmount = d.AdjustmentAmount,
                                       BalanceAmount = d.BalanceAmount,
                                       ReceivedById = d.ReceivedById,
                                       ReceivedBy = d.MstUser4.FullName,
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

            return (Models.TrnReceivingReceipt)receivingReceipt.FirstOrDefault();
        }

        // list receiving receipt by SupplierId
        [Authorize]
        [HttpGet]
        [Route("api/receivingReceiptBySupplierId/{supplierId}")]
        public List<Models.TrnReceivingReceipt> listReceivingReceiptBySupplierId(String supplierId)
        {
            var receivingReceipts = from d in db.TrnReceivingReceipts
                                    where d.SupplierId == Convert.ToInt32(supplierId)
                                    && d.BranchId == currentBranchId()
                                    && d.IsLocked == true
                                    select new Models.TrnReceivingReceipt
                                    {
                                        Id = d.Id,
                                        BranchId = d.BranchId,
                                        Branch = d.MstBranch.Branch,
                                        RRDate = d.RRDate.ToShortDateString(),
                                        RRNumber = d.RRNumber,
                                        SupplierId = d.SupplierId,
                                        Supplier = d.MstArticle.Article,
                                        TermId = d.TermId,
                                        Term = d.MstTerm.Term,
                                        DocumentReference = d.DocumentReference,
                                        ManualRRNumber = d.ManualRRNumber,
                                        Remarks = d.Remarks,
                                        Amount = d.Amount,
                                        WTaxAmount = d.WTaxAmount,
                                        PaidAmount = d.PaidAmount,
                                        AdjustmentAmount = d.AdjustmentAmount,
                                        BalanceAmount = d.BalanceAmount,
                                        ReceivedById = d.ReceivedById,
                                        ReceivedBy = d.MstUser4.FullName,
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

            return receivingReceipts.ToList();
        }

        // list receiving receipt by SupplierId and by Balance > 0 for disbursement accounts payable
        [Authorize]
        [HttpGet]
        [Route("api/receivingReceiptBySupplierIdByBalance/{supplierId}")]
        public List<Models.TrnReceivingReceipt> listReceivingReceiptBySupplierIdByBalance(String supplierId)
        {
            var receivingReceipts = from d in db.TrnReceivingReceipts
                                    where d.SupplierId == Convert.ToInt32(supplierId)
                                    && d.BalanceAmount > 0
                                    && d.IsLocked == true
                                    select new Models.TrnReceivingReceipt
                                    {
                                        Id = d.Id,
                                        BranchId = d.BranchId,
                                        Branch = d.MstBranch.Branch,
                                        RRDate = d.RRDate.ToShortDateString(),
                                        RRNumber = d.RRNumber,
                                        SupplierId = d.SupplierId,
                                        Supplier = d.MstArticle.Article,
                                        TermId = d.TermId,
                                        Term = d.MstTerm.Term,
                                        DocumentReference = d.DocumentReference,
                                        ManualRRNumber = d.ManualRRNumber,
                                        Remarks = d.Remarks,
                                        Amount = d.Amount,
                                        WTaxAmount = d.WTaxAmount,
                                        PaidAmount = d.PaidAmount,
                                        AdjustmentAmount = d.AdjustmentAmount,
                                        BalanceAmount = d.BalanceAmount,
                                        ReceivedById = d.ReceivedById,
                                        ReceivedBy = d.MstUser4.FullName,
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

            return receivingReceipts.ToList();
        }

        // list receiving receipt by RRDate
        [Authorize]
        [HttpGet]
        [Route("api/listReceivingReceiptFilterByRRDate/{RRDate}")]
        public List<Models.TrnReceivingReceipt> listReceivingReceiptByRRDate(String RRDate)
        {
            var receivingReceipts = from d in db.TrnReceivingReceipts.OrderByDescending(d => d.Id)
                                    where d.RRDate == Convert.ToDateTime(RRDate)
                                    && d.BranchId == currentBranchId()
                                    select new Models.TrnReceivingReceipt
                                    {
                                        Id = d.Id,
                                        BranchId = d.BranchId,
                                        Branch = d.MstBranch.Branch,
                                        RRDate = d.RRDate.ToShortDateString(),
                                        RRNumber = d.RRNumber,
                                        SupplierId = d.SupplierId,
                                        Supplier = d.MstArticle.Article,
                                        TermId = d.TermId,
                                        Term = d.MstTerm.Term,
                                        DocumentReference = d.DocumentReference,
                                        ManualRRNumber = d.ManualRRNumber,
                                        Remarks = d.Remarks,
                                        Amount = d.Amount,
                                        WTaxAmount = d.WTaxAmount,
                                        PaidAmount = d.PaidAmount,
                                        AdjustmentAmount = d.AdjustmentAmount,
                                        BalanceAmount = d.BalanceAmount,
                                        ReceivedById = d.ReceivedById,
                                        ReceivedBy = d.MstUser4.FullName,
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

            return receivingReceipts.ToList();
        }

        // get receiving receipt last RRNumber
        [Authorize]
        [HttpGet]
        [Route("api/receivingReceiptLastRRNumber")]
        public Models.TrnReceivingReceipt getReceivingReceiptLastRRNumber()
        {
            var receivingReceipts = from d in db.TrnReceivingReceipts.OrderByDescending(d => d.RRNumber)
                                    select new Models.TrnReceivingReceipt
                                    {
                                        Id = d.Id,
                                        BranchId = d.BranchId,
                                        Branch = d.MstBranch.Branch,
                                        RRDate = d.RRDate.ToShortDateString(),
                                        RRNumber = d.RRNumber,
                                        SupplierId = d.SupplierId,
                                        Supplier = d.MstArticle.Article,
                                        TermId = d.TermId,
                                        Term = d.MstTerm.Term,
                                        DocumentReference = d.DocumentReference,
                                        ManualRRNumber = d.ManualRRNumber,
                                        Remarks = d.Remarks,
                                        Amount = d.Amount,
                                        WTaxAmount = d.WTaxAmount,
                                        PaidAmount = d.PaidAmount,
                                        AdjustmentAmount = d.AdjustmentAmount,
                                        BalanceAmount = d.BalanceAmount,
                                        ReceivedById = d.ReceivedById,
                                        ReceivedBy = d.MstUser4.FullName,
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

            return (Models.TrnReceivingReceipt)receivingReceipts.FirstOrDefault();
        }

        // add receiving receipt 
        [Authorize]
        [HttpPost]
        [Route("api/addReceivingReceipt")]
        public Int32 insertReceivingReceipt(Models.TrnReceivingReceipt receivingReceipt)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.TrnReceivingReceipt newReceivingReceipt = new Data.TrnReceivingReceipt();
                newReceivingReceipt.BranchId = receivingReceipt.BranchId;
                newReceivingReceipt.RRDate = Convert.ToDateTime(receivingReceipt.RRDate);
                newReceivingReceipt.RRNumber = receivingReceipt.RRNumber;
                newReceivingReceipt.SupplierId = receivingReceipt.SupplierId;
                newReceivingReceipt.TermId = receivingReceipt.TermId;
                newReceivingReceipt.DocumentReference = receivingReceipt.DocumentReference;
                newReceivingReceipt.ManualRRNumber = receivingReceipt.ManualRRNumber;
                newReceivingReceipt.Remarks = receivingReceipt.Remarks;
                newReceivingReceipt.Amount = 0;
                newReceivingReceipt.WTaxAmount = 0;
                newReceivingReceipt.PaidAmount = 0;
                newReceivingReceipt.AdjustmentAmount = 0;
                newReceivingReceipt.BalanceAmount = 0;
                newReceivingReceipt.ReceivedById = receivingReceipt.ReceivedById;
                newReceivingReceipt.PreparedById = receivingReceipt.PreparedById;
                newReceivingReceipt.CheckedById = receivingReceipt.CheckedById;
                newReceivingReceipt.ApprovedById = receivingReceipt.ApprovedById;
                newReceivingReceipt.IsLocked = false;
                newReceivingReceipt.CreatedById = userId;
                newReceivingReceipt.CreatedDateTime = DateTime.Now;
                newReceivingReceipt.UpdatedById = userId;
                newReceivingReceipt.UpdatedDateTime = DateTime.Now;

                db.TrnReceivingReceipts.InsertOnSubmit(newReceivingReceipt);
                db.SubmitChanges();

                return newReceivingReceipt.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update receiving receipt 
        [Authorize]
        [HttpPut]
        [Route("api/updateReceivingReceipt/{id}")]
        public HttpResponseMessage updateReceivingReceipt(String id, Models.TrnReceivingReceipt receivingReceipt)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var receivingReceipts = from d in db.TrnReceivingReceipts where d.Id == Convert.ToInt32(id) select d;
                if (receivingReceipts.Any())
                {
                    //  get disbursement line for CVId
                    var disbursementLineCVId = from d in db.TrnDisbursementLines where d.RRId == Convert.ToInt32(id) select d;

                    Decimal PaidAmount = 0;

                    if (disbursementLineCVId.Any())
                    {
                        Boolean disbursementHeaderIsLocked = (from d in db.TrnDisbursements where d.Id == disbursementLineCVId.First().CVId select d.IsLocked).SingleOrDefault();

                        // get disbursement line for paid amaount
                        var disbursementLines = from d in db.TrnDisbursementLines where d.RRId == Convert.ToInt32(id) select d;
                        if (disbursementLines.Any())
                        {
                            if (disbursementHeaderIsLocked == true)
                            {
                                PaidAmount = disbursementLines.Sum(d => d.Amount);
                            }
                            else
                            {
                                PaidAmount = 0;
                            }
                        }
                        else
                        {
                            PaidAmount = 0;
                        }
                    }

                    var updatereceivingReceipt = receivingReceipts.FirstOrDefault();
                    updatereceivingReceipt.BranchId = receivingReceipt.BranchId;
                    updatereceivingReceipt.RRDate = Convert.ToDateTime(receivingReceipt.RRDate);
                    updatereceivingReceipt.RRNumber = receivingReceipt.RRNumber;
                    updatereceivingReceipt.SupplierId = receivingReceipt.SupplierId;
                    updatereceivingReceipt.TermId = receivingReceipt.TermId;
                    updatereceivingReceipt.DocumentReference = receivingReceipt.DocumentReference;
                    updatereceivingReceipt.ManualRRNumber = receivingReceipt.ManualRRNumber;
                    updatereceivingReceipt.Remarks = receivingReceipt.Remarks;
                    updatereceivingReceipt.Amount = getAmountReceivingReceiptItem(Convert.ToInt32(id));
                    updatereceivingReceipt.WTaxAmount = 0;
                    updatereceivingReceipt.PaidAmount = PaidAmount;
                    updatereceivingReceipt.AdjustmentAmount = 0;
                    updatereceivingReceipt.BalanceAmount = getAmountReceivingReceiptItem(Convert.ToInt32(id)) - PaidAmount;
                    updatereceivingReceipt.ReceivedById = receivingReceipt.ReceivedById;
                    updatereceivingReceipt.PreparedById = receivingReceipt.PreparedById;
                    updatereceivingReceipt.CheckedById = receivingReceipt.CheckedById;
                    updatereceivingReceipt.ApprovedById = receivingReceipt.ApprovedById;
                    updatereceivingReceipt.IsLocked = true;
                    updatereceivingReceipt.UpdatedById = userId;
                    updatereceivingReceipt.UpdatedDateTime = DateTime.Now;

                    db.SubmitChanges();

                    if (updatereceivingReceipt.IsLocked == true)
                    {
                        inventory.InsertRRInventory(Convert.ToInt32(id));
                        journal.insertRRJournal(Convert.ToInt32(id));
                    }
                    else
                    {
                        inventory.deleteRRInventory(Convert.ToInt32(id));
                        journal.deleteRRJournal(Convert.ToInt32(id));
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

        // unlock receiving receipt  
        [Authorize]
        [HttpPut]
        [Route("api/updateReceivingReceiptIsLocked/{id}")]
        public HttpResponseMessage unlockReceivingReceipt(String id, Models.TrnReceivingReceipt receivingReceipt)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var receivingReceipts = from d in db.TrnReceivingReceipts where d.Id == Convert.ToInt32(id) select d;
                if (receivingReceipts.Any())
                {
                    var updatereceivingReceipt = receivingReceipts.FirstOrDefault();

                    updatereceivingReceipt.IsLocked = false;
                    updatereceivingReceipt.UpdatedById = userId;
                    updatereceivingReceipt.UpdatedDateTime = DateTime.Now;

                    db.SubmitChanges();

                    if (updatereceivingReceipt.IsLocked == true)
                    {
                        inventory.InsertRRInventory(Convert.ToInt32(id));
                        journal.insertRRJournal(Convert.ToInt32(id));
                    }
                    else
                    {
                        inventory.deleteRRInventory(Convert.ToInt32(id));
                        journal.deleteRRJournal(Convert.ToInt32(id));
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

        // delete receiving receipt  
        [Authorize]
        [HttpDelete]
        [Route("api/deleteRR/{id}")]
        public HttpResponseMessage deleteReceivingReceipt(String id)
        {
            try
            {
                var receivingReceipts = from d in db.TrnReceivingReceipts where d.Id == Convert.ToInt32(id) select d;
                if (receivingReceipts.Any())
                {
                    db.TrnReceivingReceipts.DeleteOnSubmit(receivingReceipts.First());
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
