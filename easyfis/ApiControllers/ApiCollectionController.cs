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
    public class ApiCollectionController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();
        private Business.PostJournal journal = new Business.PostJournal();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // update AR
        public void updateAR(Int32 SIId)
        {
            var salesInvoices = from d in db.TrnSalesInvoices
                                where d.Id == SIId
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

            if (salesInvoices.Any())
            {
                var salesInvoceUpdate = from d in db.TrnSalesInvoices where d.Id == SIId select d;
                if (salesInvoceUpdate.Any())
                {
                    Decimal DebitAmount = 0;
                    Decimal CreditAmount = 0;
                    var journalVoucherLines = from d in db.TrnJournalVoucherLines where d.ARSIId == SIId select d;
                    if (journalVoucherLines.Any())
                    {
                        DebitAmount = journalVoucherLines.Sum(d => d.DebitAmount);
                        CreditAmount = journalVoucherLines.Sum(d => d.CreditAmount);
                    }

                    Decimal PaidAmount = 0;
                    Decimal AdjustmentAmount = 0;
                    var collectionLinesORId = from d in db.TrnCollectionLines where d.SIId == SIId select d;
                    if (collectionLinesORId.Any())
                    {
                        Boolean collectionHeaderLocked = (from d in db.TrnCollections where d.Id == collectionLinesORId.First().ORId select d.IsLocked).SingleOrDefault();

                        var collectionLines = from d in db.TrnCollectionLines where d.SIId == SIId select d;
                        if (collectionHeaderLocked == true)
                        {
                            PaidAmount = collectionLines.Sum(d => d.Amount);
                            AdjustmentAmount = DebitAmount - CreditAmount;
                        }
                    }

                    var updateSalesInvoice = salesInvoceUpdate.FirstOrDefault();
                    updateSalesInvoice.PaidAmount = PaidAmount;
                    updateSalesInvoice.AdjustmentAmount = AdjustmentAmount;
                    db.SubmitChanges();

                    Decimal SIAmount = 0;
                    Decimal SIPaidAmount = 0;
                    foreach (var salesInvoice in salesInvoices)
                    {
                        SIAmount = salesInvoice.Amount;
                        SIPaidAmount = salesInvoice.PaidAmount;
                    }

                    updateSalesInvoice.BalanceAmount = (SIAmount - SIPaidAmount) + AdjustmentAmount;
                    db.SubmitChanges();
                }
            }
        }

        // update AR Collection
        public void UpdateARCollection(Int32 ORId)
        {
            var collectionLines = from d in db.TrnCollectionLines where d.ORId == ORId select d;
            if (collectionLines.Any())
            {
                if (collectionLines.First().SIId != null)
                {
                    updateAR(Convert.ToInt32(collectionLines.First().SIId));
                }
            }
        }

        // get collection amount from collection lines
        public Decimal getAmount(Int32 ORId)
        {
            Decimal amount = 0;

            var collectionLines = from d in db.TrnCollectionLines where d.ORId == ORId select d;
            if (collectionLines.Any())
            {
                amount = collectionLines.Sum(d => d.Amount);
            }

            return amount;
        }

        // list collection
        [Authorize]
        [HttpGet]
        [Route("api/listCollection")]
        public List<Models.TrnCollection> listCollection()
        {
            var collections = from d in db.TrnCollections.OrderByDescending(d => d.Id)
                              where d.BranchId == currentBranchId()
                              select new Models.TrnCollection
                              {
                                  Id = d.Id,
                                  BranchId = d.BranchId,
                                  Branch = d.MstBranch.Branch,
                                  ORNumber = d.ORNumber,
                                  ORDate = d.ORDate.ToShortDateString(),
                                  CustomerId = d.CustomerId,
                                  Customer = d.MstArticle.Article,
                                  Particulars = d.Particulars,
                                  ManualORNumber = d.ManualORNumber,
                                  Amount = getAmount(d.Id),
                                  PreparedById = d.PreparedById,
                                  PreparedBy = d.MstUser3.FullName,
                                  CheckedById = d.CheckedById,
                                  CheckedBy = d.MstUser.FullName,
                                  ApprovedById = d.ApprovedById,
                                  ApprovedBy = d.MstUser1.FullName,
                                  IsLocked = d.IsLocked,
                                  CreatedById = d.CreatedById,
                                  CreatedBy = d.MstUser2.FullName,
                                  CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                  UpdatedById = d.UpdatedById,
                                  UpdatedBy = d.MstUser4.FullName,
                                  UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                              };

            return collections.ToList();
        }

        // get collection by Id
        [Authorize]
        [HttpGet]
        [Route("api/collection/{id}")]
        public Models.TrnCollection getCollectionById(String id)
        {
            var collections = from d in db.TrnCollections
                              where d.Id == Convert.ToInt32(id)
                              select new Models.TrnCollection
                              {
                                  Id = d.Id,
                                  BranchId = d.BranchId,
                                  Branch = d.MstBranch.Branch,
                                  ORNumber = d.ORNumber,
                                  ORDate = d.ORDate.ToShortDateString(),
                                  CustomerId = d.CustomerId,
                                  Customer = d.MstArticle.Article,
                                  Particulars = d.Particulars,
                                  ManualORNumber = d.ManualORNumber,
                                  Amount = getAmount(d.Id),
                                  PreparedById = d.PreparedById,
                                  PreparedBy = d.MstUser3.FullName,
                                  CheckedById = d.CheckedById,
                                  CheckedBy = d.MstUser.FullName,
                                  ApprovedById = d.ApprovedById,
                                  ApprovedBy = d.MstUser1.FullName,
                                  IsLocked = d.IsLocked,
                                  CreatedById = d.CreatedById,
                                  CreatedBy = d.MstUser2.FullName,
                                  CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                  UpdatedById = d.UpdatedById,
                                  UpdatedBy = d.MstUser4.FullName,
                                  UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                              };

            return (Models.TrnCollection)collections.FirstOrDefault();
        }

        // list collection by ORDate
        [Authorize]
        [HttpGet]
        [Route("api/listCollectionFilterByORDate/{ORDate}")]
        public List<Models.TrnCollection> listCollectionByORDate(String ORDate)
        {
            var collections = from d in db.TrnCollections.OrderByDescending(d => d.Id)
                              where d.ORDate == Convert.ToDateTime(ORDate)
                              && d.BranchId == currentBranchId()
                              select new Models.TrnCollection
                              {
                                  Id = d.Id,
                                  BranchId = d.BranchId,
                                  Branch = d.MstBranch.Branch,
                                  ORNumber = d.ORNumber,
                                  ORDate = d.ORDate.ToShortDateString(),
                                  CustomerId = d.CustomerId,
                                  Customer = d.MstArticle.Article,
                                  Particulars = d.Particulars,
                                  ManualORNumber = d.ManualORNumber,
                                  Amount = getAmount(d.Id),
                                  PreparedById = d.PreparedById,
                                  PreparedBy = d.MstUser3.FullName,
                                  CheckedById = d.CheckedById,
                                  CheckedBy = d.MstUser.FullName,
                                  ApprovedById = d.ApprovedById,
                                  ApprovedBy = d.MstUser1.FullName,
                                  IsLocked = d.IsLocked,
                                  CreatedById = d.CreatedById,
                                  CreatedBy = d.MstUser2.FullName,
                                  CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                  UpdatedById = d.UpdatedById,
                                  UpdatedBy = d.MstUser4.FullName,
                                  UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                              };

            return collections.ToList();
        }

        // get collection last ORNumber
        [Authorize]
        [HttpGet]
        [Route("api/collectionLastORNumber")]
        public Models.TrnCollection getCollectionLastORNumber()
        {
            var collections = from d in db.TrnCollections.OrderByDescending(d => d.ORNumber)
                              select new Models.TrnCollection
                              {
                                  Id = d.Id,
                                  BranchId = d.BranchId,
                                  Branch = d.MstBranch.Branch,
                                  ORNumber = d.ORNumber,
                                  ORDate = d.ORDate.ToShortDateString(),
                                  CustomerId = d.CustomerId,
                                  Customer = d.MstArticle.Article,
                                  Particulars = d.Particulars,
                                  Amount = getAmount(d.Id),
                                  ManualORNumber = d.ManualORNumber,
                                  PreparedById = d.PreparedById,
                                  PreparedBy = d.MstUser3.FullName,
                                  CheckedById = d.CheckedById,
                                  CheckedBy = d.MstUser.FullName,
                                  ApprovedById = d.ApprovedById,
                                  ApprovedBy = d.MstUser1.FullName,
                                  IsLocked = d.IsLocked,
                                  CreatedById = d.CreatedById,
                                  CreatedBy = d.MstUser2.FullName,
                                  CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                  UpdatedById = d.UpdatedById,
                                  UpdatedBy = d.MstUser4.FullName,
                                  UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                              };

            return (Models.TrnCollection)collections.FirstOrDefault();
        }

        // add collection
        [Authorize]
        [HttpPost]
        [Route("api/addCollection")]
        public Int32 insertCollection(Models.TrnCollection collection)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.TrnCollection newCollection = new Data.TrnCollection();
                newCollection.BranchId = collection.BranchId;
                newCollection.ORNumber = collection.ORNumber;
                newCollection.ORDate = Convert.ToDateTime(collection.ORDate);
                newCollection.CustomerId = collection.CustomerId;
                newCollection.Particulars = collection.Particulars;
                newCollection.ManualORNumber = collection.ManualORNumber;
                newCollection.PreparedById = collection.PreparedById;
                newCollection.CheckedById = collection.CheckedById;
                newCollection.ApprovedById = collection.ApprovedById;
                newCollection.IsLocked = false;
                newCollection.CreatedById = userId;
                newCollection.CreatedDateTime = DateTime.Now;
                newCollection.UpdatedById = userId;
                newCollection.UpdatedDateTime = DateTime.Now;

                db.TrnCollections.InsertOnSubmit(newCollection);
                db.SubmitChanges();

                return newCollection.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update collection
        [Authorize]
        [HttpPut]
        [Route("api/updateCollection/{id}")]
        public HttpResponseMessage updateCollection(String id, Models.TrnCollection collection)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var collections = from d in db.TrnCollections where d.Id == Convert.ToInt32(id) select d;
                if (collections.Any())
                {
                    var updateCollection = collections.FirstOrDefault();
                    updateCollection.BranchId = collection.BranchId;
                    updateCollection.ORNumber = collection.ORNumber;
                    updateCollection.ORDate = Convert.ToDateTime(collection.ORDate);
                    updateCollection.CustomerId = collection.CustomerId;
                    updateCollection.Particulars = collection.Particulars;
                    updateCollection.ManualORNumber = collection.ManualORNumber;
                    updateCollection.PreparedById = collection.PreparedById;
                    updateCollection.CheckedById = collection.CheckedById;
                    updateCollection.ApprovedById = collection.ApprovedById;
                    updateCollection.IsLocked = true;
                    updateCollection.UpdatedById = userId;
                    updateCollection.UpdatedDateTime = DateTime.Now;

                    db.SubmitChanges();

                    journal.insertORJournal(Convert.ToInt32(id));
                    UpdateARCollection(Convert.ToInt32(id));

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

        // unlock collection
        [Authorize]
        [HttpPut]
        [Route("api/updateCollectionIsLocked/{id}")]
        public HttpResponseMessage unlockCollection(String id, Models.TrnCollection collection)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var collections = from d in db.TrnCollections where d.Id == Convert.ToInt32(id) select d;
                if (collections.Any())
                {
                    var updateCollection = collections.FirstOrDefault();
                    updateCollection.IsLocked = false;
                    updateCollection.UpdatedById = userId;
                    updateCollection.UpdatedDateTime = DateTime.Now;

                    db.SubmitChanges();

                    journal.deleteORJournal(Convert.ToInt32(id));
                    UpdateARCollection(Convert.ToInt32(id));

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

        // delete collection
        [Authorize]
        [HttpDelete]
        [Route("api/deleteCollection/{id}")]
        public HttpResponseMessage deleteCollection(String id)
        {
            try
            {
                var collections = from d in db.TrnCollections where d.Id == Convert.ToInt32(id) select d;
                if (collections.Any())
                {
                    db.TrnCollections.DeleteOnSubmit(collections.First());
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
