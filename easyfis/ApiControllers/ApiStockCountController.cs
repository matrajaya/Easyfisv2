using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiStockCountController : ApiController
    {
        // Global Variable
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();
        private DateTime date = DateTime.Now;

        // GET Current User (Id)
        public Int32 currentUserId()
        {
            var identityUserId = User.Identity.GetUserId();
            var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();

            return mstUserId;
        }

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // LIST Stock Count
        [HttpGet]
        [Route("api/stockCount/list")]
        public List<Models.TrnStockCount> listStockCount()
        {
            var stockCounts = from d in db.TrnStockCounts
                              where d.BranchId == currentUserId()
                              select new Models.TrnStockCount
                              {
                                  Id = d.Id,
                                  BranchId = d.BranchId,
                                  Branch = d.MstBranch.Branch,
                                  SCNumber = d.SCNumber,
                                  SCDate = d.SCDate.ToShortDateString(),
                                  Particulars = d.Particulars,
                                  PreparedBy = d.MstUser3.FullName,
                                  PreparedById = d.PreparedById,
                                  CheckedBy = d.MstUser1.FullName,
                                  CheckedById = d.CheckedById,
                                  ApprovedBy = d.MstUser.FullName,
                                  ApprovedById = d.ApprovedById,
                                  IsLocked = d.IsLocked,
                                  CreatedById = d.CreatedById,
                                  CreatedBy = d.MstUser2.FullName,
                                  CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                  UpdatedById = d.UpdatedById,
                                  UpdatedBy = d.MstUser4.FullName,
                                  UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                              };
            return stockCounts.ToList();
        }

        // LIST Stock Count by SCDate
        [HttpGet]
        [Route("api/stockCount/listBySCDateByBranchId/{SCDate}")]
        public List<Models.TrnStockCount> listStockCountBySCDate(String SCDate)
        {
            var stockCounts = from d in db.TrnStockCounts
                              where d.SCDate == Convert.ToDateTime(SCDate)
                              && d.BranchId == currentBranchId()
                              select new Models.TrnStockCount
                              {
                                  Id = d.Id,
                                  BranchId = d.BranchId,
                                  Branch = d.MstBranch.Branch,
                                  SCNumber = d.SCNumber,
                                  SCDate = d.SCDate.ToShortDateString(),
                                  Particulars = d.Particulars,
                                  PreparedBy = d.MstUser3.FullName,
                                  PreparedById = d.PreparedById,
                                  CheckedBy = d.MstUser1.FullName,
                                  CheckedById = d.CheckedById,
                                  ApprovedBy = d.MstUser.FullName,
                                  ApprovedById = d.ApprovedById,
                                  IsLocked = d.IsLocked,
                                  CreatedById = d.CreatedById,
                                  CreatedBy = d.MstUser2.FullName,
                                  CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                  UpdatedById = d.UpdatedById,
                                  UpdatedBy = d.MstUser4.FullName,
                                  UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                              };
            return stockCounts.ToList();
        }

        // GET Stock Count by Id
        [HttpGet]
        [Route("api/stockCount/getById/{Id}")]
        public Models.TrnStockCount getStockCountById(String Id)
        {
            var stockCounts = from d in db.TrnStockCounts
                              where d.Id == Convert.ToInt32(Id)
                              select new Models.TrnStockCount
                              {
                                  Id = d.Id,
                                  BranchId = d.BranchId,
                                  Branch = d.MstBranch.Branch,
                                  SCNumber = d.SCNumber,
                                  SCDate = d.SCDate.ToShortDateString(),
                                  Particulars = d.Particulars,
                                  PreparedBy = d.MstUser3.FullName,
                                  PreparedById = d.PreparedById,
                                  CheckedBy = d.MstUser1.FullName,
                                  CheckedById = d.CheckedById,
                                  ApprovedBy = d.MstUser.FullName,
                                  ApprovedById = d.ApprovedById,
                                  IsLocked = d.IsLocked,
                                  CreatedById = d.CreatedById,
                                  CreatedBy = d.MstUser2.FullName,
                                  CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                  UpdatedById = d.UpdatedById,
                                  UpdatedBy = d.MstUser4.FullName,
                                  UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                              };
            return (Models.TrnStockCount)stockCounts.FirstOrDefault();
        }

        // GET last Stock Count
        [HttpGet]
        [Route("api/stockCount/getLast")]
        public Models.TrnStockCount getLastStockCount()
        {
            var stockCounts = from d in db.TrnStockCounts.OrderByDescending(d => d.Id)
                              select new Models.TrnStockCount
                              {
                                  Id = d.Id,
                                  BranchId = d.BranchId,
                                  Branch = d.MstBranch.Branch,
                                  SCNumber = d.SCNumber,
                                  SCDate = d.SCDate.ToShortDateString(),
                                  Particulars = d.Particulars,
                                  PreparedBy = d.MstUser3.FullName,
                                  PreparedById = d.PreparedById,
                                  CheckedBy = d.MstUser1.FullName,
                                  CheckedById = d.CheckedById,
                                  ApprovedBy = d.MstUser.FullName,
                                  ApprovedById = d.ApprovedById,
                                  IsLocked = d.IsLocked,
                                  CreatedById = d.CreatedById,
                                  CreatedBy = d.MstUser2.FullName,
                                  CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                  UpdatedById = d.UpdatedById,
                                  UpdatedBy = d.MstUser4.FullName,
                                  UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                              };
            return (Models.TrnStockCount)stockCounts.FirstOrDefault();
        }

        // SAVE Stock Count
        [HttpPost]
        [Route("api/stockCount/save")]
        public int saveStockCount(Models.TrnStockCount stockCount)
        {
            try
            {
                Data.TrnStockCount newStockCount = new Data.TrnStockCount();
                newStockCount.BranchId = stockCount.BranchId;
                newStockCount.SCNumber = stockCount.SCNumber;
                newStockCount.SCDate = Convert.ToDateTime(stockCount.SCDate);
                newStockCount.Particulars = stockCount.Particulars;
                newStockCount.PreparedById = stockCount.PreparedById;
                newStockCount.CheckedById = stockCount.CheckedById;
                newStockCount.ApprovedById = stockCount.ApprovedById;
                newStockCount.IsLocked = false;
                newStockCount.CreatedById = currentUserId();
                newStockCount.CreatedDateTime = date;
                newStockCount.UpdatedById = currentUserId();
                newStockCount.UpdatedDateTime = date;

                db.TrnStockCounts.InsertOnSubmit(newStockCount);
                db.SubmitChanges();

                return newStockCount.Id;
            }
            catch
            {
                return 0;
            }
        }

        // LOCK Stock Count
        [HttpPut]
        [Route("api/stockCount/lock/{id}")]
        public HttpResponseMessage lockStockCount(String id, Models.TrnStockCount stockCount)
        {
            try
            {
                var stockCounts = from d in db.TrnStockCounts where d.Id == Convert.ToInt32(id) select d;
                if (stockCounts.Any())
                {
                    var updateStockCount = stockCounts.FirstOrDefault();

                    updateStockCount.BranchId = stockCount.BranchId;
                    updateStockCount.SCNumber = stockCount.SCNumber;
                    updateStockCount.SCDate = Convert.ToDateTime(stockCount.SCDate);
                    updateStockCount.Particulars = stockCount.Particulars;
                    updateStockCount.PreparedById = stockCount.PreparedById;
                    updateStockCount.CheckedById = stockCount.CheckedById;
                    updateStockCount.ApprovedById = stockCount.ApprovedById;
                    updateStockCount.IsLocked = true;
                    updateStockCount.UpdatedById = currentUserId();
                    updateStockCount.UpdatedDateTime = date;

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

        // UNLOCK Stock Count
        [HttpPut]
        [Route("api/stockCount/unlock/{id}")]
        public HttpResponseMessage unlockStockCount(String id)
        {
            try
            {
                var stockCounts = from d in db.TrnStockCounts where d.Id == Convert.ToInt32(id) select d;
                if (stockCounts.Any())
                {
                    var updateStockCount = stockCounts.FirstOrDefault();

                    updateStockCount.IsLocked = false;
                    updateStockCount.UpdatedById = currentUserId();
                    updateStockCount.UpdatedDateTime = date;

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

        // DELETE Stock Count
        [HttpDelete]
        [Route("api/stockCount/delete/{id}")]
        public HttpResponseMessage deleteStockCount(String id)
        {
            try
            {
                var stockCounts = from d in db.TrnStockCounts where d.Id == Convert.ToInt32(id) select d;
                if (stockCounts.Any())
                {
                    db.TrnStockCounts.DeleteOnSubmit(stockCounts.First());
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
