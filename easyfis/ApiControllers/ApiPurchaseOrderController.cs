using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

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
            var branchIdCookie = Request.Headers.GetCookies("branchId").SingleOrDefault();
            var purchaseOrders = from d in db.TrnPurchaseOrders
                                 where d.BranchId == Convert.ToInt32(branchIdCookie["branchId"].Value)
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

        // ===================
        // Get Amount By PO Id
        // ===================
        public Decimal getAmount(Int32 POId)
        {
            var PurchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                     where d.POId == POId
                                     select new Models.TrnPurchaseOrderItem
                                     {
                                         Id = d.Id,
                                         POId = d.POId,
                                         PO = d.TrnPurchaseOrder.PONumber,
                                         ItemId = d.ItemId,
                                         Item = d.MstArticle.Article,
                                         ItemCode = d.MstArticle.ManualArticleCode,
                                         Particulars = d.Particulars,
                                         UnitId = d.UnitId,
                                         Unit = d.MstUnit.Unit,
                                         Quantity = d.Quantity,
                                         Cost = d.Cost,
                                         Amount = d.Amount
                                     };
            Decimal amount;

            if (!PurchaseOrderItems.Any())
            {
                amount = 0;
            }
            else 
            {
                amount = PurchaseOrderItems.Sum(d => d.Amount);
            }

            return amount;
        }

        // ========================
        // GET Purchase Order by Id
        // ========================
        [Route("api/purchaseOrder/{id}")]
        public Models.TrnPurchaseOrder GetPurchaseOrderById(String id)
        {
            var purchaseOrder_Id = Convert.ToInt32(id);
            var purchaseOrders = from d in db.TrnPurchaseOrders
                                 where d.Id == purchaseOrder_Id
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
                                     //Amount = getAmount(purchaseOrder_Id),
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

        // =================================
        // List Purchase Order filter by Date
        // =================================
        [Route("api/listPurchaseOrderFilterByPODate/{PODate}")]
        public List<Models.TrnPurchaseOrder> GetPurchaseOrderFilterByPODate(String PODate)
        {
            var branchIdCookie = Request.Headers.GetCookies("branchId").SingleOrDefault();
            var purchaseOrder_PODate = Convert.ToDateTime(PODate);
            var purchaseOrders = from d in db.TrnPurchaseOrders
                                 where d.PODate == purchaseOrder_PODate
                                 && d.BranchId == Convert.ToInt32(branchIdCookie["branchId"].Value)
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
                                     Amount = getAmount(d.Id),
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

        // ==================================
        // List Purchase Order By Supplier Id
        // ==================================
        [Route("api/listPurchaseOrderBySupplierId/{SupplierId}")]
        public List<Models.TrnPurchaseOrder> GetPurchaseOrderBySupplierId(String SupplierId)
        {
            var purchaseOrder_SupplierId = Convert.ToInt32(SupplierId);
            var purchaseOrders = from d in db.TrnPurchaseOrders
                                 where d.SupplierId == purchaseOrder_SupplierId
                                 && d.IsLocked == true
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
                                     //Amount = getAmount(d.Id),
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

        // ===================================
        // GET last PONumber in Purchase Order 
        // ===================================
        [Route("api/purchaseOrderLastPONumber")]
        public Models.TrnPurchaseOrder GetLastPONumber()
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
                                     //Amount = getAmount(d.Id),
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

        // =============================
        // GET last Id in Purchase Order 
        // =============================
        [Route("api/purchaseOrderLastId")]
        public Models.TrnPurchaseOrder GetLastId()
        {
            var purchaseOrders = from d in db.TrnPurchaseOrders.OrderByDescending(d => d.Id)
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
                                     //Amount = getAmount(d.Id),
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

        // ==================
        // ADD Puschase Order
        // ==================
        [Route("api/addPurchaseOrder")]
        public int Post(Models.TrnPurchaseOrder purchaseOrder)
        {
            try
            {
                var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.TrnPurchaseOrder newPO = new Data.TrnPurchaseOrder();

                newPO.BranchId = purchaseOrder.BranchId;
                newPO.PONumber = purchaseOrder.PONumber;
                newPO.PODate = Convert.ToDateTime(purchaseOrder.PODate);
                newPO.SupplierId = purchaseOrder.SupplierId;
                newPO.TermId = purchaseOrder.TermId;
                newPO.ManualRequestNumber = purchaseOrder.ManualRequestNumber;
                newPO.ManualPONumber = purchaseOrder.ManualPONumber;
                newPO.DateNeeded = Convert.ToDateTime(purchaseOrder.DateNeeded);
                newPO.Remarks = purchaseOrder.Remarks;
                newPO.IsClose = purchaseOrder.IsClose;
                newPO.RequestedById = purchaseOrder.RequestedById;
                newPO.PreparedById = purchaseOrder.PreparedById;
                newPO.CheckedById = purchaseOrder.CheckedById;
                newPO.ApprovedById = purchaseOrder.ApprovedById;

                newPO.IsLocked = isLocked;
                newPO.CreatedById = mstUserId;
                newPO.CreatedDateTime = date;
                newPO.UpdatedById = mstUserId;
                newPO.UpdatedDateTime = date;

                db.TrnPurchaseOrders.InsertOnSubmit(newPO);
                db.SubmitChanges();

                return newPO.Id;

            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        // =====================
        // Update Puschase Order
        // =====================
        [Route("api/updatePurchaseOrder/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnPurchaseOrder purchaseOrder)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var PO_Id = Convert.ToInt32(id);
                var POs = from d in db.TrnPurchaseOrders where d.Id == PO_Id select d;

                if (POs.Any())
                {
                    var updatePO = POs.FirstOrDefault();

                    updatePO.BranchId = purchaseOrder.BranchId;
                    updatePO.PONumber = purchaseOrder.PONumber;
                    updatePO.PODate = Convert.ToDateTime(purchaseOrder.PODate);
                    updatePO.SupplierId = purchaseOrder.SupplierId;
                    updatePO.TermId = purchaseOrder.TermId;
                    updatePO.ManualRequestNumber = purchaseOrder.ManualRequestNumber;
                    updatePO.ManualPONumber = purchaseOrder.ManualPONumber;
                    updatePO.DateNeeded = Convert.ToDateTime(purchaseOrder.DateNeeded);
                    updatePO.Remarks = purchaseOrder.Remarks;
                    updatePO.IsClose = purchaseOrder.IsClose;
                    updatePO.RequestedById = purchaseOrder.RequestedById;
                    updatePO.PreparedById = purchaseOrder.PreparedById;
                    updatePO.CheckedById = purchaseOrder.CheckedById;
                    updatePO.ApprovedById = purchaseOrder.ApprovedById;

                    updatePO.IsLocked = purchaseOrder.IsLocked;
                    updatePO.UpdatedById = mstUserId;
                    updatePO.UpdatedDateTime = date;

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

        // ================================
        // Update Puschase Order - IsLocked
        // ================================
        [Route("api/updatePurchaseOrderIsLocked/{id}")]
        public HttpResponseMessage PutIsLocked(String id, Models.TrnPurchaseOrder purchaseOrder)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var PO_Id = Convert.ToInt32(id);
                var POs = from d in db.TrnPurchaseOrders where d.Id == PO_Id select d;

                if (POs.Any())
                {
                    var updatePO = POs.FirstOrDefault();

                    updatePO.IsLocked = purchaseOrder.IsLocked;
                    updatePO.UpdatedById = mstUserId;
                    updatePO.UpdatedDateTime = date;

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


        // =========
        // DELETE PO
        // =========
        [Route("api/deletePO/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var purchaseOrder_Id = Convert.ToInt32(id);
                var purchaseOrders = from d in db.TrnPurchaseOrders where d.Id == purchaseOrder_Id select d;

                if (purchaseOrders.Any())
                {
                    db.TrnPurchaseOrders.DeleteOnSubmit(purchaseOrders.First());
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
