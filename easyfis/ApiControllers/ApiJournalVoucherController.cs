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
    public class ApiJournalVoucherController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();
        private Business.PostJournal postJournal = new Business.PostJournal();

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

        // list journal voucher
        [Authorize]
        [HttpGet]
        [Route("api/listJournalVoucher")]
        public List<Models.TrnJournalVoucher> listJournalVoucher()
        {
            var journalVouchers = from d in db.TrnJournalVouchers.OrderByDescending(d => d.Id)
                                  select new Models.TrnJournalVoucher
                                  {
                                      Id = d.Id,
                                      BranchId = d.BranchId,
                                      Branch = d.MstBranch.Branch,
                                      JVNumber = d.JVNumber,
                                      JVDate = d.JVDate.ToShortDateString(),
                                      Particulars = d.Particulars,
                                      ManualJVNumber = d.ManualJVNumber,
                                      PreparedById = d.PreparedById,
                                      PreparedBy = d.MstUser.FullName,
                                      CheckedById = d.CheckedById,
                                      CheckedBy = d.MstUser1.FullName,
                                      ApprovedById = d.ApprovedById,
                                      ApprovedBy = d.MstUser2.FullName,
                                      IsLocked = d.IsLocked,
                                      CreatedById = d.CreatedById,
                                      CreatedBy = d.MstUser3.FullName,
                                      CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                      UpdatedById = d.UpdatedById,
                                      UpdatedBy = d.MstUser4.FullName,
                                      UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                  };

            return journalVouchers.ToList();
        }

        // get journal voucher last JVNumber
        [Authorize]
        [HttpGet]
        [Route("api/journalVoucherLastJVNumber")]
        public Models.TrnJournalVoucher getJournalVoucherLastJVNumber()
        {
            var journalVouchers = from d in db.TrnJournalVouchers.OrderByDescending(d => d.JVNumber)
                                  select new Models.TrnJournalVoucher
                                  {
                                      Id = d.Id,
                                      BranchId = d.BranchId,
                                      Branch = d.MstBranch.Branch,
                                      JVNumber = d.JVNumber,
                                      JVDate = d.JVDate.ToShortDateString(),
                                      Particulars = d.Particulars,
                                      ManualJVNumber = d.ManualJVNumber,
                                      PreparedById = d.PreparedById,
                                      PreparedBy = d.MstUser.FullName,
                                      CheckedById = d.CheckedById,
                                      CheckedBy = d.MstUser1.FullName,
                                      ApprovedById = d.ApprovedById,
                                      ApprovedBy = d.MstUser2.FullName,
                                      IsLocked = d.IsLocked,
                                      CreatedById = d.CreatedById,
                                      CreatedBy = d.MstUser3.FullName,
                                      CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                      UpdatedById = d.UpdatedById,
                                      UpdatedBy = d.MstUser4.FullName,
                                      UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                  };

            return (Models.TrnJournalVoucher)journalVouchers.FirstOrDefault();
        }

        // get journal voucher by Id
        [Authorize]
        [HttpGet]
        [Route("api/journalVoucher/{Id}")]
        public Models.TrnJournalVoucher getJournalVoucherById(String Id)
        {
            var journalVouchers = from d in db.TrnJournalVouchers
                                  where d.Id == Convert.ToInt32(Id)
                                  select new Models.TrnJournalVoucher
                                  {
                                      Id = d.Id,
                                      BranchId = d.BranchId,
                                      Branch = d.MstBranch.Branch,
                                      JVNumber = d.JVNumber,
                                      JVDate = d.JVDate.ToShortDateString(),
                                      Particulars = d.Particulars,
                                      ManualJVNumber = d.ManualJVNumber,
                                      PreparedById = d.PreparedById,
                                      PreparedBy = d.MstUser.FullName,
                                      CheckedById = d.CheckedById,
                                      CheckedBy = d.MstUser1.FullName,
                                      ApprovedById = d.ApprovedById,
                                      ApprovedBy = d.MstUser2.FullName,
                                      IsLocked = d.IsLocked,
                                      CreatedById = d.CreatedById,
                                      CreatedBy = d.MstUser3.FullName,
                                      CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                      UpdatedById = d.UpdatedById,
                                      UpdatedBy = d.MstUser4.FullName,
                                      UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                  };

            return (Models.TrnJournalVoucher)journalVouchers.FirstOrDefault();
        }

        // list journal voucher by JVDate
        [Authorize]
        [HttpGet]
        [Route("api/listJournalVoucherByJVDate/{JVStartDate}/{JVEndDate}")]
        public List<Models.TrnJournalVoucher> listJournalVoucherByJVDate(String JVStartDate, String JVEndDate)
        {
            var journalVouchers = from d in db.TrnJournalVouchers.OrderByDescending(d => d.Id)
                                  where d.JVDate >= Convert.ToDateTime(JVStartDate)
                                  && d.JVDate <= Convert.ToDateTime(JVEndDate)
                                  && d.BranchId == currentBranchId()
                                  select new Models.TrnJournalVoucher
                                  {
                                      Id = d.Id,
                                      BranchId = d.BranchId,
                                      Branch = d.MstBranch.Branch,
                                      JVNumber = d.JVNumber,
                                      JVDate = d.JVDate.ToShortDateString(),
                                      Particulars = d.Particulars,
                                      ManualJVNumber = d.ManualJVNumber,
                                      PreparedById = d.PreparedById,
                                      PreparedBy = d.MstUser.FullName,
                                      CheckedById = d.CheckedById,
                                      CheckedBy = d.MstUser1.FullName,
                                      ApprovedById = d.ApprovedById,
                                      ApprovedBy = d.MstUser2.FullName,
                                      IsLocked = d.IsLocked,
                                      CreatedById = d.CreatedById,
                                      CreatedBy = d.MstUser3.FullName,
                                      CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                      UpdatedById = d.UpdatedById,
                                      UpdatedBy = d.MstUser4.FullName,
                                      UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                  };

            return journalVouchers.ToList();
        }

        // add journal voucher
        [Authorize]
        [HttpPost]
        [Route("api/addJournalVoucher")]
        public Int32 insertJournalVoucher(Models.TrnJournalVoucher journalVoucher)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var lastJVNumber = from d in db.TrnJournalVouchers.OrderByDescending(d => d.Id) select d;
                var JVNumberResult = "0000000001";

                if (lastJVNumber.Any())
                {
                    var JVNumber = Convert.ToInt32(lastJVNumber.FirstOrDefault().JVNumber) + 0000000001;
                    JVNumberResult = zeroFill(JVNumber, 10);
                }

                Data.TrnJournalVoucher newJournalVoucher = new Data.TrnJournalVoucher();
                newJournalVoucher.BranchId = currentBranchId();
                newJournalVoucher.JVNumber = JVNumberResult;
                newJournalVoucher.ManualJVNumber = "NA";
                newJournalVoucher.JVDate = DateTime.Today;
                newJournalVoucher.Particulars = "NA";
                newJournalVoucher.PreparedById = userId;
                newJournalVoucher.CheckedById = userId;
                newJournalVoucher.ApprovedById = userId;
                newJournalVoucher.IsLocked = false;
                newJournalVoucher.CreatedById = userId;
                newJournalVoucher.CreatedDateTime = DateTime.Now;
                newJournalVoucher.UpdatedById = userId;
                newJournalVoucher.UpdatedDateTime = DateTime.Now;

                db.TrnJournalVouchers.InsertOnSubmit(newJournalVoucher);
                db.SubmitChanges();

                return newJournalVoucher.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update after locking the journal voucher
        public void updateBalances(Int32 JVId)
        {
            var journalVoucherLines = from d in db.TrnJournalVoucherLines
                                      where d.JVId == Convert.ToInt32(JVId)
                                      select new Models.TrnJournalVoucherLine
                                      {
                                          Id = d.Id,
                                          JVId = d.JVId,
                                          JVNumber = d.TrnJournalVoucher.JVNumber,
                                          JVDate = d.TrnJournalVoucher.JVDate.ToShortDateString(),
                                          JVParticulars = d.TrnJournalVoucher.Particulars,
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

            if (journalVoucherLines.Any())
            {
                foreach (var journalVoucherLine in journalVoucherLines)
                {
                    if (journalVoucherLine.APRRId != null)
                    {
                        var receivingReceipt = from d in db.TrnReceivingReceipts
                                               where d.Id == journalVoucherLine.APRRId
                                               select d;

                        // check receiving receipt
                        if (receivingReceipt.Any())
                        {
                            // get all amounts in jv lines for accounts payable
                            var APRRjournalVoucherLines = from d in db.TrnJournalVoucherLines
                                                          where d.APRRId == journalVoucherLine.APRRId
                                                          && d.TrnJournalVoucher.IsLocked == true
                                                          select d;

                            Decimal APRRdadjustmentAmount = 0;
                            if (APRRjournalVoucherLines.Any())
                            {
                                APRRdadjustmentAmount = APRRjournalVoucherLines.Sum(d => d.CreditAmount) - APRRjournalVoucherLines.Sum(d => d.DebitAmount);
                            }

                            // get disburement amount
                            var disbursementLines = from d in db.TrnDisbursementLines
                                                    where d.RRId == journalVoucherLine.APRRId
                                                    && d.TrnDisbursement.IsLocked == true
                                                    select d;

                            Decimal disburseAmount = 0;
                            if (disbursementLines.Any())
                            {
                                disburseAmount = disbursementLines.Sum(d => d.Amount);
                            }


                            Decimal receivingReceiptAmount = receivingReceipt.FirstOrDefault().Amount;
                            Decimal receivingReceiptWTAXAmount = receivingReceipt.FirstOrDefault().WTaxAmount;
                            Decimal receivingReceipPaidAmount = receivingReceipt.FirstOrDefault().PaidAmount;
                            Decimal adjustmentAmount = APRRdadjustmentAmount;

                            var updateReceivingReceipt = receivingReceipt.FirstOrDefault();
                            updateReceivingReceipt.BalanceAmount = (receivingReceiptAmount - receivingReceiptWTAXAmount - receivingReceipPaidAmount) + adjustmentAmount;
                            updateReceivingReceipt.AdjustmentAmount = adjustmentAmount;
                            db.SubmitChanges();
                        }
                    }
                    else
                    {
                        if (journalVoucherLine.ARSIId != null)
                        {
                            var salesInvoices = from d in db.TrnSalesInvoices
                                                where d.Id == journalVoucherLine.ARSIId
                                                select d;

                            // check sales invoice
                            if (salesInvoices.Any())
                            {
                                // get all amounts in jv lines for accounts receivable
                                var APRRjournalVoucherLines = from d in db.TrnJournalVoucherLines
                                                              where d.ARSIId == journalVoucherLine.ARSIId
                                                              && d.TrnJournalVoucher.IsLocked == true
                                                              select d;

                                Decimal ARSIadjustmentAmount = 0;
                                if (APRRjournalVoucherLines.Any())
                                {
                                    ARSIadjustmentAmount = APRRjournalVoucherLines.Sum(d => d.DebitAmount) - APRRjournalVoucherLines.Sum(d => d.CreditAmount);
                                }

                                // get paid amount
                                var collectionLines = from d in db.TrnCollectionLines
                                                      where d.SIId == journalVoucherLine.ARSIId
                                                      && d.TrnCollection.IsLocked == true
                                                      select d;

                                Decimal paidAmount = 0;
                                if (collectionLines.Any())
                                {
                                    paidAmount = collectionLines.Sum(d => d.Amount);
                                }

                                Decimal salesAmount = salesInvoices.FirstOrDefault().Amount;
                                Decimal adjustmentAmount = ARSIadjustmentAmount;

                                var updateSalesInvoice = salesInvoices.FirstOrDefault();
                                updateSalesInvoice.BalanceAmount = (salesAmount - paidAmount) + adjustmentAmount;
                                updateSalesInvoice.AdjustmentAmount = adjustmentAmount;
                                db.SubmitChanges();
                            }
                        }
                    }
                }
            }
        }

        // update journal voucher
        [Authorize]
        [HttpPut]
        [Route("api/updateJournalVoucher/{id}")]
        public HttpResponseMessage updateJournalVoucher(String id, Models.TrnJournalVoucher journalVoucher)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var journalVouchers = from d in db.TrnJournalVouchers where d.Id == Convert.ToInt32(id) select d;
                if (journalVouchers.Any())
                {
                    var updateJournalVoucher = journalVouchers.FirstOrDefault();
                    updateJournalVoucher.BranchId = journalVoucher.BranchId;
                    updateJournalVoucher.JVNumber = journalVoucher.JVNumber;
                    updateJournalVoucher.ManualJVNumber = journalVoucher.ManualJVNumber;
                    updateJournalVoucher.JVDate = Convert.ToDateTime(journalVoucher.JVDate);
                    updateJournalVoucher.Particulars = journalVoucher.Particulars;
                    updateJournalVoucher.PreparedById = journalVoucher.PreparedById;
                    updateJournalVoucher.CheckedById = journalVoucher.CheckedById;
                    updateJournalVoucher.ApprovedById = journalVoucher.ApprovedById;
                    updateJournalVoucher.IsLocked = true;
                    updateJournalVoucher.UpdatedById = userId;
                    updateJournalVoucher.UpdatedDateTime = DateTime.Now;

                    postJournal.insertJVJournal(Convert.ToInt32(id));
                    db.SubmitChanges();

                    updateBalances(Convert.ToInt32(id));

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

        // unlock journal voucher
        [Authorize]
        [HttpPut]
        [Route("api/updateJournalVoucherIsLock/{id}")]
        public HttpResponseMessage unlockJournalVoucher(String id, Models.TrnJournalVoucher journalVoucher)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var journalVouchers = from d in db.TrnJournalVouchers where d.Id == Convert.ToInt32(id) select d;
                if (journalVouchers.Any())
                {
                    var updateJournalVoucher = journalVouchers.FirstOrDefault();
                    updateJournalVoucher.IsLocked = false;
                    updateJournalVoucher.UpdatedById = userId;
                    updateJournalVoucher.UpdatedDateTime = DateTime.Now;

                    postJournal.deleteJVJournal(Convert.ToInt32(id));
                    db.SubmitChanges();

                    updateBalances(Convert.ToInt32(id));

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

        // delete journal voucher
        [Authorize]
        [HttpDelete]
        [Route("api/deleteJournalVoucher/{id}")]
        public HttpResponseMessage deleteJournalVoucher(String id)
        {
            try
            {
                var journalVouchers = from d in db.TrnJournalVouchers where d.Id == Convert.ToInt32(id) select d;
                if (journalVouchers.Any())
                {
                    db.TrnJournalVouchers.DeleteOnSubmit(journalVouchers.First());
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