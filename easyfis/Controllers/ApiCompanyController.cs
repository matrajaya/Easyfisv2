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

        // ============
        // LIST Company
        // ============
        [Route("api/listCompany")]
        public List<Models.MstCompany> Get()
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
                                    CreatedBy = d.MstUser.UserName,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedById = d.UpdatedById,
                                    UpdatedBy = d.MstUser1.UserName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                };
            return companies.ToList();
        }

        // ==================
        // LIST Company by Id
        // ==================
        [Route("api/company/{id}")]
        public Models.MstCompany GetCompany(String id)
        {
            var companyId = Convert.ToInt32(id);
            var company = from d in db.MstCompanies
                          where d.Id == companyId
                          select new Models.MstCompany
                              {
                                  Id = d.Id,
                                  Company = d.Company,
                                  Address = d.Address,
                                  ContactNumber = d.ContactNumber,
                                  TaxNumber = d.TaxNumber,
                                  IsLocked = d.IsLocked,
                                  CreatedById = d.CreatedById,
                                  CreatedBy = d.MstUser.UserName,
                                  CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                  UpdatedById = d.UpdatedById,
                                  UpdatedBy = d.MstUser1.UserName,
                                  UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                              };

            return (Models.MstCompany)company.FirstOrDefault();
        }

        // ===========
        // ADD Company
        // ===========
        [Route("api/addCompany")]
        public int Post(Models.MstCompany company)
        {
            try
            {
                var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                Data.MstCompany newCompany = new Data.MstCompany();

                newCompany.Company = company.Company;
                newCompany.Address = company.Address;
                newCompany.ContactNumber = company.ContactNumber;
                newCompany.TaxNumber = company.TaxNumber;
                newCompany.IsLocked = isLocked;
                newCompany.CreatedById = mstUserId;
                newCompany.CreatedDateTime = date;
                newCompany.UpdatedById = mstUserId;
                newCompany.UpdatedDateTime = date;

                db.MstCompanies.InsertOnSubmit(newCompany);
                db.SubmitChanges();

                return newCompany.Id;

            }
            catch
            {
                return 0;
            }
        }

        // ==============
        // UPDATE Company
        // ==============
        [Route("api/updateCompany/{id}")]
        public HttpResponseMessage Put(String id, Models.MstCompany company)
        {
            try
            {
                var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var mstUserId = (from d in db.MstUsers where d.UserId == identityUserId select d.Id).SingleOrDefault();
                var date = DateTime.Now;

                var companyId = Convert.ToInt32(id);
                var companies = from d in db.MstCompanies where d.Id == companyId select d;

                if (companies.Any())
                {
                    var updateCompany = companies.FirstOrDefault();

                    updateCompany.Company = company.Company;
                    updateCompany.Address = company.Address;
                    updateCompany.ContactNumber = company.ContactNumber;
                    updateCompany.TaxNumber = company.TaxNumber;

                    updateCompany.IsLocked = isLocked;
                    updateCompany.UpdatedById = mstUserId;
                    updateCompany.UpdatedDateTime = date;

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

        // ==============
        // DELETE Company
        // ==============
        [Route("api/deleteCompany/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var companyId = Convert.ToInt32(id);
                var companies = from d in db.MstCompanies where d.Id == companyId select d;

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
