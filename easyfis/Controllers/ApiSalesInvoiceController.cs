using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiSalesInvoiceController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==================
        // LIST Sales Invoice
        // ==================
        [Route("api/listSalesInvoice")]
        public List<Models.TrnSalesInvoice> Get()
        {
            var salesInvoices = from d in db.TrnSalesInvoices
                                select new Models.TrnSalesInvoice
                                {
                                    Id = d.Id,
                                    //RRId = d.RRId,
                                    //SINNumber = d.SINNumber,
                                    //SIDate = d.SIDate,
                                    CustomerId = d.CustomerId,
                                    TermId = d.TermId,
                                    //Term = d.Term,
                                    DocumentReference = d.DocumentReference,
                                    ManualSINumber = d.ManualSINumber,
                                    Remarks = d.Remarks,
                                    //Amount = d.Amount,
                                    //PaidAmount = d.PaidAmount,
                                    //AdjustmentAmount = d.AdjustmentAmount,
                                    //BalanceAmount = d.BalanceAmount,
                                    SoldById = d.SoldById,
                                    //SoldBy = d.SoldBy,
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
                                    //UpdatedDateTime = d.UpdatedDateTime,
                                };
            return salesInvoices.ToList();
        }
    }
}
