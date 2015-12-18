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
                                     Branch = d.MstBranch.Branch,
                                     PONumber = d.PONumber,
                                     PODate = d.PODate.ToShortDateString(),
                                     SupplierId = d.SupplierId,
                                     Supplier = d.MstArticle.Article,
                                     TermId = d.TermId,
                                     Term = d.MstTerm.Term,
                                     ManualRequestNumber = d.ManualRequestNumber,
                                     ManualPONumber = d.ManualPONumber,
                                     DateNeeded = d.DateNeeded.ToShortDateString(),
                                     Remarks = d.Remarks,
                                     IsClose = d.IsClose,
                                     RequestedById = d.RequestedById,
                                     RequestedBy = d.MstUser4.FullName,
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
                                     UpdatedBy = d.MstUser5.FullName,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };
            return purchaseOrders.ToList();
        }

        // =========================
        // LIST Purchase Order by Id
        // =========================
        [Route("api/listPurchaseOrderById/{id}")]
        public Models.TrnPurchaseOrder GetPurchaseOrderById(String id)
        {
            var POId = Convert.ToInt32(id);
            var purchaseOrders = from d in db.TrnPurchaseOrders
                                 where d.Id == POId
                                 select new Models.TrnPurchaseOrder
                                 {
                                     Id = d.Id,
                                     BranchId = d.BranchId,
                                     Branch = d.MstBranch.Branch,
                                     PONumber = d.PONumber,
                                     PODate = d.PODate.ToShortDateString(),
                                     SupplierId = d.SupplierId,
                                     Supplier = d.MstArticle.Article,
                                     TermId = d.TermId,
                                     Term = d.MstTerm.Term,
                                     ManualRequestNumber = d.ManualRequestNumber,
                                     ManualPONumber = d.ManualPONumber,
                                     DateNeeded = d.DateNeeded.ToShortDateString(),
                                     Remarks = d.Remarks,
                                     IsClose = d.IsClose,
                                     RequestedById = d.RequestedById,
                                     RequestedBy = d.MstUser4.FullName,
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
                                     UpdatedBy = d.MstUser5.FullName,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };
            return (Models.TrnPurchaseOrder)purchaseOrders.FirstOrDefault();
        }

        // ========================
        // LIST Purchase Order Last
        // ========================
        [Route("api/listPurchaseOrderLast")]
        public Models.TrnPurchaseOrder GetPOLastId()
        {
            var purchaseOrders = from d in db.TrnPurchaseOrders.OrderByDescending(d => d.PONumber)
                                 select new Models.TrnPurchaseOrder
                                 {
                                     Id = d.Id,
                                     BranchId = d.BranchId,
                                     Branch = d.MstBranch.Branch,
                                     PONumber = d.PONumber,
                                     PODate = d.PODate.ToShortDateString(),
                                     SupplierId = d.SupplierId,
                                     Supplier = d.MstArticle.Article,
                                     TermId = d.TermId,
                                     Term = d.MstTerm.Term,
                                     ManualRequestNumber = d.ManualRequestNumber,
                                     ManualPONumber = d.ManualPONumber,
                                     DateNeeded = d.DateNeeded.ToShortDateString(),
                                     Remarks = d.Remarks,
                                     IsClose = d.IsClose,
                                     RequestedById = d.RequestedById,
                                     RequestedBy = d.MstUser4.FullName,
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
                                     UpdatedBy = d.MstUser5.FullName,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };
            return (Models.TrnPurchaseOrder)purchaseOrders.FirstOrDefault();
        }


        // =========
        // DELETE PO
        // =========
        [Route("api/deletePO/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var POId = Convert.ToInt32(id);
                var POs = from d in db.TrnPurchaseOrders where d.Id == POId select d;

                if (POs.Any())
                {
                    db.TrnPurchaseOrders.DeleteOnSubmit(POs.First());
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
