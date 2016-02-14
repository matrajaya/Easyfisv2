using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiStockCountController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ================
        // LIST Stock Count
        // ================
        [Route("api/listStockCount")]
        public List<Models.TrnStockCount> Get()
        {
            var stockCounts = from d in db.TrnStockCounts
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

        // =============================
        // LIST Stock Count by Branch Id
        // =============================
        [Route("api/listStockCountByBranchId/{branchId}")]
        public List<Models.TrnStockCount> GetStockCountByBranchId(String branchId)
        {
            var stockCount_branchId = Convert.ToInt32(branchId);
            var stockCounts = from d in db.TrnStockCounts
                              where d.BranchId == stockCount_branchId
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
    }
}
