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
            var journalVouchers = from d in db.TrnJournalVouchers select new Models.TrnJournalVoucher
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
    }
}