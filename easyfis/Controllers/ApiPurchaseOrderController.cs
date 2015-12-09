using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiPurchaseOrderController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===================
        // LIST Purchase Order
        // ===================
        [Route("api/listPurchaseOrder")]
        public List<Models.TrnPurchaseOrder> Get()
        {
            var purchaseOrders = from d in db.TrnPurchaseOrders
                                 select new Models.TrnPurchaseOrder
                                 {
                                     Id = d.Id,
                                     BranchId = d.BranchId,
                                     //Branch = d.Branch,
                                     PONumber = d.PONumber,
                                     //PODate = d.PODate,
                                     SupplierId = d.SupplierId,
                                     //Supplier = d.Supplier,
                                     TermId = d.TermId,
                                     //Term = d.Term,
                                     ManualRequestNumber = d.ManualRequestNumber,
                                     ManualPONumber = d.ManualPONumber,
                                     //DateNeeded = d.DateNeeded,
                                     Remarks = d.Remarks,
                                     IsClose = d.IsClose,
                                     RequestedById = d.RequestedById,
                                     //RequestedBy = d.RequestedBy,
                                     //PreparedBy = d.PreparedBy,
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
            return purchaseOrders.ToList();
        }
    }
}
