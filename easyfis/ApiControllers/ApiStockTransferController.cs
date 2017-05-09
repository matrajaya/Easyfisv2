using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiStockTransferController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        private Business.Inventory inventory = new Business.Inventory();
        private Business.PostJournal journal = new Business.PostJournal();

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

        // list stock transfer 
        [Authorize]
        [HttpGet]
        [Route("api/listStockTransfer")]
        public List<Models.TrnStockTransfer> listStockTransfer()
        {
            var stockTransfer = from d in db.TrnStockTransfers.OrderByDescending(d => d.Id)
                                where d.BranchId == currentBranchId()
                                select new Models.TrnStockTransfer
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    STNumber = d.STNumber,
                                    STDate = d.STDate.ToShortDateString(),
                                    ToBranchId = d.ToBranchId,
                                    ToBranch = d.MstBranch1.Branch,
                                    Particulars = d.Particulars,
                                    ManualSTNumber = d.ManualSTNumber,
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

            return stockTransfer.ToList();
        }

        // get stock transfer by Id
        [Authorize]
        [HttpGet]
        [Route("api/stockTransfer/{id}")]
        public Models.TrnStockTransfer getStockTransferById(String id)
        {
            var stockTransfer = from d in db.TrnStockTransfers
                                where d.Id == Convert.ToInt32(id)
                                select new Models.TrnStockTransfer
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    STNumber = d.STNumber,
                                    STDate = d.STDate.ToShortDateString(),
                                    ToBranchId = d.ToBranchId,
                                    ToBranch = d.MstBranch1.Branch,
                                    Particulars = d.Particulars,
                                    ManualSTNumber = d.ManualSTNumber,
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

            return (Models.TrnStockTransfer)stockTransfer.FirstOrDefault();
        }

        // list stock transfer by STDate
        [Authorize]
        [HttpGet]
        [Route("api/listStockTransferFilterBySTDate/{STStartDate}/{STEndDate}")]
        public List<Models.TrnStockTransfer> listStockTransferBySTDate(String STStartDate, String STEndDate)
        {
            var stockTransfer = from d in db.TrnStockTransfers.OrderByDescending(d => d.Id)
                                where d.STDate >= Convert.ToDateTime(STStartDate)
                                && d.STDate <= Convert.ToDateTime(STEndDate)
                                && d.BranchId == currentBranchId()
                                select new Models.TrnStockTransfer
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    STNumber = d.STNumber,
                                    STDate = d.STDate.ToShortDateString(),
                                    ToBranchId = d.ToBranchId,
                                    ToBranch = d.MstBranch1.Branch,
                                    Particulars = d.Particulars,
                                    ManualSTNumber = d.ManualSTNumber,
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

            return stockTransfer.ToList();
        }

        // get stock transfer last STNumber
        [Authorize]
        [HttpGet]
        [Route("api/stockTransferLastSTNumber")]
        public Models.TrnStockTransfer getStockTransferLastSTNumber()
        {
            var stockTransfer = from d in db.TrnStockTransfers.OrderByDescending(d => d.STNumber)
                                select new Models.TrnStockTransfer
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    STNumber = d.STNumber,
                                    STDate = d.STDate.ToShortDateString(),
                                    ToBranchId = d.ToBranchId,
                                    ToBranch = d.MstBranch1.Branch,
                                    Particulars = d.Particulars,
                                    ManualSTNumber = d.ManualSTNumber,
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

            return (Models.TrnStockTransfer)stockTransfer.FirstOrDefault();
        }

        // add stock transfer
        [Authorize]
        [HttpPost]
        [Route("api/addStockTransfer")]
        public Int32 insertStockTransfer(Models.TrnStockTransfer stockTransfer)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var lastSTNumber = from d in db.TrnStockTransfers.OrderByDescending(d => d.Id) select d;
                var STNumberResult = "0000000001";

                if (lastSTNumber.Any())
                {
                    var STNumber = Convert.ToInt32(lastSTNumber.FirstOrDefault().STNumber) + 0000000001;
                    STNumberResult = zeroFill(STNumber, 10);
                }

                var users = from d in db.MstUsers where d.Id == userId select d;
                var branches = from d in db.MstBranches where d.Id != currentBranchId() && d.CompanyId == users.FirstOrDefault().CompanyId select d;

                Data.TrnStockTransfer newStockTransfer = new Data.TrnStockTransfer();
                newStockTransfer.BranchId = currentBranchId();
                newStockTransfer.STNumber = STNumberResult;
                newStockTransfer.STDate = DateTime.Today;
                newStockTransfer.ToBranchId = branches.FirstOrDefault().Id;
                newStockTransfer.Particulars = "NA";
                newStockTransfer.ManualSTNumber = "NA";
                newStockTransfer.PreparedById = userId;
                newStockTransfer.CheckedById = userId;
                newStockTransfer.ApprovedById = userId;
                newStockTransfer.IsLocked = false;
                newStockTransfer.CreatedById = userId;
                newStockTransfer.CreatedDateTime = DateTime.Now;
                newStockTransfer.UpdatedById = userId;
                newStockTransfer.UpdatedDateTime = DateTime.Now;

                db.TrnStockTransfers.InsertOnSubmit(newStockTransfer);
                db.SubmitChanges();

                return newStockTransfer.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update stock transfer
        [Authorize]
        [HttpPut]
        [Route("api/updateStockTransfer/{id}")]
        public HttpResponseMessage updateStockTransfer(String id, Models.TrnStockTransfer stockTransfer)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var stockTransfers = from d in db.TrnStockTransfers where d.Id == Convert.ToInt32(id) select d;
                if (stockTransfers.Any())
                {
                    var updateStockTransfer = stockTransfers.FirstOrDefault();

                    updateStockTransfer.BranchId = stockTransfer.BranchId;
                    updateStockTransfer.STNumber = stockTransfer.STNumber;
                    updateStockTransfer.STDate = Convert.ToDateTime(stockTransfer.STDate);
                    updateStockTransfer.ToBranchId = stockTransfer.ToBranchId;
                    updateStockTransfer.Particulars = stockTransfer.Particulars;
                    updateStockTransfer.ManualSTNumber = stockTransfer.ManualSTNumber;
                    updateStockTransfer.PreparedById = stockTransfer.PreparedById;
                    updateStockTransfer.CheckedById = stockTransfer.CheckedById;
                    updateStockTransfer.ApprovedById = stockTransfer.ApprovedById;
                    updateStockTransfer.IsLocked = true;
                    updateStockTransfer.UpdatedById = userId;
                    updateStockTransfer.UpdatedDateTime = DateTime.Now;

                    db.SubmitChanges();

                    inventory.InsertSTInventory(Convert.ToInt32(id));
                    journal.insertSTJournal(Convert.ToInt32(id));

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

        // unlock stock transfer
        [Authorize]
        [HttpPut]
        [Route("api/updateStockTransferIsLocked/{id}")]
        public HttpResponseMessage unlockStockTransfer(String id, Models.TrnStockTransfer stockTransfer)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var stockTransfers = from d in db.TrnStockTransfers where d.Id == Convert.ToInt32(id) select d;
                if (stockTransfers.Any())
                {
                    var updateStockTransfer = stockTransfers.FirstOrDefault();

                    updateStockTransfer.IsLocked = false;
                    updateStockTransfer.UpdatedById = userId;
                    updateStockTransfer.UpdatedDateTime = DateTime.Now;

                    db.SubmitChanges();

                    inventory.deleteSTInventory(Convert.ToInt32(id));
                    journal.deleteSTJournal(Convert.ToInt32(id));

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

        // delete stock transfer
        [Authorize]
        [HttpDelete]
        [Route("api/deleteStockTransfer/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var stockTransfers = from d in db.TrnStockTransfers where d.Id == Convert.ToInt32(id) select d;
                if (stockTransfers.Any())
                {
                    db.TrnStockTransfers.DeleteOnSubmit(stockTransfers.First());
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
