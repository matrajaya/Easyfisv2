using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiBranchController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        [Route("api/listBranch")]
        public List<Models.MstBranch> Get()
        {
            var branches = from d in db.MstBranches select new Models.MstBranch
                {
                    Id = d.Id,
                    CompanyId = d.CompanyId,
                    Company = d.MstCompany.Company,
                    BranchCode = d.BranchCode,
                    Branch = d.Branch,
                    Address = d.Address,
                    ContactNumber = d.ContactNumber,
                    TaxNumber = d.TaxNumber,
                    IsLocked = d.IsLocked,
                    CreatedById = d.CreatedById,
                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                    UpdatedById = d.UpdatedById,
                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                };
            return branches.ToList();
        }

        [Route("api/listBranch/{companyId}")]
        public List<Models.MstBranch> GetBranch(String companyId)
        {
            var branchCompanyId = Convert.ToInt32(companyId);
            var branches = from d in db.MstBranches where d.CompanyId == branchCompanyId select new Models.MstBranch
                {
                    Id = d.Id,
                    CompanyId = d.CompanyId,
                    Company = d.MstCompany.Company,
                    BranchCode = d.BranchCode,
                    Branch = d.Branch,
                    Address = d.Address,
                    ContactNumber = d.ContactNumber,
                    TaxNumber = d.TaxNumber,
                    IsLocked = d.IsLocked,
                    CreatedById = d.CreatedById,
                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                    UpdatedById = d.UpdatedById,
                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                };
            return branches.ToList();
        }

        [Route("api/updateBranch/{id}")]
        public HttpResponseMessage Put(String id, Models.MstBranch branch)
        {
            try
            {
                var branchId = Convert.ToInt32(id);
                var branches = from d in db.MstBranches where d.Id == branchId select d;

                if (branches.Any())
                {
                    var updateBranch = branches.FirstOrDefault();

                    updateBranch.CompanyId = branch.CompanyId;
                    updateBranch.BranchCode = branch.BranchCode;
                    updateBranch.Branch = branch.Branch;
                    updateBranch.Address = branch.Address;
                    updateBranch.ContactNumber = branch.ContactNumber;
                    updateBranch.TaxNumber = branch.TaxNumber;

                    //updateBranch.IsLocked = branch.IsLocked;
                    //updateBranch.CreatedById = company.CreatedById;
                    //updateBranch.CreatedDateTime = Convert.ToDateTime(company.CreateDateTime);
                    //updateBranch.UpdatedById = company.UpdatedById;
                    //updateBranch.UpdatedDateTime = Convert.ToDateTime(company.UpdatedDateTime);

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

        [Route("api/deleteBranch/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var branchId = Convert.ToInt32(id);
                var branches = from d in db.MstBranches where d.Id == branchId select d;

                if (branches.Any())
                {
                    db.MstBranches.DeleteOnSubmit(branches.First());
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
