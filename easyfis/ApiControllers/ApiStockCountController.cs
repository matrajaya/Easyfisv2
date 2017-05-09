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
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

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


        // list stock count
        [Authorize]
        [HttpGet]
        [Route("api/stockCount/list")]
        public List<Models.TrnStockCount> listStockCount()
        {
            var stockCounts = from d in db.TrnStockCounts.OrderByDescending(d => d.Id)
                              where d.BranchId == currentBranchId()
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

        // list stock count by SCDate
        [Authorize]
        [HttpGet]
        [Route("api/stockCount/listBySCDateByBranchId/{SCStartDate}/{SCEndDate}")]
        public List<Models.TrnStockCount> listStockCountBySCDate(String SCStartDate, String SCEndDate)
        {
            var stockCounts = from d in db.TrnStockCounts.OrderByDescending(d => d.Id)
                              where d.SCDate >= Convert.ToDateTime(SCStartDate)
                              && d.SCDate <= Convert.ToDateTime(SCEndDate)
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

        // get stock count by Id
        [Authorize]
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

        // get last Stock Count
        [Authorize]
        [HttpGet]
        [Route("api/stockCount/getLast")]
        public Models.TrnStockCount getStockCountLast()
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

        // add stock count
        [Authorize]
        [HttpPost]
        [Route("api/stockCount/save")]
        public Int32 insertStockCount(Models.TrnStockCount stockCount)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var lastSCNumber = from d in db.TrnStockCounts.OrderByDescending(d => d.Id) select d;
                var SCNumberResult = "0000000001";

                if (lastSCNumber.Any())
                {
                    var SCNumber = Convert.ToInt32(lastSCNumber.FirstOrDefault().SCNumber) + 0000000001;
                    SCNumberResult = zeroFill(SCNumber, 10);
                }

                Data.TrnStockCount newStockCount = new Data.TrnStockCount();
                newStockCount.BranchId = currentBranchId();
                newStockCount.SCNumber = SCNumberResult;
                newStockCount.SCDate = DateTime.Today;
                newStockCount.Particulars = "NA";
                newStockCount.PreparedById = userId;
                newStockCount.CheckedById = userId;
                newStockCount.ApprovedById = userId;
                newStockCount.IsLocked = false;
                newStockCount.CreatedById = userId;
                newStockCount.CreatedDateTime = DateTime.Now;
                newStockCount.UpdatedById = userId;
                newStockCount.UpdatedDateTime = DateTime.Now;

                db.TrnStockCounts.InsertOnSubmit(newStockCount);
                db.SubmitChanges();

                return newStockCount.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update stock count
        [Authorize]
        [HttpPut]
        [Route("api/stockCount/lock/{id}")]
        public HttpResponseMessage updateStockCount(String id, Models.TrnStockCount stockCount)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

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
                    updateStockCount.UpdatedById = userId;
                    updateStockCount.UpdatedDateTime = DateTime.Now;

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

        // unlock stock count
        [Authorize]
        [HttpPut]
        [Route("api/stockCount/unlock/{id}")]
        public HttpResponseMessage unlockStockCount(String id)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var stockCounts = from d in db.TrnStockCounts where d.Id == Convert.ToInt32(id) select d;
                if (stockCounts.Any())
                {
                    var updateStockCount = stockCounts.FirstOrDefault();

                    updateStockCount.IsLocked = false;
                    updateStockCount.UpdatedById = userId;
                    updateStockCount.UpdatedDateTime = DateTime.Now;

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

        // delete stock count
        [Authorize]
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
