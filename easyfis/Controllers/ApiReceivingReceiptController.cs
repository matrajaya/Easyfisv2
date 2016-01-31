using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiReceivingReceiptController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ======================
        // LIST Receiving Receipt
        // ======================
        [Route("api/listReceivingReceipt")]
        public List<Models.TrnReceivingReceipt> Get()
        {
            var receivingReceipts = from d in db.TrnReceivingReceipts
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

        // ===========================
        // GET Receiving Receipt By Id
        // ===========================
        [Route("api/receivingReceipt/{Id}")]
        public Models.TrnReceivingReceipt GetReceivingReceiptById(String Id)
        {
            var receivingReceipt_Id = Convert.ToInt32(Id);
            var receivingReceipts = from d in db.TrnReceivingReceipts
                                    where d.Id == receivingReceipt_Id
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

        // ========================================
        // List Receiving Receipt Filter by RR Date
        // ========================================
        [Route("api/listReceivingReceiptFilterByRRDate/{RRDate}")]
        public List<Models.TrnReceivingReceipt> GetReceivingReceiptFilterByRRDate(String RRDate)
        {
            var receivingReceipt_RRDate = Convert.ToDateTime(RRDate);
            var receivingReceipts = from d in db.TrnReceivingReceipts
                                    where d.RRDate == receivingReceipt_RRDate
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

        // =======================================
        // GET Last RRNumber in Receiving Receipts
        // =======================================
        [Route("api/receivingReceiptLastRRNumber")]
        public Models.TrnReceivingReceipt GetLastRRNumber()
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

        // =================================
        // GET Last Id in Receiving Receipts
        // =================================
        [Route("api/receivingReceiptLastId")]
        public Models.TrnReceivingReceipt GetLastId()
        {
            var receivingReceipts = from d in db.TrnReceivingReceipts.OrderByDescending(d => d.Id)
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

        // ======
        // ADD RR
        // ======
        [Route("api/addReceivingReceipt")]
        public int Post(Models.TrnReceivingReceipt receivingReceipt)
        {
            try
            {
                var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.TrnReceivingReceipt newReceivingReceipt = new Data.TrnReceivingReceipt();

                newReceivingReceipt.BranchId = receivingReceipt.BranchId;
                newReceivingReceipt.RRDate = Convert.ToDateTime(receivingReceipt.RRDate);
                newReceivingReceipt.RRNumber = receivingReceipt.RRNumber;
                newReceivingReceipt.SupplierId = receivingReceipt.SupplierId;
                newReceivingReceipt.TermId = receivingReceipt.TermId;
                newReceivingReceipt.DocumentReference = receivingReceipt.DocumentReference;
                newReceivingReceipt.ManualRRNumber = receivingReceipt.ManualRRNumber;
                newReceivingReceipt.Remarks = receivingReceipt.Remarks;
                newReceivingReceipt.Amount = receivingReceipt.Amount;
                newReceivingReceipt.WTaxAmount = receivingReceipt.WTaxAmount;
                newReceivingReceipt.PaidAmount = receivingReceipt.PaidAmount;
                newReceivingReceipt.AdjustmentAmount = receivingReceipt.AdjustmentAmount;
                newReceivingReceipt.BalanceAmount = receivingReceipt.BalanceAmount;
                newReceivingReceipt.ReceivedById = receivingReceipt.ReceivedById;
                newReceivingReceipt.PreparedById = receivingReceipt.PreparedById;
                newReceivingReceipt.CheckedById = receivingReceipt.CheckedById;
                newReceivingReceipt.ApprovedById = receivingReceipt.ApprovedById;

                newReceivingReceipt.IsLocked = isLocked;
                newReceivingReceipt.CreatedById = mstUserId;
                newReceivingReceipt.CreatedDateTime = date;
                newReceivingReceipt.UpdatedById = mstUserId;
                newReceivingReceipt.UpdatedDateTime = date;

                db.TrnReceivingReceipts.InsertOnSubmit(newReceivingReceipt);
                db.SubmitChanges();

                return newReceivingReceipt.Id;

            }
            catch
            {
                return 0;
            }
        }

        // =========
        // UPDATE RR
        // =========
        [Route("api/updateReceivingReceipt/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnReceivingReceipt receivingReceipt)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var receivingReceipt_Id = Convert.ToInt32(id);
                var receivingReceipts = from d in db.TrnReceivingReceipts where d.Id == receivingReceipt_Id select d;

                if (receivingReceipts.Any())
                {
                    var updatereceivingReceipt = receivingReceipts.FirstOrDefault();

                    updatereceivingReceipt.BranchId = receivingReceipt.BranchId;
                    updatereceivingReceipt.RRDate = Convert.ToDateTime(receivingReceipt.RRDate);
                    updatereceivingReceipt.RRNumber = receivingReceipt.RRNumber;
                    updatereceivingReceipt.SupplierId = receivingReceipt.SupplierId;
                    updatereceivingReceipt.TermId = receivingReceipt.TermId;
                    updatereceivingReceipt.DocumentReference = receivingReceipt.DocumentReference;
                    updatereceivingReceipt.ManualRRNumber = receivingReceipt.ManualRRNumber;
                    updatereceivingReceipt.Remarks = receivingReceipt.Remarks;
                    updatereceivingReceipt.Amount = receivingReceipt.Amount;
                    updatereceivingReceipt.WTaxAmount = receivingReceipt.WTaxAmount;
                    updatereceivingReceipt.PaidAmount = receivingReceipt.PaidAmount;
                    updatereceivingReceipt.AdjustmentAmount = receivingReceipt.AdjustmentAmount;
                    updatereceivingReceipt.BalanceAmount = receivingReceipt.BalanceAmount;
                    updatereceivingReceipt.ReceivedById = receivingReceipt.ReceivedById;
                    updatereceivingReceipt.PreparedById = receivingReceipt.PreparedById;
                    updatereceivingReceipt.CheckedById = receivingReceipt.CheckedById;
                    updatereceivingReceipt.ApprovedById = receivingReceipt.ApprovedById;

                    updatereceivingReceipt.IsLocked = receivingReceipt.IsLocked;
                    updatereceivingReceipt.UpdatedById = mstUserId;
                    updatereceivingReceipt.UpdatedDateTime = date;

                    if (updatereceivingReceipt.IsLocked == true)
                    {
                        Business.Inventory inventory = new Business.Inventory();
                        inventory.InsertRRInventory(receivingReceipt_Id);
                    }
                    else
                    {
                        Business.Inventory inventory = new Business.Inventory();
                        inventory.deleteRRInventory(receivingReceipt_Id);
                    }

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

        // ====================
        // UPDATE RR - isLocked
        // ====================
        [Route("api/updateReceivingReceiptIsLocked/{id}")]
        public HttpResponseMessage PutUpdateRRIsLocked(String id, Models.TrnReceivingReceipt receivingReceipt)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var receivingReceipt_Id = Convert.ToInt32(id);
                var receivingReceipts = from d in db.TrnReceivingReceipts where d.Id == receivingReceipt_Id select d;

                if (receivingReceipts.Any())
                {
                    var updatereceivingReceipt = receivingReceipts.FirstOrDefault();
                   
                    updatereceivingReceipt.IsLocked = receivingReceipt.IsLocked;
                    updatereceivingReceipt.UpdatedById = mstUserId;
                    updatereceivingReceipt.UpdatedDateTime = date;

                    if (updatereceivingReceipt.IsLocked == true)
                    {
                        Business.Inventory inventory = new Business.Inventory();
                        inventory.InsertRRInventory(receivingReceipt_Id);
                    }
                    else
                    {
                        Business.Inventory inventory = new Business.Inventory();
                        inventory.deleteRRInventory(receivingReceipt_Id);
                    }

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

        // =========
        // DELETE RR
        // =========
        [Route("api/deleteRR/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var receivingReceipt_Id = Convert.ToInt32(id);
                var receivingReceipts = from d in db.TrnReceivingReceipts where d.Id == receivingReceipt_Id select d;

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
