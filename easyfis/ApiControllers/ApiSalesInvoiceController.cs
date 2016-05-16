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
        private Business.Inventory inventory = new Business.Inventory();
        private Business.PostJournal journal = new Business.PostJournal();

        // ===================
        // Get Amount in Sales
        // ===================
        public Decimal getAmount(Int32 SIId)
        {
            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                    where d.SIId == SIId
                                    select new Models.TrnSalesInvoiceItem
                                    {
                                        Id = d.Id,
                                        SIId = d.SIId,
                                        SI = d.TrnSalesInvoice.SINumber,
                                        ItemId = d.ItemId,
                                        ItemCode = d.MstArticle.ManualArticleCode,
                                        Item = d.MstArticle.Article,
                                        ItemInventoryId = d.ItemInventoryId,
                                        ItemInventory = d.MstArticleInventory.InventoryCode,
                                        Particulars = d.Particulars,
                                        UnitId = d.UnitId,
                                        Unit = d.MstUnit.Unit,
                                        Quantity = d.Quantity,
                                        Price = d.Price,
                                        DiscountId = d.DiscountId,
                                        Discount = d.MstDiscount.Discount,
                                        DiscountRate = d.DiscountRate,
                                        DiscountAmount = d.DiscountAmount,
                                        NetPrice = d.NetPrice,
                                        Amount = d.Amount,
                                        VATId = d.VATId,
                                        VAT = d.MstTaxType.TaxType,
                                        VATPercentage = d.VATPercentage,
                                        VATAmount = d.VATAmount,
                                        BaseUnitId = d.BaseUnitId,
                                        BaseUnit = d.MstUnit1.Unit,
                                        BaseQuantity = d.BaseQuantity,
                                        BasePrice = d.BasePrice
                                    };

            Decimal amount;
            if (!salesInvoiceItems.Any())
            {
                amount = 0;
            }
            else
            {
                amount = salesInvoiceItems.Sum(d => d.Amount);
            }

            return amount;
        }

        // ==================
        // LIST Sales Invoice
        // ==================
        [Route("api/listSalesInvoice")]
        public List<Models.TrnSalesInvoice> Get()
        {
            var branchIdCookie = Request.Headers.GetCookies("branchId").SingleOrDefault();
            var salesInvoices = from d in db.TrnSalesInvoices
                                where d.BranchId == Convert.ToInt32(branchIdCookie["branchId"].Value)
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

        // ================================
        // GET Sales Invoice By Customer Id
        // ================================
        [Route("api/salesInvoiceByCustomerId/{customerId}")]
        public List<Models.TrnSalesInvoice> GetSalesByCustomerId(String customerId)
        {
            var sales_customerId = Convert.ToInt32(customerId);
            var salesInvoices = from d in db.TrnSalesInvoices
                                where d.CustomerId == sales_customerId
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

        // ===========================================
        // GET Sales Invoice By Customer Id by Balance
        // ===========================================
        [Route("api/salesInvoiceByCustomerIdByBalance/{customerId}")]
        public List<Models.TrnSalesInvoice> GetSalesByCustomerIdByBalance(String customerId)
        {
            var sales_customerId = Convert.ToInt32(customerId);
            var salesInvoices = from d in db.TrnSalesInvoices
                                where d.CustomerId == sales_customerId
                                && d.BalanceAmount > 0
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

        // ===================================
        // GET Sales Invoice Filter by SI Date
        // ===================================
        [Route("api/listSalesInvoiceFilterBySIDate/{SIDate}")]
        public List<Models.TrnSalesInvoice> GetSalesFilterBySIDate(String SIDate)
        {
            var branchIdCookie = Request.Headers.GetCookies("branchId").SingleOrDefault();
            var sales_SIDate = Convert.ToDateTime(SIDate);
            var salesInvoices = from d in db.TrnSalesInvoices
                                where d.SIDate == sales_SIDate
                                && d.BranchId == Convert.ToInt32(branchIdCookie["branchId"].Value)
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
                newSales.Amount = 0;
                newSales.PaidAmount = 0;
                newSales.AdjustmentAmount = 0;
                newSales.BalanceAmount = 0;
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
                    var collectionLines = from d in db.TrnCollectionLines
                                          where d.SIId == sales_Id
                                          select new Models.TrnCollectionLine
                                          {
                                              Id = d.Id,
                                              ORId = d.ORId,
                                              OR = d.TrnCollection.ORNumber,
                                              BranchId = d.BranchId,
                                              Branch = d.MstBranch.Branch,
                                              AccountId = d.AccountId,
                                              Account = d.MstAccount.Account,
                                              ArticleId = d.ArticleId,
                                              Article = d.MstArticle.Article,
                                              SIId = d.SIId,
                                              SI = d.TrnSalesInvoice.SINumber,
                                              Particulars = d.Particulars,
                                              Amount = d.Amount,
                                              PayTypeId = d.PayTypeId,
                                              PayType = d.MstPayType.PayType,
                                              CheckNumber = d.CheckNumber,
                                              CheckDate = d.CheckDate.ToShortDateString(),
                                              CheckBank = d.CheckBank,
                                              DepositoryBankId = d.DepositoryBankId,
                                              DepositoryBank = d.MstArticle1.Article,
                                              IsClear = d.IsClear,
                                          };

                    var collectionLinesORIds = from d in db.TrnCollectionLines
                                               where d.SIId == sales_Id
                                               group d by new
                                               {
                                                   ORId = d.ORId
                                               } into g
                                               select new Models.TrnCollectionLine
                                               {
                                                   ORId = g.Key.ORId,
                                               };

                    Int32 ORId = 0;
                    foreach (var collectionLinesORId in collectionLinesORIds)
                    {
                        ORId = collectionLinesORId.ORId;
                    }

                    Boolean collectionHeaderIsLocked = (from d in db.TrnCollections where d.Id == ORId select d.IsLocked).SingleOrDefault();

                    Decimal PaidAmount = 0;
                    if (collectionLines.Any())
                    {
                        if (collectionHeaderIsLocked == true)
                        {
                            PaidAmount = collectionLines.Sum(d => d.Amount);
                        }
                        else
                        {
                            PaidAmount = 0;
                        }
                    }
                    else
                    {
                        PaidAmount = 0;
                    }

                    var updateSales = salesInvoces.FirstOrDefault();

                    updateSales.BranchId = sales.BranchId;
                    updateSales.SINumber = sales.SINumber;
                    updateSales.SIDate = Convert.ToDateTime(sales.SIDate);
                    updateSales.CustomerId = sales.CustomerId;
                    updateSales.TermId = sales.TermId;
                    updateSales.DocumentReference = sales.DocumentReference;
                    updateSales.ManualSINumber = sales.ManualSINumber;
                    updateSales.Remarks = sales.Remarks;
                    updateSales.Amount = getAmount(sales_Id);
                    updateSales.PaidAmount = PaidAmount;
                    updateSales.AdjustmentAmount = 0;
                    updateSales.BalanceAmount = getAmount(sales_Id) - PaidAmount;
                    updateSales.SoldById = sales.SoldById;
                    updateSales.PreparedById = sales.PreparedById;
                    updateSales.CheckedById = sales.CheckedById;
                    updateSales.ApprovedById = sales.ApprovedById;

                    updateSales.IsLocked = sales.IsLocked;
                    updateSales.UpdatedById = mstUserId;
                    updateSales.UpdatedDateTime = date;

                    db.SubmitChanges();

                    if (updateSales.IsLocked == true)
                    {
                        inventory.InsertSIInventory(sales_Id);
                        journal.insertSIJournal(sales_Id);
                    }
                    else
                    {
                        inventory.deleteSIInventory(sales_Id);
                        journal.deleteSIJournal(sales_Id);
                    }

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

                    if (updateSales.IsLocked == true)
                    {
                        inventory.InsertSIInventory(sales_Id);
                        journal.insertSIJournal(sales_Id);
                    }
                    else
                    {
                        inventory.deleteSIInventory(sales_Id);
                        journal.deleteSIJournal(sales_Id);
                    }

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
