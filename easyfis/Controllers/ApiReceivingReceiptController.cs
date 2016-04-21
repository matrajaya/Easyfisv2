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

        // ===================
        // Get Amount in Sales
        // ===================
        public Decimal getAmount(Int32 RRId)
        {
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        where d.RRId == RRId
                                        select new Models.TrnReceivingReceiptItem
                                        {
                                            Id = d.Id,
                                            RRId = d.RRId,
                                            RR = d.TrnReceivingReceipt.RRNumber,
                                            POId = d.POId,
                                            PO = d.TrnPurchaseOrder.PONumber,
                                            ItemId = d.ItemId,
                                            Item = d.MstArticle.Article,
                                            ItemCode = d.MstArticle.ManualArticleCode,
                                            Particulars = d.Particulars,
                                            UnitId = d.UnitId,
                                            Unit = d.MstUnit.Unit,
                                            Quantity = d.Quantity,
                                            Cost = d.Cost,
                                            Amount = d.Amount,
                                            VATId = d.VATId,
                                            VAT = d.MstTaxType.TaxType,
                                            VATPercentage = d.VATPercentage,
                                            VATAmount = d.VATAmount,
                                            WTAXId = d.WTAXId,
                                            WTAX = d.MstTaxType1.TaxType,
                                            WTAXPercentage = d.WTAXPercentage,
                                            WTAXAmount = d.WTAXAmount,
                                            BranchId = d.BranchId,
                                            Branch = d.MstBranch.Branch,
                                            BaseUnitId = d.BaseUnitId,
                                            BaseUnit = d.MstUnit1.Unit,
                                            BaseQuantity = d.BaseQuantity,
                                            BaseCost = d.BaseCost
                                        };

            Decimal amount;
            if (!receivingReceiptItems.Any())
            {
                amount = 0;
            }
            else
            {
                amount = receivingReceiptItems.Sum(d => d.Amount);
            }

            return amount;
        }

        // ======================
        // LIST Receiving Receipt
        // ======================
        [Route("api/listReceivingReceipt")]
        public List<Models.TrnReceivingReceipt> Get()
        {
            var branchIdCookie = Request.Headers.GetCookies("branchId").SingleOrDefault();
            var receivingReceipts = from d in db.TrnReceivingReceipts
                                    where d.BranchId == Convert.ToInt32(branchIdCookie["branchId"].Value)
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

        // ===================================
        // GET Receiving Receipt By SupplierId
        // ===================================
        [Route("api/receivingReceiptBySupplierId/{supplierId}")]
        public List<Models.TrnReceivingReceipt> GetReceivingReceiptBySupplierId(String supplierId)
        {
            var receivingReceipt_SupplierId = Convert.ToInt32(supplierId);
            var receivingReceipts = from d in db.TrnReceivingReceipts
                                    where d.SupplierId == receivingReceipt_SupplierId
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

        // ========================================
        // List Receiving Receipt Filter by RR Date
        // ========================================
        [Route("api/listReceivingReceiptFilterByRRDate/{RRDate}")]
        public List<Models.TrnReceivingReceipt> GetReceivingReceiptFilterByRRDate(String RRDate)
        {
            var branchIdCookie = Request.Headers.GetCookies("branchId").SingleOrDefault();
            var receivingReceipt_RRDate = Convert.ToDateTime(RRDate);
            var receivingReceipts = from d in db.TrnReceivingReceipts
                                    where d.RRDate == receivingReceipt_RRDate
                                    && d.BranchId == Convert.ToInt32(branchIdCookie["branchId"].Value)
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
                newReceivingReceipt.Amount = 0;
                newReceivingReceipt.WTaxAmount = 0;
                newReceivingReceipt.PaidAmount = 0;
                newReceivingReceipt.AdjustmentAmount = 0;
                newReceivingReceipt.BalanceAmount = 0;
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
                    // get Disbursement Line for Paid Amount
                    var disbursementLines = from d in db.TrnDisbursementLines
                                            where d.RRId == receivingReceipt_Id
                                            select new Models.TrnDisbursementLine
                                            {
                                                Id = d.Id,
                                                CVId = d.CVId,
                                                CV = d.TrnDisbursement.CVNumber,
                                                BranchId = d.BranchId,
                                                Branch = d.MstBranch.Branch,
                                                AccountId = d.AccountId,
                                                Account = d.MstAccount.Account,
                                                ArticleId = d.ArticleId,
                                                Article = d.MstArticle.Article,
                                                RRId = d.RRId,
                                                RR = d.TrnReceivingReceipt.RRNumber,
                                                Particulars = d.Particulars,
                                                Amount = d.Amount
                                            };

                    // get Disbursement Line for CVId
                    var disbursementLineCVIds = from d in db.TrnDisbursementLines
                                                where d.RRId == receivingReceipt_Id
                                                group d by new
                                                {
                                                    CVId = d.CVId,
                                                } into g
                                                select new Models.TrnDisbursementLine
                                                {
                                                    CVId = g.Key.CVId,
                                                };

                    Int32 CVId = 0;
                    foreach (var disbursementLineCVId in disbursementLineCVIds)
                    {
                        CVId = disbursementLineCVId.CVId;
                    }

                    Boolean disbursementHeaderIsLocked = (from d in db.TrnDisbursements where d.Id == CVId select d.IsLocked).SingleOrDefault();

                    Decimal PaidAmount = 0;
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

                    var updatereceivingReceipt = receivingReceipts.FirstOrDefault();

                    updatereceivingReceipt.BranchId = receivingReceipt.BranchId;
                    updatereceivingReceipt.RRDate = Convert.ToDateTime(receivingReceipt.RRDate);
                    updatereceivingReceipt.RRNumber = receivingReceipt.RRNumber;
                    updatereceivingReceipt.SupplierId = receivingReceipt.SupplierId;
                    updatereceivingReceipt.TermId = receivingReceipt.TermId;
                    updatereceivingReceipt.DocumentReference = receivingReceipt.DocumentReference;
                    updatereceivingReceipt.ManualRRNumber = receivingReceipt.ManualRRNumber;
                    updatereceivingReceipt.Remarks = receivingReceipt.Remarks;
                    updatereceivingReceipt.Amount = getAmount(receivingReceipt_Id);
                    updatereceivingReceipt.WTaxAmount = 0;
                    updatereceivingReceipt.PaidAmount = PaidAmount;
                    updatereceivingReceipt.AdjustmentAmount = 0;
                    updatereceivingReceipt.BalanceAmount = getAmount(receivingReceipt_Id) - PaidAmount;
                    updatereceivingReceipt.ReceivedById = receivingReceipt.ReceivedById;
                    updatereceivingReceipt.PreparedById = receivingReceipt.PreparedById;
                    updatereceivingReceipt.CheckedById = receivingReceipt.CheckedById;
                    updatereceivingReceipt.ApprovedById = receivingReceipt.ApprovedById;

                    updatereceivingReceipt.IsLocked = receivingReceipt.IsLocked;
                    updatereceivingReceipt.UpdatedById = mstUserId;
                    updatereceivingReceipt.UpdatedDateTime = date;

                    db.SubmitChanges();

                    if (updatereceivingReceipt.IsLocked == true)
                    {
                        inventory.InsertRRInventory(receivingReceipt_Id);
                        journal.insertRRJournal(receivingReceipt_Id);
                    }
                    else
                    {
                        inventory.deleteRRInventory(receivingReceipt_Id);
                        journal.deleteRRJournal(receivingReceipt_Id);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
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

                    db.SubmitChanges();

                    if (updatereceivingReceipt.IsLocked == true)
                    {
                        inventory.InsertRRInventory(receivingReceipt_Id);
                        journal.insertRRJournal(receivingReceipt_Id);
                    }
                    else
                    {
                        inventory.deleteRRInventory(receivingReceipt_Id);
                        journal.deleteRRJournal(receivingReceipt_Id);
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
