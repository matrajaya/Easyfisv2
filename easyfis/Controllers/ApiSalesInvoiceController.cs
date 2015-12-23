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
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    SINumber = d.SINumber,
                                    SIDate = d.SIDate.ToShortDateString(),
                                    CustomerId = d.CustomerId,
                                    Customer = d.MstArticle.Article,
                                    TermId = d.TermId,
                                    Term = d.MstTerm.Term,
                                    DocumentReference = d.DocumentReference,
                                    ManualSINumber = d.ManualSINumber,
                                    Remarks = d.Remarks,
                                    Amount = d.Amount,
                                    PaidAmount = d.PaidAmount,
                                    AdjustmentAmount = d.AdjustmentAmount,
                                    BalanceAmount = d.BalanceAmount,
                                    SoldById = d.SoldById,
                                    SoldBy = d.MstUser4.FullName,
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
            return salesInvoices.ToList();
        }

        // =======================
        // GET Sales Invoice By Id
        // =======================
        [Route("api/salesInvoice/{id}")]
        public Models.TrnSalesInvoice GetSalesById(String id)
        {
            var sales_Id = Convert.ToInt32(id);
            var salesInvoices = from d in db.TrnSalesInvoices
                                where d.Id == sales_Id
                                select new Models.TrnSalesInvoice
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    SINumber = d.SINumber,
                                    SIDate = d.SIDate.ToShortDateString(),
                                    CustomerId = d.CustomerId,
                                    Customer = d.MstArticle.Article,
                                    TermId = d.TermId,
                                    Term = d.MstTerm.Term,
                                    DocumentReference = d.DocumentReference,
                                    ManualSINumber = d.ManualSINumber,
                                    Remarks = d.Remarks,
                                    Amount = d.Amount,
                                    PaidAmount = d.PaidAmount,
                                    AdjustmentAmount = d.AdjustmentAmount,
                                    BalanceAmount = d.BalanceAmount,
                                    SoldById = d.SoldById,
                                    SoldBy = d.MstUser4.FullName,
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
            return (Models.TrnSalesInvoice)salesInvoices.FirstOrDefault();
        }
        
        // ===================================
        // GET Sales Invoice Filter by SI Date
        // ===================================
        [Route("api/listSalesInvoiceFilterBySIDate/{SIDate}")]
        public List<Models.TrnSalesInvoice> GetSalesFilterBySIDate(String SIDate)
        {
            var sales_SIDate = Convert.ToDateTime(SIDate);
            var salesInvoices = from d in db.TrnSalesInvoices
                                where d.SIDate == sales_SIDate
                                select new Models.TrnSalesInvoice
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    SINumber = d.SINumber,
                                    SIDate = d.SIDate.ToShortDateString(),
                                    CustomerId = d.CustomerId,
                                    Customer = d.MstArticle.Article,
                                    TermId = d.TermId,
                                    Term = d.MstTerm.Term,
                                    DocumentReference = d.DocumentReference,
                                    ManualSINumber = d.ManualSINumber,
                                    Remarks = d.Remarks,
                                    Amount = d.Amount,
                                    PaidAmount = d.PaidAmount,
                                    AdjustmentAmount = d.AdjustmentAmount,
                                    BalanceAmount = d.BalanceAmount,
                                    SoldById = d.SoldById,
                                    SoldBy = d.MstUser4.FullName,
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
            return salesInvoices.ToList();
        }

        // =================================
        // GET last SINumber in SalesInvoice
        // =================================
        [Route("api/salesInvoiceLastSINumber")]
        public Models.TrnSalesInvoice GetSalesLastId()
        {
            var salesInvoices = from d in db.TrnSalesInvoices.OrderByDescending(d => d.SINumber)
                                select new Models.TrnSalesInvoice
                                {
                                    Id = d.Id,
                                    BranchId = d.BranchId,
                                    Branch = d.MstBranch.Branch,
                                    SINumber = d.SINumber,
                                    SIDate = d.SIDate.ToShortDateString(),
                                    CustomerId = d.CustomerId,
                                    Customer = d.MstArticle.Article,
                                    TermId = d.TermId,
                                    Term = d.MstTerm.Term,
                                    DocumentReference = d.DocumentReference,
                                    ManualSINumber = d.ManualSINumber,
                                    Remarks = d.Remarks,
                                    Amount = d.Amount,
                                    PaidAmount = d.PaidAmount,
                                    AdjustmentAmount = d.AdjustmentAmount,
                                    BalanceAmount = d.BalanceAmount,
                                    SoldById = d.SoldById,
                                    SoldBy = d.MstUser4.FullName,
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
            return (Models.TrnSalesInvoice)salesInvoices.FirstOrDefault();
        }

        // ============
        // DELETE Sales
        // ============
        [Route("api/deleteSales/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var sales_Id = Convert.ToInt32(id);
                var sales = from d in db.TrnSalesInvoices where d.Id == sales_Id select d;

                if (sales.Any())
                {
                    db.TrnSalesInvoices.DeleteOnSubmit(sales.First());
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
