using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
        [Route("api/journalVoucherById/{Id}")]
        public List<Models.TrnJournalVoucher> GetJournalVoucherById(String Id)
        {
            var journalVoucherId = Convert.ToInt32(Id);
            var journalVouchers = from d in db.TrnJournalVouchers where d.Id == journalVoucherId select new Models.TrnJournalVoucher
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

        // =================================
        // LIST Journal Voucher by Branch Id
        // =================================
        [Route("api/listJournalVoucherByBranchId/{branchId}")]
        public List<Models.TrnJournalVoucher> GetJournalVoucherByBranchId(String branchId)
        {
            var journalVoucherBranchId = Convert.ToInt32(branchId);
            var journalVouchers = from d in db.TrnJournalVouchers where d.BranchId == journalVoucherBranchId select new Models.TrnJournalVoucher
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

        // ==============
        // DELETE Company
        // ==============
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