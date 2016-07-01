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

        // current branch Id
        public Int32 currentBranchId()
        {
            return (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.BranchId).SingleOrDefault();
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
        [Route("api/listJournalVoucherByJVDate/{JVDate}")]
        public List<Models.TrnJournalVoucher> listJournalVoucherByJVDate(String JVDate)
        {
            var journalVouchers = from d in db.TrnJournalVouchers.OrderByDescending(d => d.Id)
                                  where d.JVDate == Convert.ToDateTime(JVDate)
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

                Data.TrnJournalVoucher newJournalVoucher = new Data.TrnJournalVoucher();
                newJournalVoucher.BranchId = journalVoucher.BranchId;
                newJournalVoucher.JVNumber = journalVoucher.JVNumber;
                newJournalVoucher.ManualJVNumber = journalVoucher.ManualJVNumber;
                newJournalVoucher.JVDate = Convert.ToDateTime(journalVoucher.JVDate);
                newJournalVoucher.Particulars = journalVoucher.Particulars;
                newJournalVoucher.PreparedById = journalVoucher.PreparedById;
                newJournalVoucher.CheckedById = journalVoucher.CheckedById;
                newJournalVoucher.ApprovedById = journalVoucher.ApprovedById;
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