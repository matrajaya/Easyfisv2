﻿using System;
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

        // current company Id
        public Int32 getUserDefaultCompanyId()
        {
            var identityUserId = User.Identity.GetUserId();
            var users = from d in db.MstUsers where d.UserId == identityUserId select d;

            return users.FirstOrDefault().CompanyId;
        }

        // list branch
        [Authorize, HttpGet, Route("api/branch/list")]
        public List<Models.MstBranch> getListBranch()
        {
            var branches = from d in db.MstBranches
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

        // list branch
        [Authorize]
        [HttpGet]
        [Route("api/listBranch")]
        public List<Models.MstBranch> listBranch()
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

        // get branch last
        [Authorize]
        [HttpGet]
        [Route("api/listBranchLast")]
        public Models.MstBranch getBranchLastRecord()
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

        // get branch by Id
        [Authorize]
        [HttpGet]
        [Route("api/listBranchById/{Id}")]
        public Models.MstBranch GetBranchById(String Id)
        {
            var branches = from d in db.MstBranches
                           where d.Id == Convert.ToInt32(Id)
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

        // list branch by CompanyId
        [Authorize]
        [HttpGet]
        [Route("api/listBranchByCompanyId/{companyId}")]
        public List<Models.MstBranch> listBranchByCompanyId(String companyId)
        {
            var branches = from d in db.MstBranches
                           where d.CompanyId == Convert.ToInt32(companyId)
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

        // list branch by CompanyId
        [Authorize]
        [HttpGet]
        [Route("api/branch/byCompanyId/list/locked/{companyId}")]
        public List<Models.MstBranch> listBranchByCompanyIdLocked(String companyId)
        {
            var branches = from d in db.MstBranches
                           where d.CompanyId == Convert.ToInt32(companyId)
                           && d.MstCompany.IsLocked == true
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


        public String zeroFill(Int32 number, Int32 length)
        {
            var result = number.ToString();
            var pad = length - result.Length;
            while (pad > 0)
            {
                result = '0' + result;
                pad--;
            }

            return result;
        }

        // add branch
        [Authorize]
        [HttpPost]
        [Route("api/addBranch")]
        public Int32 insertBranch(Models.MstBranch branch)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var codeDefault = "101";
                var lastBranch = from d in db.MstBranches.OrderByDescending(d => d.Id) select d;
                if (lastBranch.Any())
                {
                    codeDefault = lastBranch.FirstOrDefault().BranchCode;
                    var lastBranchByCompanyId = from d in db.MstBranches.OrderByDescending(d => d.Id) where d.CompanyId == branch.CompanyId select d;
                    if (lastBranchByCompanyId.Any())
                    {
                        codeDefault = lastBranchByCompanyId.FirstOrDefault().BranchCode;
                        var CVNumber = Convert.ToInt32(lastBranchByCompanyId.FirstOrDefault().BranchCode) + 001;
                        codeDefault = zeroFill(CVNumber, 3);
                    }
                    else
                    {
                        var branchCodeIncrement = Convert.ToInt32(lastBranch.FirstOrDefault().BranchCode) + 100;
                        var branchCode = Math.Round(Convert.ToDouble(branchCodeIncrement) / 100, 0) * 100;
                        var CVNumber = branchCode + 001;
                        codeDefault = zeroFill(Convert.ToInt32(CVNumber), 3);
                    }
                }

                Data.MstBranch newBranch = new Data.MstBranch();
                newBranch.CompanyId = branch.CompanyId;
                newBranch.Branch = branch.Branch;
                newBranch.BranchCode = codeDefault;
                newBranch.Address = branch.Address;
                newBranch.ContactNumber = branch.ContactNumber;
                newBranch.TaxNumber = branch.TaxNumber;
                newBranch.IsLocked = true;
                newBranch.CreatedById = userId;
                newBranch.CreatedDateTime = DateTime.Now;
                newBranch.UpdatedById = userId;
                newBranch.UpdatedDateTime = DateTime.Now;

                db.MstBranches.InsertOnSubmit(newBranch);
                db.SubmitChanges();

                return newBranch.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update branch
        [Authorize]
        [HttpPut]
        [Route("api/updateBranch/{id}")]
        public HttpResponseMessage updateBranch(String id, Models.MstBranch branch)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var branches = from d in db.MstBranches where d.Id == Convert.ToInt32(id) select d;
                if (branches.Any())
                {
                    var updateBranch = branches.FirstOrDefault();
                    updateBranch.CompanyId = branch.CompanyId;
                    updateBranch.Branch = branch.Branch;
                    updateBranch.Address = branch.Address;
                    updateBranch.ContactNumber = branch.ContactNumber;
                    updateBranch.TaxNumber = branch.TaxNumber;
                    updateBranch.IsLocked = true;
                    updateBranch.UpdatedById = userId;
                    updateBranch.UpdatedDateTime = DateTime.Now;

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

        // delete branch
        [Authorize]
        [HttpDelete]
        [Route("api/deleteBranch/{id}")]
        public HttpResponseMessage deleteBranch(String id)
        {
            try
            {
                var branches = from d in db.MstBranches where d.Id == Convert.ToInt32(id) select d;
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
