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
                                      PreparedBy = d.MstUser3.UserName,
                                      CheckedById = d.CheckedById,
                                      CheckedBy = d.MstUser1.UserName,
                                      ApprovedById = d.ApprovedById,
                                      ApprovedBy = d.MstUser.UserName,
                                      IsLocked = d.IsLocked,
                                      CreatedById = d.CreatedById,
                                      CreatedBy = d.MstUser2.UserName,
                                      CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                      UpdatedById = d.UpdatedById,
                                      UpdatedBy = d.MstUser4.UserName,
                                      UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                  };
            return journalVouchers.ToList();
        }

        // ==========================
        // LIST Journal Voucher by Id
        // ==========================
        [Route("api/journalVoucher/{Id}")]
        public Models.TrnJournalVoucher GetJournalVoucherById(String Id)
        {
            var journalVoucherId = Convert.ToInt32(Id);
            var journalVouchers = from d in db.TrnJournalVouchers
                                  where d.Id == journalVoucherId
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
                                          PreparedBy = d.MstUser3.UserName,
                                          CheckedById = d.CheckedById,
                                          CheckedBy = d.MstUser1.UserName,
                                          ApprovedById = d.ApprovedById,
                                          ApprovedBy = d.MstUser.UserName,
                                          IsLocked = d.IsLocked,
                                          CreatedById = d.CreatedById,
                                          CreatedBy = d.MstUser2.UserName,
                                          CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                          UpdatedById = d.UpdatedById,
                                          UpdatedBy = d.MstUser4.UserName,
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
            var journalVoucherBranchId = Convert.ToInt32(branchId);
            var journalVouchers = from d in db.TrnJournalVouchers
                                  where d.BranchId == journalVoucherBranchId
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
                                          PreparedBy = d.MstUser3.UserName,
                                          CheckedById = d.CheckedById,
                                          CheckedBy = d.MstUser1.UserName,
                                          ApprovedById = d.ApprovedById,
                                          ApprovedBy = d.MstUser.UserName,
                                          IsLocked = d.IsLocked,
                                          CreatedById = d.CreatedById,
                                          CreatedBy = d.MstUser2.UserName,
                                          CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                          UpdatedById = d.UpdatedById,
                                          UpdatedBy = d.MstUser4.UserName,
                                          UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                      };
            return journalVouchers.ToList();
        }

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

        // ===================
        // ADD Journal Voucher
        // ===================
        [Route("api/addJournalVoucher")]
        public int Post(Models.TrnJournalVoucher journalVoucher)
        {
            try
            {
                var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.TrnJournalVoucher newJournalVoucher = new Data.TrnJournalVoucher();

                newJournalVoucher.BranchId = journalVoucher.BranchId;
                newJournalVoucher.JVNumber = journalVoucher.JVNumber;
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

                this.getMaxJournalVoucherNo();
                var getMax = Convert.ToInt32(getMaxJournalVoucherNo());
                var sum = getMax + 1;

                Debug.WriteLine(sum);

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
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var journalVoucherId = Convert.ToInt32(id);
                var journalVouchers = from d in db.TrnJournalVouchers where d.Id == journalVoucherId select d;

                Business.PostJournal postJournal = new Business.PostJournal();

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

                    db.SubmitChanges();

                    // postJournal.postJournalVoucher(journalVoucherId);
                    this.getMaxJournalVoucherNo();
                    var getMax = Convert.ToInt32(getMaxJournalVoucherNo());
                    var sum = getMax + 1;

                    Debug.WriteLine(sum);

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