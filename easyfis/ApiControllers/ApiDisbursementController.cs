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
    public class ApiDisbursementController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();
        private Business.PostJournal journal = new Business.PostJournal();

        // current branch Id
        public Int32 currentBranchId()
        {
            return (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.BranchId).SingleOrDefault();
        }

        // update AP
        public void updateAP(Int32 RRId)
        {
            var receivingReceipts = from d in db.TrnReceivingReceipts
                                    where d.Id == RRId
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

            if (receivingReceipts.Any())
            {
                var receivingReceiptsUpdate = from d in db.TrnReceivingReceipts where d.Id == RRId select d;
                if (receivingReceiptsUpdate.Any())
                {
                    Decimal DebitAmount = 0;
                    Decimal CreditAmount = 0;

                    var journalVoucherLines = from d in db.TrnJournalVoucherLines where d.APRRId == RRId select d;
                    if (journalVoucherLines.Any())
                    {
                        DebitAmount = journalVoucherLines.Sum(d => d.DebitAmount);
                        CreditAmount = journalVoucherLines.Sum(d => d.CreditAmount);
                    }

                    Decimal PaidAmount = 0;
                    Decimal AdjustmentAmount = 0;
                    var disbursementLineCVId = from d in db.TrnDisbursementLines where d.RRId == RRId select d;
                    if (disbursementLineCVId.Any())
                    {
                        Boolean disbursementHeaderIsLocked = (from d in db.TrnDisbursements where d.Id == disbursementLineCVId.First().CVId select d.IsLocked).SingleOrDefault();
                        if (disbursementHeaderIsLocked == true)
                        {
                            var disbursementLines = from d in db.TrnDisbursementLines where d.RRId == RRId select d;

                            PaidAmount = disbursementLines.Sum(d => d.Amount);
                            AdjustmentAmount = CreditAmount - DebitAmount;
                        }
                    }

                    var updateReceivingReceipt = receivingReceiptsUpdate.FirstOrDefault();
                    updateReceivingReceipt.PaidAmount = PaidAmount;
                    updateReceivingReceipt.AdjustmentAmount = AdjustmentAmount;
                    db.SubmitChanges();

                    Decimal ReceivingReceiptAmount = 0;
                    Decimal ReceivingReceiptWTAXAmount = 0;
                    Decimal ReceivingReceiptPaidAmount = 0;

                    foreach (var receivingReceipt in receivingReceipts)
                    {
                        ReceivingReceiptAmount = receivingReceipt.Amount;
                        ReceivingReceiptWTAXAmount = receivingReceipt.WTaxAmount;
                        ReceivingReceiptPaidAmount = receivingReceipt.PaidAmount;
                    }

                    updateReceivingReceipt.BalanceAmount = (ReceivingReceiptAmount - ReceivingReceiptWTAXAmount - ReceivingReceiptPaidAmount) + AdjustmentAmount;
                    db.SubmitChanges();
                }
            }
        }

        // update AP
        public void updateAPDisbursement(Int32 CVId)
        {
            var disbursementLines = from d in db.TrnDisbursementLines where d.CVId == CVId select d;
            if (disbursementLines.Any())
            {
                if (disbursementLines.First().RRId != null)
                {
                    updateAP(Convert.ToInt32(disbursementLines.First().RRId));
                }
            }
        }

        // list disbursement
        [Authorize]
        [HttpGet]
        [Route("api/listDisbursement")]
        public List<Models.TrnDisbursement> listDisbursement()
        {
            var disbursements = from d in db.TrnDisbursements.OrderByDescending(d => d.Id)
                                where d.BranchId == currentBranchId()
                                select new Models.TrnDisbursement
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    CVNumber = d.CVNumber,
                                    CVDate = d.CVDate.ToShortDateString(),
                                    SupplierId = d.SupplierId,
                                    Supplier = d.MstArticle1.Article,
                                    Payee = d.Payee,
                                    PayTypeId = d.PayTypeId,
                                    PayType = d.MstPayType.PayType,
                                    BankId = d.BankId,
                                    Bank = d.MstArticle.Article,
                                    ManualCVNumber = d.ManualCVNumber,
                                    Particulars = d.Particulars,
                                    CheckNumber = d.CheckNumber,
                                    CheckDate = d.CheckDate.ToShortDateString(),
                                    Amount = d.Amount,
                                    IsCrossCheck = d.IsCrossCheck,
                                    IsClear = d.IsClear,
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
                                    UpdatedBy = d.MstUser4.FullName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                };

            return disbursements.ToList();
        }

        // get disbursement by Id
        [Authorize]
        [HttpGet]
        [Route("api/disbursement/{id}")]
        public Models.TrnDisbursement getDisbusementById(String id)
        {
            var disbursements = from d in db.TrnDisbursements
                                where d.Id == Convert.ToInt32(id)
                                select new Models.TrnDisbursement
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    CVNumber = d.CVNumber,
                                    CVDate = d.CVDate.ToShortDateString(),
                                    SupplierId = d.SupplierId,
                                    Supplier = d.MstArticle1.Article,
                                    Payee = d.Payee,
                                    PayTypeId = d.PayTypeId,
                                    PayType = d.MstPayType.PayType,
                                    BankId = d.BankId,
                                    Bank = d.MstArticle.Article,
                                    ManualCVNumber = d.ManualCVNumber,
                                    Particulars = d.Particulars,
                                    CheckNumber = d.CheckNumber,
                                    CheckDate = d.CheckDate.ToShortDateString(),
                                    Amount = d.Amount,
                                    IsCrossCheck = d.IsCrossCheck,
                                    IsClear = d.IsClear,
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
                                    UpdatedBy = d.MstUser4.FullName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                };

            return (Models.TrnDisbursement)disbursements.FirstOrDefault();
        }

        // list disbursement by CVDate
        [Authorize]
        [HttpGet]
        [Route("api/listDisbursementFilterByCVDate/{CVDate}")]
        public List<Models.TrnDisbursement> listDisbusementByCVDate(String CVDate)
        {
            var disbursements = from d in db.TrnDisbursements.OrderByDescending(d => d.Id)
                                where d.CVDate == Convert.ToDateTime(CVDate)
                                && d.BranchId == currentBranchId()
                                select new Models.TrnDisbursement
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    CVNumber = d.CVNumber,
                                    CVDate = d.CVDate.ToShortDateString(),
                                    SupplierId = d.SupplierId,
                                    Supplier = d.MstArticle1.Article,
                                    Payee = d.Payee,
                                    PayTypeId = d.PayTypeId,
                                    PayType = d.MstPayType.PayType,
                                    BankId = d.BankId,
                                    Bank = d.MstArticle.Article,
                                    ManualCVNumber = d.ManualCVNumber,
                                    Particulars = d.Particulars,
                                    CheckNumber = d.CheckNumber,
                                    CheckDate = d.CheckDate.ToShortDateString(),
                                    Amount = d.Amount,
                                    IsCrossCheck = d.IsCrossCheck,
                                    IsClear = d.IsClear,
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
                                    UpdatedBy = d.MstUser4.FullName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                };

            return disbursements.ToList();
        }

        // get disbursement last CVNumber
        [Authorize]
        [HttpGet]
        [Route("api/disbursementLastCVNumber")]
        public Models.TrnDisbursement getDisbusementLastCVNumber()
        {
            var disbursements = from d in db.TrnDisbursements.OrderByDescending(d => d.CVNumber)
                                select new Models.TrnDisbursement
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    CVNumber = d.CVNumber,
                                    CVDate = d.CVDate.ToShortDateString(),
                                    SupplierId = d.SupplierId,
                                    Supplier = d.MstArticle1.Article,
                                    Payee = d.Payee,
                                    PayTypeId = d.PayTypeId,
                                    PayType = d.MstPayType.PayType,
                                    BankId = d.BankId,
                                    Bank = d.MstArticle.Article,
                                    ManualCVNumber = d.ManualCVNumber,
                                    Particulars = d.Particulars,
                                    CheckNumber = d.CheckNumber,
                                    CheckDate = d.CheckDate.ToShortDateString(),
                                    Amount = d.Amount,
                                    IsCrossCheck = d.IsCrossCheck,
                                    IsClear = d.IsClear,
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
                                    UpdatedBy = d.MstUser4.FullName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                };

            return (Models.TrnDisbursement)disbursements.FirstOrDefault();
        }

        // list disbursement by BankId and by CVDate for Bank Recon
        [Authorize]
        [HttpGet]
        [Route("api/listDisbursementByBankIdByCVDate/{bankId}/{dateStart}/{dateEnd}")]
        public List<Models.TrnDisbursement> listDisbusementByBankIdByCVDate(String bankId, String dateStart, String dateEnd)
        {
            var disbursements = from d in db.TrnDisbursements
                                where d.BankId == Convert.ToInt32(bankId)
                                && d.CVDate >= Convert.ToDateTime(dateStart)
                                && d.CVDate <= Convert.ToDateTime(dateEnd)
                                select new Models.TrnDisbursement
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    CVNumber = d.CVNumber,
                                    CVDate = d.CVDate.ToShortDateString(),
                                    SupplierId = d.SupplierId,
                                    Supplier = d.MstArticle1.Article,
                                    Payee = d.Payee,
                                    PayTypeId = d.PayTypeId,
                                    PayType = d.MstPayType.PayType,
                                    BankId = d.BankId,
                                    Bank = d.MstArticle.Article,
                                    ManualCVNumber = d.ManualCVNumber,
                                    Particulars = d.Particulars,
                                    CheckNumber = d.CheckNumber,
                                    CheckDate = d.CheckDate.ToShortDateString(),
                                    Amount = d.Amount,
                                    IsCrossCheck = d.IsCrossCheck,
                                    IsClear = d.IsClear,
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
                                    UpdatedBy = d.MstUser4.FullName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                };

            return disbursements.ToList();
        }

        // add disbursement
        [Authorize]
        [HttpPost]
        [Route("api/addDisbursement")]
        public Int32 insertDisbursement(Models.TrnDisbursement disbursement)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.TrnDisbursement newDisbursement = new Data.TrnDisbursement();
                newDisbursement.BranchId = disbursement.BranchId;
                newDisbursement.CVNumber = disbursement.CVNumber;
                newDisbursement.CVDate = Convert.ToDateTime(disbursement.CVDate);
                newDisbursement.SupplierId = disbursement.SupplierId;
                newDisbursement.Payee = disbursement.Payee;
                newDisbursement.PayTypeId = disbursement.PayTypeId;
                newDisbursement.BankId = disbursement.BankId;
                newDisbursement.ManualCVNumber = disbursement.ManualCVNumber;
                newDisbursement.Particulars = disbursement.Particulars;
                newDisbursement.CheckNumber = disbursement.CheckNumber;
                newDisbursement.CheckDate = Convert.ToDateTime(disbursement.CheckDate);
                newDisbursement.Amount = disbursement.Amount;
                newDisbursement.IsCrossCheck = disbursement.IsCrossCheck;
                newDisbursement.IsClear = disbursement.IsClear;
                newDisbursement.PreparedById = disbursement.PreparedById;
                newDisbursement.CheckedById = disbursement.CheckedById;
                newDisbursement.ApprovedById = disbursement.ApprovedById;
                newDisbursement.IsLocked = false;
                newDisbursement.CreatedById = userId;
                newDisbursement.CreatedDateTime = DateTime.Now;
                newDisbursement.UpdatedById = userId;
                newDisbursement.UpdatedDateTime = DateTime.Now;

                db.TrnDisbursements.InsertOnSubmit(newDisbursement);
                db.SubmitChanges();

                return newDisbursement.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update disbursement
        [Authorize]
        [HttpPut]
        [Route("api/updateDisbursement/{id}")]
        public HttpResponseMessage updateDisbursement(String id, Models.TrnDisbursement disbursement)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var disbursements = from d in db.TrnDisbursements where d.Id == Convert.ToInt32(id) select d;
                if (disbursements.Any())
                {
                    var updateDisbursement = disbursements.FirstOrDefault();
                    updateDisbursement.BranchId = disbursement.BranchId;
                    updateDisbursement.CVNumber = disbursement.CVNumber;
                    updateDisbursement.CVDate = Convert.ToDateTime(disbursement.CVDate);
                    updateDisbursement.SupplierId = disbursement.SupplierId;
                    updateDisbursement.Payee = disbursement.Payee;
                    updateDisbursement.PayTypeId = disbursement.PayTypeId;
                    updateDisbursement.BankId = disbursement.BankId;
                    updateDisbursement.ManualCVNumber = disbursement.ManualCVNumber;
                    updateDisbursement.Particulars = disbursement.Particulars;
                    updateDisbursement.CheckNumber = disbursement.CheckNumber;
                    updateDisbursement.CheckDate = Convert.ToDateTime(disbursement.CheckDate);
                    updateDisbursement.Amount = updateDisbursement.TrnDisbursementLines.Sum(d => d.Amount);
                    updateDisbursement.IsCrossCheck = disbursement.IsCrossCheck;
                    updateDisbursement.IsClear = disbursement.IsClear;
                    updateDisbursement.PreparedById = disbursement.PreparedById;
                    updateDisbursement.CheckedById = disbursement.CheckedById;
                    updateDisbursement.ApprovedById = disbursement.ApprovedById;
                    updateDisbursement.IsLocked = true;
                    updateDisbursement.UpdatedById = userId;
                    updateDisbursement.UpdatedDateTime = DateTime.Now;

                    db.SubmitChanges();

                    journal.insertCVJournal(Convert.ToInt32(id));
                    updateAPDisbursement(Convert.ToInt32(id));

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

        // unlock disbursement
        [Authorize]
        [HttpPut]
        [Route("api/updateDisbursementIsLocked/{id}")]
        public HttpResponseMessage unlockDisbursement(String id, Models.TrnDisbursement disbursement)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var disbursements = from d in db.TrnDisbursements where d.Id == Convert.ToInt32(id) select d;
                if (disbursements.Any())
                {
                    var updateDisbursement = disbursements.FirstOrDefault();
                    updateDisbursement.IsLocked = false;
                    updateDisbursement.UpdatedById = userId;
                    updateDisbursement.UpdatedDateTime = DateTime.Now;

                    db.SubmitChanges();

                    journal.deleteCVJournal(Convert.ToInt32(id));
                    updateAPDisbursement(Convert.ToInt32(id));

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

        // delete disbursement
        [Authorize]
        [HttpDelete]
        [Route("api/deleteDisbursement/{id}")]
        public HttpResponseMessage deleteDisbursement(String id)
        {
            try
            {
                var disbursements = from d in db.TrnDisbursements where d.Id == Convert.ToInt32(id) select d;
                if (disbursements.Any())
                {
                    db.TrnDisbursements.DeleteOnSubmit(disbursements.First());
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
