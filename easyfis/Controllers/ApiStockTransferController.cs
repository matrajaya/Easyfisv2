using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiStockTransferController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===================
        // LIST Stock Transfer
        // ===================
        [Route("api/listStockTransfer")]
        public List<Models.TrnStockTransfer> Get()
        {
            var stockTransfer = from d in db.TrnStockTransfers
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

        // ========================
        // GET Stock Transfer by Id
        // ========================
        [Route("api/stockTransfer/{id}")]
        public Models.TrnStockTransfer GetStockTransferById(String id)
        {
            var stockTransfer_Id = Convert.ToInt32(id);
            var stockTransfer = from d in db.TrnStockTransfers
                                where d.Id == stockTransfer_Id
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

        // ===================================
        // GET Stock Transfer Filter by STDate
        // ===================================
        [Route("api/listStockTransferFilterBySTDate/{STDate}")]
        public List<Models.TrnStockTransfer> GetStockTransferFilterBySTDate(String STDate)
        {
            var stockTransfer_STDate = Convert.ToDateTime(STDate);
            var stockTransfer = from d in db.TrnStockTransfers
                                where d.STDate == stockTransfer_STDate
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


        // ==============================
        // Get STNumber in Stock Transfer
        // ==============================
        [Route("api/stockTransferLastSTNumber")]
        public Models.TrnStockTransfer GetStockTransferLastSTNumber()
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

        // =====================
        // DELETE Stock Transfer
        // =====================
        [Route("api/deleteStockTransfer/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var stockTransfer_Id = Convert.ToInt32(id);
                var stockTransfers = from d in db.TrnStockTransfers where d.Id == stockTransfer_Id select d;

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
