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
            var stockCount = from d in db.TrnStockCounts
                        select new Models.TrnStockCount
                        {
                            Id = d.Id,
                            BranchId = d.BranchId,
                            //Branch = d.Branch,
                            SCNumber = d.SCNumber,
                            //SCDate = d.SCDate,
                            Particulars = d.Particulars,
                            //PreparedBy = d.PreparedBy,
                            PreparedById = d.PreparedById,
                            //CheckedBy = d.CheckedBy,
                            CheckedById = d.CheckedById,
                            //ApprovedBy = d.ApprovedBy,
                            ApprovedById = d.ApprovedById,
                            IsLocked = d.IsLocked,
                            CreatedById = d.CreatedById,
                            //CreatedBy = d.CreatedBy,
                            //CreatedDateTime = d.CreatedDateTime,
                            UpdatedById = d.UpdatedById,
                            //UpdatedBy = d.UpdatedBy,
                            //UpdatedDateTime = d.UpdatedDateTime
                        };
            return stockCount.ToList();
        }
    }
}
