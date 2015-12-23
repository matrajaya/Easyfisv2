using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
