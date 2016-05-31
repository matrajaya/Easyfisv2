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

        // ===============
        // LIST Collection
        // ===============
        [Route("api/listCollection")]
        public List<Models.TrnCollection> Get()
        {
            var collections = from d in db.TrnCollections
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

        // ====================
        // GET Collection By Id
        // ====================
        [Route("api/collection/{id}")]
        public Models.TrnCollection GetCollectionById(String id)
        {
            var collection_Id = Convert.ToInt32(id);
            var collections = from d in db.TrnCollections
                              where d.Id == collection_Id
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

        // ===============================
        // GET Collection Filter by OR Date
        // ================================
        [Route("api/listCollectionFilterByORDate/{ORDate}")]
        public List<Models.TrnCollection> GetCollectionFilterByORDate(String ORDate)
        {
            var collection_ORDate = Convert.ToDateTime(ORDate);
            var collections = from d in db.TrnCollections
                              where d.ORDate == collection_ORDate
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

        // ===============================
        // GET last ORNumber in Collection
        // ===============================
        [Route("api/collectionLastORNumber")]
        public Models.TrnCollection GetCollectionLastORNumber()
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

        // =========================
        // GET last Id in Collection
        // =========================
        [Route("api/collectionLastId")]
        public Models.TrnCollection GetCollectionLastId()
        {
            var collections = from d in db.TrnCollections.OrderByDescending(d => d.Id)
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

        // ==============
        // ADD Collection
        // ==============
        [Route("api/addCollection")]
        public int Post(Models.TrnCollection collection)
        {
            try
            {
                var isLocked = false;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

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

                newCollection.IsLocked = isLocked;
                newCollection.CreatedById = mstUserId;
                newCollection.CreatedDateTime = date;
                newCollection.UpdatedById = mstUserId;
                newCollection.UpdatedDateTime = date;

                db.TrnCollections.InsertOnSubmit(newCollection);
                db.SubmitChanges();

                return newCollection.Id;

            }
            catch
            {
                return 0;
            }
        }

        // =================
        // UPDATE Collection
        // =================
        [Route("api/updateCollection/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnCollection collection)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var collection_Id = Convert.ToInt32(id);
                var collections = from d in db.TrnCollections where d.Id == collection_Id select d;

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

                    updateCollection.IsLocked = collection.IsLocked;
                    updateCollection.UpdatedById = mstUserId;
                    updateCollection.UpdatedDateTime = date;

                    db.SubmitChanges();

                    if (updateCollection.IsLocked == true)
                    {
                        journal.insertORJournal(collection_Id);

                        UpdateARCollection(collection_Id);
                    }
                    else
                    {
                        journal.deleteORJournal(collection_Id);
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

        // ============================
        // UPDATE Collection - isLocked
        // ============================
        [Route("api/updateCollectionIsLocked/{id}")]
        public HttpResponseMessage PutIslock(String id, Models.TrnCollection collection)
        {
            try
            {
                //var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var collection_Id = Convert.ToInt32(id);
                var collections = from d in db.TrnCollections where d.Id == collection_Id select d;

                if (collections.Any())
                {
                    var updateCollection = collections.FirstOrDefault();

                    updateCollection.IsLocked = collection.IsLocked;
                    updateCollection.UpdatedById = mstUserId;
                    updateCollection.UpdatedDateTime = date;

                    db.SubmitChanges();

                    if (updateCollection.IsLocked == true)
                    {
                        journal.insertORJournal(collection_Id);

                        UpdateARCollection(collection_Id);
                    }
                    else
                    {
                        journal.deleteORJournal(collection_Id);

                        UpdateARCollection(collection_Id);
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

        // =================
        // DELETE Collection
        // =================
        [Route("api/deleteCollection/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var collection_Id = Convert.ToInt32(id);
                var collections = from d in db.TrnCollections where d.Id == collection_Id select d;

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

        public void UpdateARCollection(Int32 ORId)
        {
            // collection header
            var collectionHeader = from d in db.TrnCollections
                                   where d.Id == ORId
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

            // colelction lines
            var collectionLines = from d in db.TrnCollectionLines
                                  where d.ORId == ORId
                                  group d by new
                                  {
                                      SIId = d.SIId
                                  } into g
                                  select new Models.TrnCollectionLine
                                  {
                                      SIId = g.Key.SIId
                                  };

            try
            {
                if (collectionHeader.Any())
                {
                    if (collectionLines.Any())
                    {
                        foreach (var siIdcollectionLines in collectionLines)
                        {
                            if (siIdcollectionLines.SIId != null)
                            {
                                updateAR(Convert.ToInt32(siIdcollectionLines.SIId));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }

        public void updateAR(Int32 SIId)
        {
            // sales Invoice Header
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

            try
            {
                if (salesInvoices.Any())
                {
                    var salesInvoceUpdate = from d in db.TrnSalesInvoices where d.Id == SIId select d;
                    Debug.WriteLine(SIId);
                    if (salesInvoceUpdate.Any())
                    {
                        Decimal PaidAmount = 0;
                        Decimal AdjustmentAmount = 0;

                        var collectionLines = from d in db.TrnCollectionLines
                                              where d.SIId == SIId
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

                        var journalVoucherLines = from d in db.TrnJournalVoucherLines
                                                  where d.ARSIId == SIId
                                                  select new Models.TrnJournalVoucherLine
                                                  {
                                                      Id = d.Id,
                                                      JVId = d.JVId,
                                                      BranchId = d.BranchId,
                                                      Branch = d.MstBranch.Branch,
                                                      AccountId = d.AccountId,
                                                      Account = d.MstAccount.Account,
                                                      ArticleId = d.ArticleId,
                                                      Article = d.MstArticle.Article,
                                                      Particulars = d.Particulars,
                                                      DebitAmount = d.DebitAmount,
                                                      CreditAmount = d.CreditAmount,
                                                      APRRId = d.APRRId,
                                                      APRR = d.TrnReceivingReceipt.RRNumber,
                                                      APRRBranch = d.TrnReceivingReceipt.MstBranch.Branch,
                                                      ARSIId = d.ARSIId,
                                                      ARSI = d.TrnSalesInvoice.SINumber,
                                                      ARSIBranch = d.TrnSalesInvoice.MstBranch.Branch,
                                                      IsClear = d.IsClear
                                                  };

                        Decimal DebitAmount;
                        Decimal CreditAmount;

                        if (!journalVoucherLines.Any())
                        {
                            DebitAmount = 0;
                            CreditAmount = 0;
                        }
                        else
                        {
                            DebitAmount = journalVoucherLines.Sum(d => d.DebitAmount);
                            CreditAmount = journalVoucherLines.Sum(d => d.CreditAmount);
                        }

                        var collectionLinesORId = from d in db.TrnCollectionLines
                                                  where d.SIId == SIId
                                                  group d by new
                                                  {
                                                      ORId = d.ORId,
                                                  } into g
                                                  select new Models.TrnCollectionLine
                                                  {
                                                      ORId = g.Key.ORId,
                                                  };

                        Int32 ORId = 0;
                        foreach (var salesInvoiceLinesCVId in collectionLinesORId)
                        {
                            ORId = salesInvoiceLinesCVId.ORId;
                        }

                        Boolean collectionHeaderLocked = (from d in db.TrnCollections where d.Id == ORId select d.IsLocked).SingleOrDefault();

                        if (collectionHeaderLocked == true)
                        {
                            PaidAmount = collectionLines.Sum(d => d.Amount);
                            AdjustmentAmount = DebitAmount - CreditAmount;
                            Debug.WriteLine(PaidAmount);
                        }
                        else
                        {
                            PaidAmount = 0;
                            AdjustmentAmount = 0;
                            Debug.WriteLine(PaidAmount);
                        }

                        Debug.WriteLine("Update SI");
                        var updateSI = salesInvoceUpdate.FirstOrDefault();
                        Debug.WriteLine("Update SI 2");
                        updateSI.PaidAmount = PaidAmount;
                        updateSI.AdjustmentAmount = AdjustmentAmount;
                        db.SubmitChanges();
                        Debug.WriteLine("Update SI 3");

                        Decimal SIAmount = 0;
                        Decimal SIPaidAmount = 0;
                        foreach (var siForUpdate in salesInvoices)
                        {
                            SIAmount = siForUpdate.Amount;
                            SIPaidAmount = siForUpdate.PaidAmount;
                        }

                        updateSI.BalanceAmount = (SIAmount - SIPaidAmount) + AdjustmentAmount;
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }
    }
}
