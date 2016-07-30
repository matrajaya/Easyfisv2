using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class ApiCompanyController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        public Int32 getUserDefaultCompanyId()
        {
            return (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d).FirstOrDefault().CompanyId;
        }

        // list company
        [Authorize]
        [HttpGet]
        [Route("api/company/list")]
        public List<Models.MstCompany> listCompany()
        {
            var companies = from d in db.MstCompanies
                            select new Models.MstCompany
                            {
                                Id = d.Id,
                                Company = d.Company,
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

            return companies.ToList();
        }

        // list company by default CompanyId
        [Authorize]
        [HttpGet]
        [Route("api/listCompany")]
        public List<Models.MstCompany> listCompanyByDefaultComapanyId()
        {
            var companies = from d in db.MstCompanies
                            where d.Id == getUserDefaultCompanyId()
                            select new Models.MstCompany
                                {
                                    Id = d.Id,
                                    Company = d.Company,
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

            return companies.ToList();
        }

        // get company
        [Authorize]
        [HttpGet]
        [Route("api/company/{id}")]
        public Models.MstCompany getCompany(String id)
        {
            var company = from d in db.MstCompanies
                          where d.Id == Convert.ToInt32(id)
                          select new Models.MstCompany
                              {
                                  Id = d.Id,
                                  Company = d.Company,
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

            return (Models.MstCompany)company.FirstOrDefault();
        }

        // add company
        [Authorize]
        [HttpPost]
        [Route("api/addCompany")]
        public Int32 insertCompany(Models.MstCompany company)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                Data.MstCompany newCompany = new Data.MstCompany();
                newCompany.Company = "NA";
                newCompany.Address = "NA";
                newCompany.ContactNumber = "NA";
                newCompany.TaxNumber = "NA";
                newCompany.IsLocked = false;
                newCompany.CreatedById = userId;
                newCompany.CreatedDateTime = DateTime.Now;
                newCompany.UpdatedById = userId;
                newCompany.UpdatedDateTime = DateTime.Now;

                db.MstCompanies.InsertOnSubmit(newCompany);
                db.SubmitChanges();

                return newCompany.Id;
            }
            catch
            {
                return 0;
            }
        }

        // update company
        [Authorize]
        [HttpPut]
        [Route("api/updateCompany/{id}")]
        public HttpResponseMessage updateCompany(String id, Models.MstCompany company)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var companies = from d in db.MstCompanies where d.Id == Convert.ToInt32(id) select d;
                if (companies.Any())
                {
                    var updateCompany = companies.FirstOrDefault();
                    updateCompany.Company = company.Company;
                    updateCompany.Address = company.Address;
                    updateCompany.ContactNumber = company.ContactNumber;
                    updateCompany.TaxNumber = company.TaxNumber;
                    updateCompany.IsLocked = true;
                    updateCompany.UpdatedById = userId;
                    updateCompany.UpdatedDateTime = DateTime.Now;

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

        // unlock company
        [Authorize]
        [HttpPut]
        [Route("api/updateCompanyIsLock/{id}")]
        public HttpResponseMessage unlockCompany(String id, Models.MstCompany company)
        {
            try
            {
                var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();

                var companies = from d in db.MstCompanies where d.Id == Convert.ToInt32(id) select d;
                if (companies.Any())
                {
                    var updateCompany = companies.FirstOrDefault();

                    updateCompany.IsLocked = false;
                    updateCompany.UpdatedById = userId;
                    updateCompany.UpdatedDateTime = DateTime.Now;

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

        // delete company
        [Authorize]
        [HttpDelete]
        [Route("api/deleteCompany/{id}")]
        public HttpResponseMessage deleteCompany(String id)
        {
            try
            {
                var companies = from d in db.MstCompanies where d.Id == Convert.ToInt32(id) select d;
                if (companies.Any())
                {
                    db.MstCompanies.DeleteOnSubmit(companies.First());
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
