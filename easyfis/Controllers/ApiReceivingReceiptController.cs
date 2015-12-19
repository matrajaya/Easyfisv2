using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiReceivingReceiptController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ======================
        // LIST Receiving Receipt
        // ======================
        [Route("api/listReceivingReceipt")]
        public List<Models.TrnReceivingReceipt> Get()
        {
            var receivingReceipts = from d in db.TrnReceivingReceipts
                                    select new Models.TrnReceivingReceipt
                                    {
                                        Id = d.Id,
                                        BranchId = d.BranchId,
                                        Branch = d.MstBranch.Branch,
                                        RRDate = d.RRDate.ToShortDateString(),
                                        RRNumber = d.RRNumber,
                                        SupplierId = d.SupplierId,
                                        Supplier = d.MstArticle.Article,
                                        TermId = d.TermId,
                                        Term = d.MstTerm.Term,
                                        DocumentReference = d.DocumentReference,
                                        ManualRRNumber = d.ManualRRNumber,
                                        Remarks = d.Remarks,
                                        Amount = d.Amount,
                                        WTaxAmount = d.WTaxAmount,
                                        PaidAmount = d.PaidAmount,
                                        AdjustmentAmount = d.AdjustmentAmount,
                                        BalanceAmount = d.BalanceAmount,
                                        ReceivedById = d.ReceivedById,
                                        ReceivedBy = d.MstUser4.FullName,
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
            return receivingReceipts.ToList();
        }

        // ============================
        // LIST Receiving Receipt By Id
        // ============================
        [Route("api/listReceivingReceiptById/{Id}")]
        public Models.TrnReceivingReceipt GetRRById(String Id)
        {
            var RRId = Convert.ToInt32(Id);
            var receivingReceipts = from d in db.TrnReceivingReceipts
                                    where d.Id == RRId
                                    select new Models.TrnReceivingReceipt
                                    {
                                        Id = d.Id,
                                        BranchId = d.BranchId,
                                        Branch = d.MstBranch.Branch,
                                        RRDate = d.RRDate.ToShortDateString(),
                                        RRNumber = d.RRNumber,
                                        SupplierId = d.SupplierId,
                                        Supplier = d.MstArticle.Article,
                                        TermId = d.TermId,
                                        Term = d.MstTerm.Term,
                                        DocumentReference = d.DocumentReference,
                                        ManualRRNumber = d.ManualRRNumber,
                                        Remarks = d.Remarks,
                                        Amount = d.Amount,
                                        WTaxAmount = d.WTaxAmount,
                                        PaidAmount = d.PaidAmount,
                                        AdjustmentAmount = d.AdjustmentAmount,
                                        BalanceAmount = d.BalanceAmount,
                                        ReceivedById = d.ReceivedById,
                                        ReceivedBy = d.MstUser4.FullName,
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
            return (Models.TrnReceivingReceipt)receivingReceipts.FirstOrDefault();
        }

        // ==============================
        // LIST Receiving Receipt Last Id
        // ==============================
        [Route("api/listReceivingReceiptLast")]
        public Models.TrnReceivingReceipt GetRRLastId()
        {
            var receivingReceipts = from d in db.TrnReceivingReceipts.OrderByDescending(d => d.RRNumber)
                                    select new Models.TrnReceivingReceipt
                                    {
                                        Id = d.Id,
                                        BranchId = d.BranchId,
                                        Branch = d.MstBranch.Branch,
                                        RRDate = d.RRDate.ToShortDateString(),
                                        RRNumber = d.RRNumber,
                                        SupplierId = d.SupplierId,
                                        Supplier = d.MstArticle.Article,
                                        TermId = d.TermId,
                                        Term = d.MstTerm.Term,
                                        DocumentReference = d.DocumentReference,
                                        ManualRRNumber = d.ManualRRNumber,
                                        Remarks = d.Remarks,
                                        Amount = d.Amount,
                                        WTaxAmount = d.WTaxAmount,
                                        PaidAmount = d.PaidAmount,
                                        AdjustmentAmount = d.AdjustmentAmount,
                                        BalanceAmount = d.BalanceAmount,
                                        ReceivedById = d.ReceivedById,
                                        ReceivedBy = d.MstUser4.FullName,
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
            return (Models.TrnReceivingReceipt)receivingReceipts.FirstOrDefault();
        }

        // =========
        // DELETE RR
        // =========
        [Route("api/deleteRR/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var RRId = Convert.ToInt32(id);
                var RRs = from d in db.TrnReceivingReceipts where d.Id == RRId select d;

                if (RRs.Any())
                {
                    db.TrnReceivingReceipts.DeleteOnSubmit(RRs.First());
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
