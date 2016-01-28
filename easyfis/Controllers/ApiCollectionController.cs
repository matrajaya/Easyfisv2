using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiCollectionController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===============
        // LIST Collection
        // ===============
        [Route("api/listCollection")]
        public List<Models.TrnCollection> Get()
        {
            var collections = from d in db.TrnCollections
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
    }
}
