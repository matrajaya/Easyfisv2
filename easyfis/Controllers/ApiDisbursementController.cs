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

        // =================
        // LIST Disbursement
        // =================
        [Route("api/listDisbursement")]
        public List<Models.TrnDisbursement> Get()
        {
            var disbursements = from d in db.TrnDisbursements
                                select new Models.TrnDisbursement
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    CVNumber = d.CVNumber,
                                    CVDate = d.CVDate.ToShortDateString(),
                                    SupplierId = d.SupplierId,
                                    Supplier = d.MstArticle.Article,
                                    Payee = d.Payee,
                                    PayTypeId = d.PayTypeId,
                                    PayType = d.MstPayType.PayType,
                                    BankId = d.BankId,
                                    Bank = d.MstArticle1.Article,
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

        // ======================
        // GET Disbursement By Id
        // ======================
        [Route("api/disbursement/{id}")]
        public Models.TrnDisbursement GetDisbusementById(String id)
        {
            var disbursement_Id = Convert.ToInt32(id);
            var disbursements = from d in db.TrnDisbursements
                                where d.Id == disbursement_Id
                                select new Models.TrnDisbursement
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    CVNumber = d.CVNumber,
                                    CVDate = d.CVDate.ToShortDateString(),
                                    SupplierId = d.SupplierId,
                                    Supplier = d.MstArticle.Article,
                                    Payee = d.Payee,
                                    PayTypeId = d.PayTypeId,
                                    PayType = d.MstPayType.PayType,
                                    BankId = d.BankId,
                                    Bank = d.MstArticle1.Article,
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

        // ==================================
        // GET Disbursement Filter by CV Date
        // ==================================
        [Route("api/listDisbursementFilterByCVDate/{CVDate}")]
        public List<Models.TrnDisbursement> GetDisbusementFilterByCVDate(String CVDate)
        {
            var disbursement_CVDate = Convert.ToDateTime(CVDate);
            var disbursements = from d in db.TrnDisbursements
                                where d.CVDate == disbursement_CVDate
                                select new Models.TrnDisbursement
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    CVNumber = d.CVNumber,
                                    CVDate = d.CVDate.ToShortDateString(),
                                    SupplierId = d.SupplierId,
                                    Supplier = d.MstArticle.Article,
                                    Payee = d.Payee,
                                    PayTypeId = d.PayTypeId,
                                    PayType = d.MstPayType.PayType,
                                    BankId = d.BankId,
                                    Bank = d.MstArticle1.Article,
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

        // =================================
        // GET last CVNumber in Disbursement
        // =================================
        [Route("api/disbursementLastCVNumber")]
        public Models.TrnDisbursement GetDisbusementLastCVNumber()
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
                                    Supplier = d.MstArticle.Article,
                                    Payee = d.Payee,
                                    PayTypeId = d.PayTypeId,
                                    PayType = d.MstPayType.PayType,
                                    BankId = d.BankId,
                                    Bank = d.MstArticle1.Article,
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

        // ===========================
        // GET last Id in Disbursement
        // ===========================
        [Route("api/disbursementLastId")]
        public Models.TrnDisbursement GetDisbusementLastId()
        {
            var disbursements = from d in db.TrnDisbursements.OrderByDescending(d => d.Id)
                                select new Models.TrnDisbursement
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    CVNumber = d.CVNumber,
                                    CVDate = d.CVDate.ToShortDateString(),
                                    SupplierId = d.SupplierId,
                                    Supplier = d.MstArticle.Article,
                                    Payee = d.Payee,
                                    PayTypeId = d.PayTypeId,
                                    PayType = d.MstPayType.PayType,
                                    BankId = d.BankId,
                                    Bank = d.MstArticle1.Article,
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


        // ================
        // ADD Disbursement
        // ================
        [Route("api/addDisbursement")]
        public int Post(Models.TrnDisbursement disbursement)
        {
            try
            {
                var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

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

                newDisbursement.IsLocked = isLocked;
                newDisbursement.CreatedById = mstUserId;
                newDisbursement.CreatedDateTime = date;
                newDisbursement.UpdatedById = mstUserId;
                newDisbursement.UpdatedDateTime = date;

                db.TrnDisbursements.InsertOnSubmit(newDisbursement);
                db.SubmitChanges();

                return newDisbursement.Id;

            }
            catch
            {
                return 0;
            }
        }

        // ===================
        // UPDATE Disbursement
        // ===================
        [Route("api/updateDisbursement/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnDisbursement disbursement)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var disbursement_Id = Convert.ToInt32(id);
                var disbursements = from d in db.TrnDisbursements where d.Id == disbursement_Id select d;

                if (disbursements.Any())
                {
                    // disbursement Lines total Amount
                    var disbursementLinesTotalAmount = from d in db.TrnDisbursementLines
                                                       where d.CVId == disbursement_Id
                                                       group d by new
                                                       {
                                                           BranchId = d.BranchId,
                                                           AccountId = d.AccountId,
                                                           ArticleId = d.ArticleId
                                                       } into g
                                                       select new Models.TrnDisbursementLine
                                                       {
                                                           BranchId = g.Key.BranchId,
                                                           AccountId = g.Key.AccountId,
                                                           ArticleId = g.Key.ArticleId,
                                                           Amount = g.Sum(d => d.Amount)
                                                       };

                    Decimal Amount = 0;
                    if (disbursementLinesTotalAmount.Any())
                    {
                        foreach (var disbursementLinesAmount in disbursementLinesTotalAmount)
                        {
                            Amount = disbursementLinesAmount.Amount;
                        }
                    }
                    else
                    {
                        Amount = 0;
                    }

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
                    //updateDisbursement.Amount = disbursement.Amount;
                    updateDisbursement.Amount = Amount;
                    updateDisbursement.IsCrossCheck = disbursement.IsCrossCheck;
                    updateDisbursement.IsClear = disbursement.IsClear;
                    updateDisbursement.PreparedById = disbursement.PreparedById;
                    updateDisbursement.CheckedById = disbursement.CheckedById;
                    updateDisbursement.ApprovedById = disbursement.ApprovedById;

                    updateDisbursement.IsLocked = disbursement.IsLocked;
                    updateDisbursement.UpdatedById = mstUserId;
                    updateDisbursement.UpdatedDateTime = date;

                    db.SubmitChanges();

                    if (updateDisbursement.IsLocked == true)
                    {
                        journal.insertCVJournal(disbursement_Id);

                        UpdateAPDisbursement(disbursement_Id);
                    }
                    else
                    {
                        journal.deleteCVJournal(disbursement_Id);
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

        // ==============================
        // UPDATE Disbursement - IsLocked
        // ==============================
        [Route("api/updateDisbursementIsLocked/{id}")]
        public HttpResponseMessage PutUpdateDisbursementIsLocked(String id, Models.TrnDisbursement disbursement)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var disbursement_Id = Convert.ToInt32(id);
                var disbursements = from d in db.TrnDisbursements where d.Id == disbursement_Id select d;

                if (disbursements.Any())
                {
                    var updateDisbursement = disbursements.FirstOrDefault();

                    updateDisbursement.IsLocked = disbursement.IsLocked;
                    updateDisbursement.UpdatedById = mstUserId;
                    updateDisbursement.UpdatedDateTime = date;

                    if (updateDisbursement.IsLocked == true)
                    {
                        journal.insertCVJournal(disbursement_Id);
                    }
                    else
                    {
                        journal.deleteCVJournal(disbursement_Id);
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

        // ===================
        // DELETE Disbursement
        // ===================
        [Route("api/deleteDisbursement/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var disbursement_Id = Convert.ToInt32(id);
                var disbursements = from d in db.TrnDisbursements where d.Id == disbursement_Id select d;

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

        public void UpdateAPDisbursement(Int32 CVId)
        {
            // disbursement Header
            var disbursementHeader = from d in db.TrnDisbursements
                                     where d.Id == CVId
                                     select new Models.TrnDisbursement
                                     {
                                         Id = d.Id,
                                         BranchId = d.BranchId,
                                         Branch = d.MstBranch.Branch,
                                         BranchCode = d.MstBranch.BranchCode,
                                         CVNumber = d.CVNumber,
                                         CVDate = d.CVDate.ToShortDateString(),
                                         SupplierId = d.SupplierId,
                                         Supplier = d.MstArticle.Article,
                                         Payee = d.Payee,
                                         PayTypeId = d.PayTypeId,
                                         PayType = d.MstPayType.PayType,
                                         BankId = d.BankId,
                                         Bank = d.MstArticle1.Article,
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

            // disbursement Lines
            var disbursementLines = from d in db.TrnDisbursementLines
                                    where d.CVId == CVId
                                    group d by new
                                    {
                                        RRId = d.RRId
                                    } into g
                                    select new Models.TrnDisbursementLine
                                    {
                                        RRId = g.Key.RRId
                                    };

            try
            {
                if (disbursementHeader.Any())
                {
                    if (disbursementLines.Any())
                    {
                        foreach (var rrIdDisursementLines in disbursementLines)
                        {
                            if (rrIdDisursementLines.RRId != null)
                            {
                                updateAP(Convert.ToInt32(rrIdDisursementLines.RRId));
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
            }
        }

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

            try
            {
                if (receivingReceipts.Any())
                {
                    var receivingReceiptsUpdate = from d in db.TrnReceivingReceipts where d.Id == RRId select d;
                    if (receivingReceiptsUpdate.Any())
                    {
                        Decimal PaidAmount = 0;
                        Decimal AdjustmentAmount = 0;

                        var disbursementLines = from d in db.TrnDisbursementLines
                                                where d.RRId == RRId
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

                        var journalVoucherLines = from d in db.TrnJournalVoucherLines
                                                  where d.APRRId == RRId
                                                  select new Models.TrnJournalVoucherLine
                                                  {
                                                      Id = d.Id,
                                                      JVId = d.JVId,
                                                      BranchId = d.BranchId,
                                                      Branch = d.MstBranch.Branch,
                                                      AccountId = d.AccountId,
                                                      Account = d.MstAccount.Account,
                                                      ArticleId = d.ArticleId,
                                                      Article = d.MstArticle.Article,
                                                      Particulars = d.Particulars,
                                                      DebitAmount = d.DebitAmount,
                                                      CreditAmount = d.CreditAmount,
                                                      APRRId = d.APRRId,
                                                      APRR = d.TrnReceivingReceipt.RRNumber,
                                                      APRRBranch = d.TrnReceivingReceipt.MstBranch.Branch,
                                                      ARSIId = d.ARSIId,
                                                      ARSI = d.TrnSalesInvoice.SINumber,
                                                      ARSIBranch = d.TrnSalesInvoice.MstBranch.Branch,
                                                      IsClear = d.IsClear
                                                  };

                        Decimal DebitAmount;
                        Decimal CreditAmount;
                        DebitAmount = journalVoucherLines.Sum(d => d.DebitAmount);
                        CreditAmount = journalVoucherLines.Sum(d => d.CreditAmount);

                        PaidAmount = disbursementLines.Sum(d => d.Amount);
                        AdjustmentAmount = CreditAmount - DebitAmount;

                        var updateRR = receivingReceiptsUpdate.FirstOrDefault();
                        updateRR.PaidAmount = PaidAmount;
                        updateRR.AdjustmentAmount = AdjustmentAmount;
                        db.SubmitChanges();

                        Decimal RRamount = 0;
                        Decimal RRWTAXAmount = 0;
                        Decimal RRPaidAmount = 0;
                        foreach (var rrForUpdate in receivingReceipts)
                        {
                            RRamount = rrForUpdate.Amount;
                            RRWTAXAmount = rrForUpdate.WTaxAmount;
                            RRPaidAmount = rrForUpdate.PaidAmount;
                        }

                        updateRR.BalanceAmount = (RRamount - RRWTAXAmount - RRPaidAmount) + AdjustmentAmount;
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
