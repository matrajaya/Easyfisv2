﻿using System;
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

        // ==============================================
        // Get the Max Journal Voucher Number in Database
        // ==============================================
        public String getMaxJournalVoucherNo()
        {
            var maxJVNo = (from d in db.TrnJournalVouchers select d.JVNumber).Max();
            Debug.WriteLine(maxJVNo);

            if (maxJVNo == null)
            {
                maxJVNo = "0";
            }
            return maxJVNo;
        }

        // ====================
        // LIST Journal Voucher
        // ====================
        [Route("api/listJournalVoucher")]
        public List<Models.TrnJournalVoucher> Get()
        {
            var journalVouchers = from d in db.TrnJournalVouchers
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
                                      //PreparedBy = d.MstUser3.UserName,
                                      PreparedBy = d.MstUser.FullName,
                                      CheckedById = d.CheckedById,
                                      //CheckedBy = d.MstUser1.UserName,
                                      CheckedBy = d.MstUser1.FullName,
                                      ApprovedById = d.ApprovedById,
                                      //ApprovedBy = d.MstUser.UserName,
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

        // ==============================
        // GET last Id in Journal Voucher
        // ==============================
        [Route("api/journalVoucherLastId")]
        public Models.TrnJournalVoucher GetLastId()
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
            return (Models.TrnJournalVoucher)journalVouchers.FirstOrDefault();
        }

        // ====================================
        // GET last JVNumber in Journal Voucher
        // ====================================
        [Route("api/journalVoucherLastJVNumber")]
        public Models.TrnJournalVoucher GetLastJVNumber()
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

        // =========================
        // GET Journal Voucher by Id
        // =========================
        [Route("api/journalVoucher/{Id}")]
        public Models.TrnJournalVoucher GetJournalVoucherById(String Id)
        {
            var journalVoucher_Id = Convert.ToInt32(Id);
            var journalVouchers = from d in db.TrnJournalVouchers
                                  where d.Id == journalVoucher_Id
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

        // =================================
        // LIST Journal Voucher by Branch Id
        // =================================
        [Route("api/listJournalVoucherByBranchId/{branchId}")]
        public List<Models.TrnJournalVoucher> GetJournalVoucherByBranchId(String branchId)
        {
            var journalVoucher_BranchId = Convert.ToInt32(branchId);
            var journalVouchers = from d in db.TrnJournalVouchers
                                  where d.BranchId == journalVoucher_BranchId
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

        // ===================
        // ADD Journal Voucher
        // ===================
        [Route("api/addJournalVoucher")]
        public int Post(Models.TrnJournalVoucher journalVoucher)
        {
            try
            {
                Data.TrnJournalVoucher newJournalVoucher = new Data.TrnJournalVoucher();

                var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var getMax = Convert.ToInt32(getMaxJournalVoucherNo());
                var sumOfMaxJVNoPlusOne = getMax + 1;

                var numberZeroSequenceOfJVNoGetMaxNo = String.Format("{0:0000000000}", sumOfMaxJVNoPlusOne);

                newJournalVoucher.BranchId = journalVoucher.BranchId;
                //newJournalVoucher.JVNumber = journalVoucher.JVNumber;
                newJournalVoucher.JVNumber = numberZeroSequenceOfJVNoGetMaxNo;
                newJournalVoucher.ManualJVNumber = journalVoucher.ManualJVNumber;
                newJournalVoucher.JVDate = Convert.ToDateTime(journalVoucher.JVDate);
                newJournalVoucher.Particulars = journalVoucher.Particulars;
                newJournalVoucher.PreparedById = journalVoucher.PreparedById;
                newJournalVoucher.CheckedById = journalVoucher.CheckedById;
                newJournalVoucher.ApprovedById = journalVoucher.ApprovedById;
                newJournalVoucher.IsLocked = isLocked;
                newJournalVoucher.CreatedById = mstUserId;
                newJournalVoucher.CreatedDateTime = date;
                newJournalVoucher.UpdatedById = mstUserId;
                newJournalVoucher.UpdatedDateTime = date;

                db.TrnJournalVouchers.InsertOnSubmit(newJournalVoucher);
                db.SubmitChanges();

                return newJournalVoucher.Id;
            }
            catch
            {
                return 0;
            }
        }

        // ======================
        // UPDATE Journal Voucher
        // ======================
        [Route("api/updateJournalVoucher/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnJournalVoucher journalVoucher)
        {
            try
            {
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var journalVoucherId = Convert.ToInt32(id);
                var journalVouchers = from d in db.TrnJournalVouchers where d.Id == journalVoucherId select d;

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
                    updateJournalVoucher.IsLocked = journalVoucher.IsLocked;
                    updateJournalVoucher.UpdatedById = mstUserId;
                    updateJournalVoucher.UpdatedDateTime = date;

                    if (updateJournalVoucher.IsLocked == true)
                    {
                        postJournal.insertJVJournal(journalVoucherId);
                    }
                    else
                    {
                        postJournal.deleteJVJournal(journalVoucherId);
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

        // =============================
        // UPDATE Journal Voucher IsLock
        // =============================
        [Route("api/updateJournalVoucherIsLock/{id}")]
        public HttpResponseMessage PutIsLock(String id, Models.TrnJournalVoucher journalVoucher)
        {
            try
            {
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var journalVoucherId = Convert.ToInt32(id);
                var journalVouchers = from d in db.TrnJournalVouchers where d.Id == journalVoucherId select d;

                if (journalVouchers.Any())
                {
                    var updateJournalVoucher = journalVouchers.FirstOrDefault();

                    updateJournalVoucher.IsLocked = journalVoucher.IsLocked;
                    updateJournalVoucher.UpdatedById = mstUserId;
                    updateJournalVoucher.UpdatedDateTime = date;

                    if (updateJournalVoucher.IsLocked == true)
                    {
                        postJournal.insertJVJournal(journalVoucherId);
                    }
                    else
                    {
                        postJournal.deleteJVJournal(journalVoucherId);
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

        // ======================
        // DELETE Journal Voucher
        // ======================
        [Route("api/deleteJournalVoucher/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var journalVoucherId = Convert.ToInt32(id);
                var journalVouchers = from d in db.TrnJournalVouchers where d.Id == journalVoucherId select d;

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