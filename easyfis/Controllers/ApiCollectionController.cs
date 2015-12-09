using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiCollectionController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===============
        // LIST Collection
        // ===============
        [Route("api/listCollection")]
        public List<Models.TrnCollection> Get()
        {
            var collect = from d in db.TrnCollections
                        select new Models.TrnCollection
                        {
                            Id = d.Id,
                            BranchId = d.BranchId,
                            //Branch = d.Branch,
                            ORNumber = d.ORNumber,
                            //ORDate = d.ORDate,
                            //Customer = d.Customer,
                            Particulars = d.Particulars,
                            ManualORNumber = d.ManualORNumber,
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
            return collect.ToList();
        }
    }
}
