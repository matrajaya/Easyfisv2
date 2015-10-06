using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiCompanyController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        [Route("api/listCompany")]
        public List<Models.MstCompany> Get()
        {
            var companies = from d in db.MstCompanies select new Models.MstCompany
                { 
                    Id = d.Id, 
                    Company = d.Company,
                    Address = d.Address,
                    ContactNumber = d.ContactNumber,
                    TaxNumber = d.TaxNumber,
                    IsLocked = d.IsLocked,
                    CreatedById = d.CreatedById,
                    CreateDateTime = d.CreatedDateTime.ToShortDateString(),
                    UpdatedById = d.UpdatedById,
                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                };
            return companies.ToList();
        }

        [Route("api/companyById/{id}")]
        public Models.MstCompany GetCompany(String id)
        {
            var companyId = Convert.ToInt32(id);
            var company = from d in db.MstCompanies where d.Id == companyId select new Models.MstCompany
                {
                    Id = d.Id,
                    Company = d.Company,
                    Address = d.Address,
                    ContactNumber = d.ContactNumber,
                    TaxNumber = d.TaxNumber,
                    IsLocked = d.IsLocked,
                    CreatedById = d.CreatedById,
                    CreateDateTime = d.CreatedDateTime.ToShortDateString(),
                    UpdatedById = d.UpdatedById,
                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                };

            return (Models.MstCompany)company.FirstOrDefault();
        }

        [Route("api/addCompany")]
        public int Post(Models.MstCompany company)
        {
            try
            {
                Data.MstCompany newCompany = new Data.MstCompany();

                newCompany.Company = company.Company;
                newCompany.Address = company.Address;
                newCompany.ContactNumber = company.ContactNumber;
                newCompany.TaxNumber = company.TaxNumber;

                newCompany.IsLocked = company.IsLocked;
                newCompany.CreatedById = company.CreatedById;
                newCompany.CreatedDateTime = Convert.ToDateTime(company.CreateDateTime);
                newCompany.UpdatedById = company.UpdatedById;
                newCompany.UpdatedDateTime = Convert.ToDateTime(company.UpdatedDateTime);

                db.MstCompanies.InsertOnSubmit(newCompany);
                db.SubmitChanges();

                return newCompany.Id;

            }
            catch
            {
                return 0;
            }
        }

        [Route("api/updateCompany/{id}")]
        public HttpResponseMessage Put(String id, Models.MstCompany company)
        {
            try
            {
                var companyId = Convert.ToInt32(id);
                var companies = from d in db.MstCompanies where d.Id == companyId select d;

                if(companies.Any())
                {
                    var updateCompany = companies.FirstOrDefault();

                    updateCompany.Company = company.Company;
                    updateCompany.Address = company.Address;
                    updateCompany.ContactNumber = company.ContactNumber;
                    updateCompany.TaxNumber = company.TaxNumber;

                    //updateCompany.IsLocked = company.IsLocked;
                    //updateCompany.CreatedById = company.CreatedById;
                    //updateCompany.CreatedDateTime = Convert.ToDateTime(company.CreateDateTime);
                    //updateCompany.UpdatedById = company.UpdatedById;
                    //updateCompany.UpdatedDateTime = Convert.ToDateTime(company.UpdatedDateTime);
                    
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
