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
    public class ApiBranchController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==============================================
        // Get the Max Journal Voucher Number in Database
        // ==============================================
        public String getMaxBranchCode()
        {
            var maxJVNo = (from d in db.MstBranches select d.BranchCode).Max();
            Debug.WriteLine(maxJVNo);

            if (maxJVNo == null)
            {
                maxJVNo = "0";
            }

            return maxJVNo;
        }

        public Int32 getUserDefaultCompanyId()
        {
            var identityUserId = User.Identity.GetUserId();
            var users = from d in db.MstUsers where d.UserId == identityUserId select d;

            return users.FirstOrDefault().CompanyId;
        }

        // ===========
        // LIST Branch
        // ===========
        [Route("api/listBranch")]
        public List<Models.MstBranch> Get()
        {
            var branches = from d in db.MstBranches
                           where d.CompanyId == getUserDefaultCompanyId()
                           select new Models.MstBranch
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
                                   CreatedBy = d.MstUser.FullName,
                                   CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                   UpdatedById = d.UpdatedById,
                                   UpdatedBy = d.MstUser1.FullName,
                                   UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                               };
            return branches.ToList();
        }

        // ===========================
        // LIST Branch get Last Record
        // ===========================
        [Route("api/listBranchLast")]
        public Models.MstBranch GetLastBranch()
        {
            var branches = from d in db.MstBranches.OrderByDescending(d => d.BranchCode)
                           select new Models.MstBranch
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
                               CreatedBy = d.MstUser.FullName,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedById = d.UpdatedById,
                               UpdatedBy = d.MstUser1.FullName,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };

            return (Models.MstBranch)branches.FirstOrDefault();
        }

        // =================
        // LIST Branch by Id
        // =================
        [Route("api/listBranchById/{Id}")]
        public Models.MstBranch GetBranchById(String Id)
        {
            var branchId = Convert.ToInt32(Id);
            var branches = from d in db.MstBranches
                           where d.Id == branchId
                           select new Models.MstBranch
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
                               CreatedBy = d.MstUser.FullName,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedById = d.UpdatedById,
                               UpdatedBy = d.MstUser1.FullName,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };
            return (Models.MstBranch)branches.FirstOrDefault();
        }

        // =========================
        // LIST Branch by Company Id
        // =========================
        [Route("api/listBranchByCompanyId/{companyId}")]
        public List<Models.MstBranch> GetBranch(String companyId)
        {
            var branchCompanyId = Convert.ToInt32(companyId);
            var branches = from d in db.MstBranches
                           where d.CompanyId == branchCompanyId
                           select new Models.MstBranch
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
                                   CreatedBy = d.MstUser.FullName,
                                   CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                   UpdatedById = d.UpdatedById,
                                   UpdatedBy = d.MstUser1.FullName,
                                   UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                               };
            return branches.ToList();
        }

        // ==========
        // ADD Branch
        // ==========
        [Route("api/addBranch")]
        public int Post(Models.MstBranch branch)
        {
            try
            {
                var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var getMax = Convert.ToInt32(getMaxBranchCode());
                var sumOfMaxBranchCodePlusOne = getMax + 1;

                var numberZeroSequenceOfBranchCodeGetMaxNo = String.Format("{0:000}", sumOfMaxBranchCodePlusOne);

                Data.MstBranch newBranch = new Data.MstBranch();

                newBranch.CompanyId = branch.CompanyId;
                newBranch.Branch = branch.Branch;
                //newBranch.BranchCode = branch.BranchCode;
                newBranch.BranchCode = numberZeroSequenceOfBranchCodeGetMaxNo;
                newBranch.Address = branch.Address;
                newBranch.ContactNumber = branch.ContactNumber;
                newBranch.TaxNumber = branch.TaxNumber;
                newBranch.IsLocked = isLocked;
                newBranch.CreatedById = mstUserId;
                newBranch.CreatedDateTime = date;
                newBranch.UpdatedById = mstUserId;
                newBranch.UpdatedDateTime = date;

                db.MstBranches.InsertOnSubmit(newBranch);
                db.SubmitChanges();

                return newBranch.Id;

            }
            catch
            {
                return 0;
            }
        }

        // =============
        // UPDATE Branch
        // =============
        [Route("api/updateBranch/{id}")]
        public HttpResponseMessage Put(String id, Models.MstBranch branch)
        {
            try
            {
                var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

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
                    updateBranch.IsLocked = isLocked;
                    updateBranch.UpdatedById = mstUserId;
                    updateBranch.UpdatedDateTime = date;

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

        // =============
        // DELETE Branch
        // =============
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
