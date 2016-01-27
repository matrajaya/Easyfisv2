using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

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
        public Models.TrnSalesInvoice GetSalesLastSINumber()
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

        // ====================
        // GET last Id in Sales
        // ====================
        [Route("api/salesInvoiceLastId")]
        public Models.TrnSalesInvoice GetSalesLastId()
        {
            var salesInvoices = from d in db.TrnSalesInvoices.OrderByDescending(d => d.Id)
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


        // =========
        // ADD Sales
        // =========
        [Route("api/addSales")]
        public int Post(Models.TrnSalesInvoice sales)
        {
            try
            {
                var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.TrnSalesInvoice newSales = new Data.TrnSalesInvoice();

                newSales.BranchId = sales.BranchId;
                newSales.SINumber = sales.SINumber;
                newSales.SIDate = Convert.ToDateTime(sales.SIDate);
                newSales.CustomerId = sales.CustomerId;
                newSales.TermId = sales.TermId;
                newSales.DocumentReference = sales.DocumentReference;
                newSales.ManualSINumber = sales.ManualSINumber;
                newSales.Remarks = sales.Remarks;
                newSales.Amount = sales.Amount;
                newSales.PaidAmount = sales.PaidAmount;
                newSales.AdjustmentAmount = sales.AdjustmentAmount;
                newSales.BalanceAmount = sales.BalanceAmount;
                newSales.SoldById = sales.SoldById;
                newSales.PreparedById = sales.PreparedById;
                newSales.CheckedById = sales.CheckedById;
                newSales.ApprovedById = sales.ApprovedById;

                newSales.IsLocked = isLocked;
                newSales.CreatedById = mstUserId;
                newSales.CreatedDateTime = date;
                newSales.UpdatedById = mstUserId;
                newSales.UpdatedDateTime = date;

                db.TrnSalesInvoices.InsertOnSubmit(newSales);
                db.SubmitChanges();

                return newSales.Id;
            }
            catch
            {
                return 0;
            }
        }

        // ============
        // UPDATE Sales
        // ============
        [Route("api/updateSales/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnSalesInvoice sales)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var sales_Id = Convert.ToInt32(id);
                var salesInvoces = from d in db.TrnSalesInvoices where d.Id == sales_Id select d;

                if (salesInvoces.Any())
                {
                    var updateSales = salesInvoces.FirstOrDefault();

                    updateSales.BranchId = sales.BranchId;
                    updateSales.SINumber = sales.SINumber;
                    updateSales.SIDate = Convert.ToDateTime(sales.SIDate);
                    updateSales.CustomerId = sales.CustomerId;
                    updateSales.TermId = sales.TermId;
                    updateSales.DocumentReference = sales.DocumentReference;
                    updateSales.ManualSINumber = sales.ManualSINumber;
                    updateSales.Remarks = sales.Remarks;
                    updateSales.Amount = sales.Amount;
                    updateSales.PaidAmount = sales.PaidAmount;
                    updateSales.AdjustmentAmount = sales.AdjustmentAmount;
                    updateSales.BalanceAmount = sales.BalanceAmount;
                    updateSales.SoldById = sales.SoldById;
                    updateSales.PreparedById = sales.PreparedById;
                    updateSales.CheckedById = sales.CheckedById;
                    updateSales.ApprovedById = sales.ApprovedById;

                    updateSales.IsLocked = sales.IsLocked;
                    updateSales.UpdatedById = mstUserId;
                    updateSales.UpdatedDateTime = date;

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

        // =======================
        // UPDATE Sales - IsLocked
        // =======================
        [Route("api/updateSalesIsLocked/{id}")]
        public HttpResponseMessage PutSalesIsLocked(String id, Models.TrnSalesInvoice sales)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var sales_Id = Convert.ToInt32(id);
                var salesInvoces = from d in db.TrnSalesInvoices where d.Id == sales_Id select d;

                if (salesInvoces.Any())
                {
                    var updateSales = salesInvoces.FirstOrDefault();

                    updateSales.IsLocked = sales.IsLocked;
                    updateSales.UpdatedById = mstUserId;
                    updateSales.UpdatedDateTime = date;

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
