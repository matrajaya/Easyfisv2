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
            var rr = from d in db.TrnReceivingReceipts
                        select new Models.TrnReceivingReceipt
                        {
                            Id = d.Id,
                            BranchId = d.BranchId,
                            //Branch = d.Branch,
                            //RRDate = d.RRDate,
                            RRNumber = d.RRNumber,
                            SupplierId = d.SupplierId,
                            //Supplier = d.Supplier,
                            TermId = d.TermId,
                            //Term = d.Term,
                            DocumentReference = d.DocumentReference,
                            ManualRRNumber = d.ManualRRNumber,
                            Remarks = d.Remarks,
                            //Amount = d.Amount,
                            //WTaxAmount = d.WTaxAmount,
                            //PaidAmount = d.PaidAmount,
                            //AdjustmentAmount = d.AdjustmentAmount,
                            //BalanceAmount = d.BalanceAmount,
                            ReceivedById = d.ReceivedById,
                            //ReceivedBy = d.ReceivedBy,
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
            return rr.ToList();
        }
    }
}
